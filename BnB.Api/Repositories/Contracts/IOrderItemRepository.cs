using BnB.Api.Dto.OrderItem;
using BnB.Api.Dto.Statistic;
using BnB.Api.Models;

namespace BnB.Api.Interfaces;

public interface IOrderItemRepository
{
    Task<IEnumerable<DetailOrderItem>> GetOrderItemsByOrderId(string orderId);
    Task<IEnumerable<OrderItem>> CreateOrderItems(IEnumerable<OrderItem> createOrderItems);
}