using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarSearchAPI.Models
{
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
