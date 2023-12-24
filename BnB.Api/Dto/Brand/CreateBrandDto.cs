using System.ComponentModel.DataAnnotations;

namespace BnB.Api.Dto.Brand;

public class CreateBrandDto
{
    [Required]
    public string Name { get; set; }
}