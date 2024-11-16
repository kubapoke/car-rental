using System.ComponentModel.DataAnnotations;

namespace CarSearchAPI.Models
{
    public class ApplicationUser
    {
        [Key] // this will tell EF that it is PK
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Surname { get; set; }

        [Required]
        public DateOnly BirthDate { get; set; }

        [Required]
        public DateOnly LicenceDate { get; set; }

        
        // we will add the location and other properties in the future (when we will now more about them)
        
    }
}
