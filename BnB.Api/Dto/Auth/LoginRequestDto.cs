using System.ComponentModel.DataAnnotations;

namespace BnB.Api.Dto;

public class LoginRequestDto
{
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set; }
}