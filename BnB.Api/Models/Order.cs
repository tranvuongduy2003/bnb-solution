using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using BnB.Api.Dto;
using BnB.Api.Enums;

namespace BnB.Api.Models;

public class Order
{
    [Key]
    public string Id { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OrderStatus Status { get; set; }
    [Required]
    public string UserId { get; set; }
    [Required]
    [StringLength(225)]
    public string ReceiptAddress { get; set; }
    [Required]
    [StringLength(45)]
    public string ReceiptName { get; set; }
    [Required]
    [StringLength(45)]
    public string ReceiptPhone { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    [NotMapped] 
    [ForeignKey("UserId")] 
    public virtual User? User { get; set; }
}