using System.ComponentModel.DataAnnotations;

namespace BnB.Api.Dto.Auth;

public class RefreshTokenRequestDto
{
    public string RefreshToken { get; set; }
}