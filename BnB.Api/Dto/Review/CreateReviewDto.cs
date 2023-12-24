using System.ComponentModel.DataAnnotations;

namespace BnB.Api.Dto.Review;

public class CreateReviewDto
{
    [Required]
    public string UserId { get; set; }
    public string Content { get; set; } = String.Empty;
    [Required]
    public double Rating { get; set; }
    [Required]
    public string ProductId { get; set; }
}