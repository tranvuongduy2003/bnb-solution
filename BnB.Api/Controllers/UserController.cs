using System.Security.Claims;
using AutoMapper;
using BnB.Api.Dto;
using BnB.Api.Interfaces;
using BnB.Api.Models;
using BnB.Api.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Extensions;

namespace BnB.Api.Controllers
{
    [Route("users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        protected ResponseDto _response;

        public UserController(IOrderItemRepository orderItemRepository, IOrderRepository orderRepository,
            IUserRepository userRepository, UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager, ITokenService tokenService, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _response = new();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
                foreach (var user in userDtos)
                {
                    var orders = await _orderRepository.GetOrdersByUserId(user.Id);
                    decimal totalPayment = 0;
                    foreach (var order in orders)
                    {
                        var orderItems = await _orderItemRepository.GetOrderItemsByOrderId(order.Id);
                        totalPayment += orderItems.Sum(oi => oi.SumPrice) ?? 0;
                    }

                    user.OrderCount = orders.Count();
                    user.TotalPayment = totalPayment;
                }

                _response.Data = userDtos;
                _response.Message = "Get users successfully!";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message.ToString();
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }

            return Ok(_response);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto updateUserDto)
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
                var user = await _userManager.FindByEmailAsync(userEmail);

                if (user is null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Unauthorized";
                    return Unauthorized(_response);
                }

                if (updateUserDto.Phone != null)
                {
                    user.PhoneNumber = updateUserDto.Phone;
                }

                if (updateUserDto.Fullname != null)
                {
                    user.Fullname = updateUserDto.Fullname;
                }

                if (updateUserDto.DOB != null)
                {
                    user.DOB = updateUserDto.DOB;
                }

                if (updateUserDto.Avatar != null)
                {
                    user.Avatar = updateUserDto.Avatar;
                }

                user.UpdatedAt = DateTime.Now;

                var updatedUser = await _userManager.UpdateAsync(user);

                if (updatedUser is null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "User not found";
                    return NotFound(_response);
                }

                if (updateUserDto.Role != null)
                {
                    if (!_roleManager.RoleExistsAsync(updateUserDto.Role.GetDisplayName()).GetAwaiter()
                            .GetResult())
                    {
                        _roleManager.CreateAsync(new IdentityRole(updateUserDto.Role.GetDisplayName()))
                            .GetAwaiter().GetResult();
                    }

                    var userRoles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, userRoles);
                    await _userManager.AddToRoleAsync(user, updateUserDto.Role.GetDisplayName());
                }

                _response.Message = "Update the user successfully!";
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
        [HttpPatch("change-status/{userId}/{isActive:int}")]
        public async Task<IActionResult> UpdateUserStatus(string userId, int isActive)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user is null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Unauthorized";
                    return Unauthorized(_response);
                }

                user.IsActive = isActive == 1 ? true : false;
                
                user.UpdatedAt = DateTime.Now;

                await _userManager.UpdateAsync(user);

                _response.Message = "Update user status successfully!";
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
        [HttpPatch("change-password")]
        public async Task<IActionResult> UpdateUserPassword([FromBody] UpdatePasswordDto updatePasswordDto)
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
                var user = await _userManager.FindByEmailAsync(userEmail);

                if (user is null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "User not found";
                    return NotFound(_response);
                }

                bool isValid = BCrypt.Net.BCrypt.Verify(updatePasswordDto.OldPassword, user.Password);

                if (!isValid)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Old password is wrong";
                    return BadRequest(_response);
                }

                user.Password = BCrypt.Net.BCrypt.HashPassword(updatePasswordDto.NewPassword);

                await _userManager.UpdateAsync(user);

                _response.Message = "Change user password successfully!";
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