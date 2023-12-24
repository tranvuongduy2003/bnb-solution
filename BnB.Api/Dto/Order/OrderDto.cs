using System.Text.Json.Serialization;
using BnB.Api.Dto.OrderItem;
using BnB.Api.Enums;

namespace BnB.Api.Dto.Order;

public class OrderDto
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string ReceiptAddress { get; set; }
    public string ReceiptName { get; set; }
    public string ReceiptPhone { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OrderStatus Status { get; set; }
    public IEnumerable<OrderItemDto>? OrderItems { get; set; }
    
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}