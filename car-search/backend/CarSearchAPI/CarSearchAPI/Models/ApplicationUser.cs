using System.ComponentModel.DataAnnotations;

namespace CarSearchAPI.Models
{
    public class ApplicationUser
    {
        [Key] // this will tell EF that it is PK
        [EmailAddress]
        [Required]
        public string email { get; set; }

        [Required]
        [MaxLength(50)]
        public string name { get; set; }

        [Required]
        [MaxLength(50)]
        public string surname { get; set; }

        [Required]
        public DateOnly birthDate { get; set; }

        [Required]
        public DateOnly licenceDate { get; set; }

        
        // we will add the location and other properties in the future (when we will now more about them)
        
    }
}
