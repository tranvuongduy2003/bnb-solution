using BnB.Api.Dto.Review;
using BnB.Api.Models;

namespace BnB.Api.Interfaces;

public interface IReviewRepository
{
    Task<IEnumerable<DetailReview>> GetReviewsByProductId(string productId);
    Task<Review> CreateReview(CreateReviewDto createReview);
}