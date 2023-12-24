using System.ComponentModel.DataAnnotations;

namespace BnB.Api.Dto.Order;

public class CreateOrderDto
{
    [Required]
    public string ReceiptAddress { get; set; }
    [Required]
    public string ReceiptName { get; set; }
    [Required]
    public string ReceiptPhone { get; set; }
    [Required] public string UserId { get; set; }
    public IEnumerable<OrderProduct>? Products { get; set; }
}