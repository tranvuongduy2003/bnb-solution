using BnB.Api.Dto.Statistic;

namespace BnB.Api.Interfaces;

public interface IStatisticRepository
{
    Task<IEnumerable<TotalProfit>> GetProfitStatistic();
    Task<IEnumerable<TotalOrder>> GetOrdersStatistic();
    Task<IEnumerable<RevenueStatistic>> GetRevenuesStatistic();
    Task<IEnumerable<TimelineStatistic>> GetOrderInTimeline();
}