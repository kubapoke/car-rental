using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalAPI.Models;

public class Rent
{
    [Key] 
    [Required] 
    public int RentId { get; set; }
    
    [ForeignKey("CarId")]
    [Required]
    public Car Car { get; set; }
    public int CarId { get; set; }
    
    [Required]
    [MaxLength(30)]
    public string UserEmail { get; set; }

    [Required]
    public int Status { get; set; }

    [Required]
    public DateOnly RentStart { get; set; }
    
    public DateOnly? RentEnd { get; set; }

    [Required]
    public Int16 RentState { get; set; } // Egor: I think that, it is better to have this redundancy (RentEnd + isActive), to reduce query time when checking active/ended rents 
}