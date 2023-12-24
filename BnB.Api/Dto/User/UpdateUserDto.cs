using System.Text.Json.Serialization;
using BnB.Api.Enums;

namespace BnB.Api.Dto;

public class UpdateUserDto
{
    public string? Fullname { get; set; }
    public string? Phone { get; set; }
    public DateTime? DOB { get; set; }
    public bool? IsActive { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Role? Role { get; set; }
    public string? Avatar { get; set; }
}