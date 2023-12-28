using System.Text;
using BnB.Api.Dto;
using BnB.Api.Dto.Product;
using BnB.Api.Services.IServices;
using Newtonsoft.Json;

namespace BnB.Api.Services;

public class RecommendService : IRecommendService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public RecommendService(IHttpClientFactory clientFactory)
    {
        _httpClientFactory = clientFactory;
    }

    public async Task<IEnumerable<ProductDto>> GetRecommendedProducts()
    {
        var client = _httpClientFactory.CreateClient("Product");
        var response = await client.GetAsync($"/recommend_products/1");
        var apiContent = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<RecommendedResponseDto>(apiContent);
        if (resp.recommended_products != null)
        {
            return resp.recommended_products;
        }

        return new List<ProductDto>();
    }
}