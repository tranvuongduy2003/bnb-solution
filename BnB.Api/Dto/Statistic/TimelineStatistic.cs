using BnB.Api.Dto.Order;

namespace BnB.Api.Dto.Statistic;

public class TimelineStatistic
{
    public DateTime Date { get; set; }
    public IEnumerable<object> Orders { get; set; }
}