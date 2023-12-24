using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BnB.Api.Models;

public class User : IdentityUser
{
    [Required]
    public string Fullname { get; set; }
    [Required]
    public string Password { get; set; }
    public DateTime? DOB { get; set; }
    public bool IsActive { get; set; } = true;
    [StringLength(500)]
    public string? Avatar { get; set; } 
    public string? RefreshToken { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}