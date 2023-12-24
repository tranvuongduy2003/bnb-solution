using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BnB.Api.Models;

public class Product
{
    [Key]
    public string Id { get; set; }
    [Required]
    [StringLength(45)]
    public string Name { get; set; }
    [Required]
    [StringLength(225)]
    public string Desc { get; set; }
    [Required]
    public decimal Price { get; set; }
    public int BrandId { get; set; }
    [Required]
    public decimal ImportPrice { get; set; }
    public int CategoryId { get; set; }
    [Required]
    public int Inventory { get; set; }
    public int Sold { get; set; } = 0;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    [NotMapped]
    [ForeignKey("CategoryId")]
    public virtual Category Category { get; set; }
    [NotMapped]
    [ForeignKey("BrandId")]
    public virtual Brand Brand { get; set; }
}