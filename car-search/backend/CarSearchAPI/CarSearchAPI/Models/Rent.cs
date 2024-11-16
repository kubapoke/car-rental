using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarSearchAPI.Models
{
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
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}
