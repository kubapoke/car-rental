using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
    
    [Required]
    public string Location { get; set; }

    public ICollection<Rent> Rents;
}