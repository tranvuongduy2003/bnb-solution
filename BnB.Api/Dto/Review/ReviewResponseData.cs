namespace BnB.Api.Dto.Review;

public class ReviewResponseData
{
    public IEnumerable<RatingPoint> RatingPoint { get; set; }
    public IEnumerable<ReviewDto> Reviews { get; set; }
}