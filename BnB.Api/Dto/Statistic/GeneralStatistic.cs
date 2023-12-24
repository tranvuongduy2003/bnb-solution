namespace BnB.Api.Dto.Statistic;

public class GeneralStatistic
{
    public int TotalProducts { get; set; }
    public int TotalCustomers { get; set; }
    public IEnumerable<TotalOrder> TotalOrder { get; set; }
    public IEnumerable<TotalProfit> TotalProfit { get; set; }
}