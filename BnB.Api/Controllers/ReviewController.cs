using AutoMapper;
using BnB.Api.Dto;
using BnB.Api.Dto.Review;
using BnB.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BnB.Api.Controllers
{
    [Route("reviews")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        protected ResponseDto _response;

        public ReviewController(IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet("{productIdsssss}")]
        public async Task<IActionResult> GetReviews(string productId)
        {
            try
            {
                var reviews = await _reviewRepository.GetReviewsByProductId(productId);
                var reviewDtos = _mapper.Map<IEnumerable<ReviewDto>>(reviews);

                var totalPoints = new List<double>() { 0, 0, 0, 0, 0 };

                foreach (var reviewDto in reviewDtos)
                {
                    var rating = reviewDto.Rating;

                    if (rating <= 1)
                    {
                        totalPoints[0]++;
                    }
                    else if (rating <= 2)
                    {
                        totalPoints[1]++;
                    }
                    else if (rating <= 3)
                    {
                        totalPoints[2]++;
                    }
                    else if (rating <= 4)
                    {
                        totalPoints[3]++;
                    }
                    else if (rating <= 5)
                    {
                        totalPoints[4]++;
                    }
                }

                var totalRating = reviews.Count();

                _response.Data = new ReviewResponseData
                {
                    Reviews = reviewDtos,
                    RatingPoint = new List<RatingPoint>()
                    {
                        new RatingPoint { Level = 1, Percents = totalPoints[0] / totalRating },
                        new RatingPoint { Level = 2, Percents = totalPoints[1] / totalRating },
                        new RatingPoint { Level = 3, Percents = totalPoints[2] / totalRating },
                        new RatingPoint { Level = 4, Percents = totalPoints[3] / totalRating },
                        new RatingPoint { Level = 5, Percents = totalPoints[4] / totalRating }
                    }
                };
                _response.Message = "Get reviews successfully!";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message.ToString();
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }

            return Ok(_response);
        }
        
        [Authorize]
        [Authorize(Roles = "CUSTOMER")]
        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewDto createReviewDto)
        {
            try
            {
                // var newReview = _mapper.Map<Review>(createReviewDto);
                var createdReview = await _reviewRepository.CreateReview(createReviewDto);

                if (createdReview is null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Failed";
                    return BadRequest(_response);
                }

                var reviews = await _reviewRepository.GetReviewsByProductId(createdReview.ProductId);
                var reviewDtos = _mapper.Map<IEnumerable<ReviewDto>>(reviews);
                
                var totalPoints = new List<double>() { 0, 0, 0, 0, 0 };

                foreach (var reviewDto in reviewDtos)
                {
                    var rating = reviewDto.Rating;

                    if (rating <= 1)
                    {
                        totalPoints[0]++;
                    }
                    else if (rating <= 2)
                    {
                        totalPoints[1]++;
                    }
                    else if (rating <= 3)
                    {
                        totalPoints[2]++;
                    }
                    else if (rating <= 4)
                    {
                        totalPoints[3]++;
                    }
                    else if (rating <= 5)
                    {
                        totalPoints[4]++;
                    }
                }

                var totalRating = reviews.Count();

                _response.Data = new ReviewResponseData
                {
                    Reviews = reviewDtos,
                    RatingPoint = new List<RatingPoint>()
                    {
                        new RatingPoint { Level = 1, Percents = totalPoints[0] / totalRating },
                        new RatingPoint { Level = 2, Percents = totalPoints[1] / totalRating },
                        new RatingPoint { Level = 3, Percents = totalPoints[2] / totalRating },
                        new RatingPoint { Level = 4, Percents = totalPoints[3] / totalRating },
                        new RatingPoint { Level = 5, Percents = totalPoints[4] / totalRating }
                    }
                };
                _response.Message = "Create new review successfully!";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message.ToString();
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }

            return Ok(_response);
        }
    }
}