using System.Security.Claims;
using AutoMapper;
using BnB.Api.Dto;
using BnB.Api.Dto.Auth;
using BnB.Api.Enums;
using BnB.Api.Interfaces;
using BnB.Api.Models;
using BnB.Api.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Extensions;

namespace BnB.Api.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        protected ResponseDto _response;

        public AuthController(IUserRepository userRepository, UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager, ITokenService tokenService, IMapper mapper)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _response = new();
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            try
            {
                var useWithEmailExisted = await _userRepository.GetUserByEmail(registrationRequestDto.Email);

                if (useWithEmailExisted != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Email exists";
                    return BadRequest(_response);
                }


                User user = new()
                {
                    UserName = registrationRequestDto.Email,
                    Email = registrationRequestDto.Email,
                    Fullname = registrationRequestDto.Fullname,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Password = BCrypt.Net.BCrypt.HashPassword(registrationRequestDto.Password)
                };

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    var userToReturn = await _userRepository.GetUserByEmail(registrationRequestDto.Email);

                    if (!_roleManager.RoleExistsAsync(registrationRequestDto.Role.GetDisplayName()).GetAwaiter()
                            .GetResult())
                    {
                        _roleManager.CreateAsync(new IdentityRole(registrationRequestDto.Role.GetDisplayName()))
                            .GetAwaiter().GetResult();
                    }

                    await _userManager.AddToRoleAsync(user, registrationRequestDto.Role.GetDisplayName());

                    UserDto userDto = new()
                    {
                        Email = userToReturn.Email,
                        Id = userToReturn.Id,
                        Fullname = userToReturn.Fullname,
                    };

                    _response.Message = "Register new account successfully!";
                }
                else
                {
                    throw new Exception(result.Errors.FirstOrDefault().Description);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message.ToString();
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }

            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            try
            {
                var user = await _userRepository.GetUserByEmail(loginRequestDto.Email);

                var isValid = BCrypt.Net.BCrypt.Verify(loginRequestDto.Password, user.Password);

                if (user == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Tài khoản không tồn tại";
                    return Unauthorized(_response);
                }

                if (isValid == false)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Mật khẩu không tồn tại";
                    return Unauthorized(_response);
                }

                //If user was found , Generate JWT Token
                var roles = await _userManager.GetRolesAsync(user);
                var accessToken = _tokenService.GenerateAccessToken(user, roles);
                var refreshToken = _tokenService.GenerateRefreshToken();

                var token = new TokenDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };

                _userRepository.UpdateRefreshToken(user.Id, refreshToken);

                var userRoles = await _userManager.GetRolesAsync(user);

                var userDto = _mapper.Map<UserDto>(user);
                userDto.Role = Enum.Parse<Role>(userRoles.FirstOrDefault());

                LoginResponseDto loginResponseDto = new LoginResponseDto()
                {
                    User = userDto,
                    Token = token
                };

                _response.Data = loginResponseDto;
                _response.Message = "Đăng nhập thành công";
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
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto tokenRequestDto)
        {
            try
            {
                var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

                if (tokenRequestDto.RefreshToken == "")
                {
                    _response.IsSuccess = false;
                    _response.Message = "Unaccepted token";
                    return BadRequest(_response);
                }

                if (accessToken == "")
                {
                    _response.IsSuccess = false;
                    _response.Message = "Unauthorized";
                    return Unauthorized(_response);
                }


                var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
                if (principal == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Unauthorized";
                    return Unauthorized(_response);
                }

                var userEmail = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
                var user = await _userRepository.GetUserByEmail(userEmail);
                if (user.RefreshToken != tokenRequestDto.RefreshToken ||
                    !_tokenService.ValidateTokenExpire(tokenRequestDto.RefreshToken))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Unauthorized";
                    return Unauthorized(_response);
                }

                var roles = await _userManager.GetRolesAsync(user);
                var newAccessToken = _tokenService.GenerateAccessToken(user, roles);


                if (newAccessToken == "")
                {
                    _response.IsSuccess = false;
                    _response.Message = "Unauthorized";
                    return Unauthorized(_response);
                }

                _response.Data = newAccessToken;
                _response.Message = "Refresh token successfully!";
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