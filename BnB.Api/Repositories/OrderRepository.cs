using BnB.Api.Data;
using BnB.Api.Dto.Order;
using BnB.Api.Enums;
using BnB.Api.Interfaces;
using BnB.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BnB.Api.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _db;

    public OrderRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Order>> GetOrders()
    {
        var orders = await _db.Orders.ToListAsync();
        return orders;
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserId(string userId)
    {
        var orders = await _db.Orders.Where(o => o.UserId == userId).ToListAsync();
        return orders;
    }

    public async Task<Order> CreateOrder(Order createOrder)
    {
        createOrder.Id = Guid.NewGuid().ToString();
        createOrder.Status = OrderStatus.PENDING;
        createOrder.CreatedAt = DateTime.Now;
        createOrder.UpdatedAt = DateTime.Now;
        _db.Orders.Add(createOrder);


        await _db.SaveChangesAsync();
        return createOrder;
    }

    public async Task<Order> UpdateOrder(string orderId, UpdateOrderDto updateOrder)
    {
        var order = await _db.Orders.FirstOrDefaultAsync(p => p.Id == orderId);

        if (order is null)
        {
            return null;
        }
        
        order.Status = updateOrder.Status;
        order.UpdatedAt = DateTime.Now;
        await _db.SaveChangesAsync();

        return order;
    }
}