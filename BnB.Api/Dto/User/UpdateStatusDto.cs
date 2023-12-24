using Microsoft.Build.Framework;

namespace BnB.Api.Dto;

public class UpdateStatusDto
{
    [Required]
    public bool IsActive { get; set; }
}