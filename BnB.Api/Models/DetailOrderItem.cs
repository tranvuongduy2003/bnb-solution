namespace BnB.Api.Dto.OrderItem;

public class DetailOrderItem
{
    public string Id { get; set; }
    public int Quantity { get; set; }
    public string ProductId { get; set; }
    public string OrderId { get; set; }
    public string ProductName { get; set; }
    public string ProductImage { get; set; }
    public decimal ProductPrice { get; set; }
    public decimal? SumPrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}