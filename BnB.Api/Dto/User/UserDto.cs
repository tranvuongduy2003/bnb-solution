using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BnB.Api.Enums;

namespace BnB.Api.Dto;

public class UserDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string Fullname { get; set; }
    public string? Phone { get; set; }
    public DateTime? DOB { get; set; }
    public bool IsActive { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Role Role { get; set; }
    public string? Avatar { get; set; }
    
    public int? OrderCount { get; set; }
    public decimal? TotalPayment { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}