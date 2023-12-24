using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using BnB.Api.Enums;


public class RegistrationRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [MaxLength(45)]
    public string Fullname { get; set; }
    [Required]
    [MinLength(8)]
    [MaxLength(32)]
    public string Password { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Role? Role { get; set; }
}