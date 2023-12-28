using BnB.Api.Dto.Product;

namespace BnB.Api.Services.IServices;

public interface IRecommendService
{
    Task<IEnumerable<ProductDto>> GetRecommendedProducts();
}