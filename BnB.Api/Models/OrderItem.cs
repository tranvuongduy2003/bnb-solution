using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BnB.Api.Dto.Order;
using BnB.Api.Dto.Product;

namespace BnB.Api.Models;

public class OrderItem
{
    [Key]
    public string Id { get; set; }
    [Required]
    public int Quantity { get; set; }
    [Required]
    public string ProductId { get; set; }
    public string OrderId { get; set; }
    public decimal? SumPrice { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    [NotMapped]
    [ForeignKey("OrderId")]
    public virtual Order? Order { get; set; }
    [NotMapped] 
    [ForeignKey("ProductId")] 
    public virtual Product? Product { get; set; }
}