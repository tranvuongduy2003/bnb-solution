using BnB.Api.Enums;

namespace BnB.Api.Dto.Order;

public class UpdateOrderDto
{
    public OrderStatus Status { get; set; }
}