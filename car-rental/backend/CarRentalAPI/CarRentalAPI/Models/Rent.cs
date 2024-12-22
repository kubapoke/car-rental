using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarRentalAPI.Enums;

namespace CarRentalAPI.Models
{
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
        public RentStatuses Status { get; set; }

        [Required]
        public DateTime RentStart { get; set; }
    
        public DateTime? RentEnd { get; set; }

        [MaxLength(1024)]
        public string? Description { get; set; }
        public string? ImageUri {  get; set; }
    }
}