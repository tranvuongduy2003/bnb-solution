namespace BnB.Api.Dto.Product;

public class ProductDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Desc { get; set; }
    public decimal Price { get; set; }
    public int BrandId { get; set; }
    public decimal ImportPrice { get; set; }
    public int CategoryId { get; set; }
    public int Inventory { get; set; }
    public int Sold { get; set; }
    public double? Rating { get; set; }
    public IEnumerable<string>? Images { get; set; }
    
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}