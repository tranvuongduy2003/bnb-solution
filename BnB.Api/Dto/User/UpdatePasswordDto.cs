using Microsoft.Build.Framework;

namespace BnB.Api.Dto;

public class UpdatePasswordDto
{
    [Required]
    public string OldPassword { get; set; }
    [Required]
    public string NewPassword { get; set; }
}