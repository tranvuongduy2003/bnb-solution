using System.ComponentModel.DataAnnotations;

namespace BnB.Api.Dto.Category;

public class CreateCategoryDto
{
    [Required]
    public string Name { get; set; }
    public string Desc { get; set; }
}