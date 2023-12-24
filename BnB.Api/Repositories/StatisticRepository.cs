using System.Collections;
using AutoMapper;
using BnB.Api.Data;
using BnB.Api.Dto.Order;
using BnB.Api.Dto.Statistic;
using BnB.Api.Interfaces;
using BnB.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BnB.Api.Repositories;

public class StatisticRepository : IStatisticRepository
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;
    private DateTime _startDate;

    public StatisticRepository(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
        _startDate = DateTime.Now.Subtract(TimeSpan.FromDays(7));
    }

    public async Task<IEnumerable<TotalProfit>> GetProfitStatistic()
    {
        var totalProfit = await _db.OrderItems
            .Join(_db.Orders, orderItem => orderItem.OrderId, order => order.Id, (orderItem, order) => new
            {
                Quantity = orderItem.Quantity,
                CreatedAt = order.CreatedAt,
                SumPrice = orderItem.SumPrice ?? 0,
                ProductId = orderItem.ProductId
            })
            .Join(_db.Products, joinedOrderItem => joinedOrderItem.ProductId, product => product.Id,
                (joinedOrderItem, product) => new
                {
                    Quantity = joinedOrderItem.Quantity,
                    CreatedAt = joinedOrderItem.CreatedAt,
                    SumPrice = joinedOrderItem.SumPrice,
                    ImportPrice = product.ImportPrice
                })
            .Where(oi => DateTime.Compare(
                oi.CreatedAt, _startDate) >= 0 && DateTime.Compare(oi.CreatedAt, DateTime.Now) <= 0)
            .GroupBy(oi => new DateTime(oi.CreatedAt.Year, oi.CreatedAt.Month, oi.CreatedAt.Day))
            .Select(oi => new TotalProfit
            {
                Date = oi.Key,
                Profit = oi.Sum(i => i.SumPrice - i.ImportPrice * i.Quantity),
                Revenue = oi.Sum(i => i.SumPrice)
            }).ToListAsync();

        return totalProfit;
    }

    public async Task<IEnumerable<TotalOrder>> GetOrdersStatistic()
    {
        var totalOrders = await _db.Orders
            .Where(o => DateTime.Compare(o.CreatedAt, _startDate) >= 0 && DateTime.Compare(o.CreatedAt, DateTime.Now) <= 0)
            .GroupBy(o => new DateTime(o.CreatedAt.Year, o.CreatedAt.Month, o.CreatedAt.Day))
            .Select(o => new TotalOrder
            {
                Date = o.Key,
                TotalOrders = o.Count(),
            }).ToListAsync();

        return totalOrders;
    }

    public async Task<IEnumerable<RevenueStatistic>> GetRevenuesStatistic()
    {
        var revenues = await _db.OrderItems
            .Join(_db.Orders, orderItem => orderItem.OrderId, order => order.Id, (orderItem, order) => new
            {
                CreatedAt = order.CreatedAt,
                SumPrice = orderItem.SumPrice ?? 0,
                ProductId = orderItem.ProductId
            })
            .Join(_db.Products, joinedOrderItem => joinedOrderItem.ProductId, product => product.Id,
                (joinedOrderItem, product) => new
                {
                    CreatedAt = joinedOrderItem.CreatedAt,
                    SumPrice = joinedOrderItem.SumPrice,
                    CategoryId = product.CategoryId
                })
            .Where(oi =>
                DateTime.Compare(oi.CreatedAt, _startDate) >= 0 && DateTime.Compare(oi.CreatedAt, DateTime.Now) <= 0)
            .GroupBy(oi => new
            {
                CreatedAt = new DateTime(oi.CreatedAt.Year, oi.CreatedAt.Month, oi.CreatedAt.Day),
                CategoryId = oi.CategoryId
            })
            .Select(oi => new RevenueStatistic
            {
                Date = oi.Key.CreatedAt,
                CategoryId = oi.Key.CategoryId,
                Revenue = oi.Sum(i => i.SumPrice)
            }).ToListAsync();

        return revenues;
    }

    public async Task<IEnumerable<TimelineStatistic>> GetOrderInTimeline()
    {
        var orders = await _db.Orders
            .Where(o =>
                DateTime.Compare(o.CreatedAt, _startDate) >= 0 && DateTime.Compare(o.CreatedAt, DateTime.Now) <= 0)
            .GroupBy(o => new DateTime(o.CreatedAt.Year, o.CreatedAt.Month, o.CreatedAt.Day))
            .Select(o => new TimelineStatistic
            {
                Date = o.Key.Date,
                Orders = o.ToList(),
            }).ToListAsync();

        return orders;
    }
}