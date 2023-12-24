namespace BnB.Api.Dto;

public class LoginResponseDto
{
    public UserDto User { get; set; }
    public TokenDto Token { get; set; }
}