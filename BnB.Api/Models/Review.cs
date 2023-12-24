using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BnB.Api.Dto;
using BnB.Api.Dto.Product;

namespace BnB.Api.Models;

public class Review
{
    [Key] public string Id { get; set; }
    [Required] public string UserId { get; set; }
    [Required] [StringLength(225)] public string Content { get; set; }
    [Required] public double Rating { get; set; }
    [Required] public string ProductId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    [NotMapped] [ForeignKey("ProductId")] public virtual Product? Product { get; set; }
    [NotMapped] [ForeignKey("UserId")] public virtual User? User { get; set; }
}