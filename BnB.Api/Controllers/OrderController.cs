using System.Security.Claims;
using AutoMapper;
using BnB.Api.Dto;
using BnB.Api.Dto.Order;
using BnB.Api.Dto.OrderItem;
using BnB.Api.Enums;
using BnB.Api.Interfaces;
using BnB.Api.Models;
using BnB.Api.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Extensions;

namespace BnB.Api.Controllers
{
    [Route("orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        protected ResponseDto _response;

        public OrderController(UserManager<User> userManager, IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository, IUserRepository userRepository, ITokenService tokenService,
            IMapper mapper)
        {
            _userManager = userManager;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _mapper = mapper;
            _response = new();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);

                if (principal == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Unauthorized";
                    return Unauthorized(_response);
                }

                var userEmail = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
                var user = await _userRepository.GetUserByEmail(userEmail);

                if (user is null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Unauthorized";
                    return Unauthorized(_response);
                }

                var userRoles = await _userManager.GetRolesAsync(user);

                if (userRoles.IsNullOrEmpty())
                {
                    _response.IsSuccess = false;
                    _response.Message = "Unauthorized";
                    return Unauthorized(_response);
                }

                var orders = userRoles.Contains(Role.CUSTOMER.GetDisplayName())
                    ? await _orderRepository.GetOrdersByUserId(user.Id)
                    : await _orderRepository.GetOrders();

                var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
                
                foreach (var orderDto in orderDtos)
                {
                    var orderItems = await _orderItemRepository.GetOrderItemsByOrderId((orderDto.Id));
                    orderDto.OrderItems = _mapper.Map<IEnumerable<OrderItemDto>>(orderItems);
                }

                _response.Data = orderDtos;
                _response.Message = "Get orders successfully!";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message.ToString();
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }

            return Ok(_response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            try
            {
                var newOrder = _mapper.Map<Order>(createOrderDto);
                var createdOrder = await _orderRepository.CreateOrder(newOrder);

                if (createdOrder is null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Failed";
                    return BadRequest(_response);
                }

                var orderItems = _mapper.Map<IEnumerable<OrderItem>>(createOrderDto.Products);
                
                foreach (var orderItem in orderItems)
                {
                    orderItem.OrderId = createdOrder.Id;
                }

                await _orderItemRepository.CreateOrderItems(orderItems);

                _response.Data = createdOrder;

                _response.Message = "Create new order successfully!";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message.ToString();
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }

            return Ok(_response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(string id, [FromBody] UpdateOrderDto updateOrderDto)
        {
            try
            {
                var updatedOrder = await _orderRepository.UpdateOrder(id, updateOrderDto);

                if (updatedOrder is null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Order not found";
                    return NotFound(_response);
                }

                _response.Message = "Update the order successfully!";
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