using System.ComponentModel.DataAnnotations;

namespace BnB.Api.Dto.Product;

public class CreateProductDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Desc { get; set; }
    public decimal Price { get; set; }
    public int Inventory { get; set; }
    public decimal ImportPrice { get; set; }
    [Required]
    public int BrandId { get; set; }
    [Required]
    public int CategoryId { get; set; }
    public int Sold { get; set; }
    public IEnumerable<string>? Images { get; set; }
    
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}