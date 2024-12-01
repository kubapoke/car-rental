using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalAPI.Models;

public enum RentStatus
{
    Active = 1,
    ReadyToReturn = 2,
    Returned = 3
}

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
    public RentStatus Status { get; set; }

    [Required]
    public DateTime RentStart { get; set; }
    
    public DateTime? RentEnd { get; set; }
}