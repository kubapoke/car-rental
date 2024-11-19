using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Models;

public class Brand
{
    [Key]
    [Required]
    public int BrandId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    
    public ICollection<Model> Models { get; set; }
}