using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalAPI.Models;

public class Car
{
    [Key]
    [Required]
    public int CarId { get; set; }
    
    [ForeignKey("ModelId")]
    [Required]
    public Model Model { get; set; }
    public int ModelId { get; set; }
    
    [Required]
    public int Mileage { get; set; }
    
    [Required]
    public bool IsActive { get; set; }
    
    // add location?

    public ICollection<Rent> Rents;
}