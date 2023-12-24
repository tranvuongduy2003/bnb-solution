using BnB.Api.Data;
using BnB.Api.Dto.OrderItem;
using BnB.Api.Interfaces;
using BnB.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BnB.Api.Repositories;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly ApplicationDbContext _db;

    public OrderItemRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<DetailOrderItem>> GetOrderItemsByOrderId(string orderId)
    {
        var orderItems = await _db.OrderItems.Where(oi => oi.OrderId == orderId).Join(_db.Products,
            item => item.ProductId, product => product.Id,
            (item, product) =>
                new DetailOrderItem
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UpdatedAt = item.UpdatedAt,
                    OrderId = item.OrderId,
                    CreatedAt = item.CreatedAt,
                    SumPrice = item.SumPrice,
                    ProductImage = _db.Images.FirstOrDefault(i => i.ProductId == product.Id).Url,
                    ProductName = product.Name,
                    ProductPrice = product.Price
                }).ToListAsync();
        return orderItems;
    }
    public async Task<IEnumerable<OrderItem>> CreateOrderItems(IEnumerable<OrderItem> createOrderItems)
    {
        foreach (var orderItem in createOrderItems)
        {
            var product = _db.Products.FirstOrDefault(p => p.Id == orderItem.ProductId);
            
            orderItem.Id = Guid.NewGuid().ToString();
            orderItem.SumPrice = product.Price * orderItem.Quantity;
            orderItem.CreatedAt = DateTime.Now;
            orderItem.UpdatedAt = DateTime.Now;
            
            product.Sold += orderItem.Quantity;
            product.Inventory -= orderItem.Quantity;
            product.UpdatedAt = DateTime.Now;
        }

        _db.OrderItems.AddRange(createOrderItems);
        await _db.SaveChangesAsync();
        
        return createOrderItems;
    }
}