using BnB.Api.Data;
using BnB.Api.Dto.Review;
using BnB.Api.Interfaces;
using BnB.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BnB.Api.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly ApplicationDbContext _db;

    public ReviewRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<DetailReview>> GetReviewsByProductId(string productId)
    {
        var reviews = await _db.Reviews.Where(r => r.ProductId == productId).Join(_db.Users, review => review.UserId,
            user => user.Id, (
                review, user) => new DetailReview
            {
                Id = review.Id,
                UserId = review.UserId,
                Content = review.Content,
                Rating = review.Rating,
                Avatar = user.Avatar,
                Fullname = user.Fullname,
                ProductId = review.ProductId,
                CreatedAt = review.CreatedAt,
                UpdatedAt = review.UpdatedAt,
            }).ToListAsync();
        return reviews;
    }

    public async Task<Review> CreateReview(CreateReviewDto createReview)
    {
        var review = new Review
        {
            Id = Guid.NewGuid().ToString(),
            Content = createReview.Content,
            Rating = createReview.Rating,
            UserId = createReview.UserId,
            ProductId = createReview.ProductId,
        };
        
        review.CreatedAt = DateTime.Now;
        review.UpdatedAt = DateTime.Now;
        var newReview = await _db.Reviews.AddAsync(review);
        await _db.SaveChangesAsync();
        return newReview.Entity;
    }
}