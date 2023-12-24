using System.Security.Claims;
using BnB.Api.Models;

namespace BnB.Api.Services.IServices;

public interface ITokenService
{
    string GenerateAccessToken(User user, IEnumerable<string> roles);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    bool ValidateTokenExpire(string token);
}