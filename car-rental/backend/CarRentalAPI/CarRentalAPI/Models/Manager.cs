using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Models
{
    public class Manager
    {
        [Key]
        [Required]
        public int ManagerId { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        [StringLength(256)]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(256)]
        public string Salt { get; set; }
    }
}
