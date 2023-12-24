using AutoMapper;
using BnB.Api.Dto;
using BnB.Api.Dto.Statistic;
using BnB.Api.Enums;
using BnB.Api.Interfaces;
using BnB.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;

namespace BnB.Api.Controllers
{
    [Route("general")]
    [ApiController]
    public class GeneralController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly UserManager<User> _userManager;
        private readonly IStatisticRepository _statisticRepository;
        private readonly IMapper _mapper;
        protected ResponseDto _response;

        public GeneralController(IProductRepository productRepository, UserManager<User> userManager,
            IStatisticRepository statisticRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _userManager = userManager;
            _statisticRepository = statisticRepository;
            _mapper = mapper;
            _response = new();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistic()
        {
            try
            {
                var totalProducts = _productRepository.GetProducts().GetAwaiter().GetResult().Count();
                var totalCustomers = _userManager.GetUsersInRoleAsync(Role.CUSTOMER.GetDisplayName()).GetAwaiter()
                    .GetResult().Count();
                var totalOrders = _statisticRepository.GetOrdersStatistic().GetAwaiter().GetResult();
                var totalProfit = _statisticRepository.GetProfitStatistic().GetAwaiter().GetResult();


                _response.Data = new GeneralStatistic
                {
                    TotalProducts = totalProducts,
                    TotalCustomers = totalCustomers,
                    TotalOrder = totalOrders,
                    TotalProfit = totalProfit,
                };
                _response.Message = "Get the statistic successfully!";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message.ToString();
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }

            return Ok(_response);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("revenue-by-category")]
        public async Task<IActionResult> GetRevenueByCategory()
        {
            try
            {
                var revenues = await _statisticRepository.GetRevenuesStatistic();
                
                _response.Data = revenues;
                _response.Message = "Get the revenue successfully!";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message.ToString();
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }

            return Ok(_response);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("order-in-timeline")]
        public async Task<IActionResult> GetOrderInTimeline()
        {
            try
            {
                var orders = await _statisticRepository.GetOrderInTimeline();
                
                _response.Data = orders;
                _response.Message = "Get the orders successfully!";
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