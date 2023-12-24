using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace BnB.Api.Models;

public class Image
{
    [Key]
    public string Id { get; set; }
    [Required]
    public string ProductId { get; set; }
    public string Url { get; set; }
    
    [NotMapped]
    [ForeignKey("ProductId")]
    public virtual Product Product { get; set; }
}