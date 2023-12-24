using BnB.Api.Dto.Order;
using BnB.Api.Dto.Statistic;
using BnB.Api.Enums;
using BnB.Api.Models;

namespace BnB.Api.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetOrders();
    Task<IEnumerable<Order>> GetOrdersByUserId(string userId);
    Task<Order> CreateOrder(Order createOrder);
    Task<Order> UpdateOrder(string orderId, UpdateOrderDto updateOrder);
}