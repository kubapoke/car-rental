using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarSearchAPI.Models
{
    public enum RentStatus
    {
        // This is different than the RentStatus in CarRental which has an additional "ReadyToReturn" state
        Active = 1,
        Returned = 2
    }

    
    public class Rent
    {
        [Key]
        [Required]
        public int RentId { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]        
        public string Model { get; set; }

        [ForeignKey("UserEmail")]
        [Required]
        public ApplicationUser RentUser { get; set; }
        public string UserEmail { get; set; }
        
        [Required]
        public RentStatus Status { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}
