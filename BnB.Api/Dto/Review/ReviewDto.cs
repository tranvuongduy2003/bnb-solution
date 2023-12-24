namespace BnB.Api.Dto.Review;

public class ReviewDto
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string Content { get; set; }
    public double Rating { get; set; }
    public string? Avatar { get; set; }
    public string? Fullname { get; set; }
    public int ProductId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}