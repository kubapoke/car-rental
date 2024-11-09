using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalAPI.Models;

public class Model
{
    [Key]
    [Required]
    public int ModelID { get; set; }
    
    [ForeignKey("BrandId")]
    [Required]
    public Brand Brand { get; set; }
    public int BrandId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    
    [StringLength(4)]
    public string Year { get; set; }
    
    public ICollection<Car> Cars { get; set; }
}