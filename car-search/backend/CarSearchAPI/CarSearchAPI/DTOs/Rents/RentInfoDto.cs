using CarSearchAPI.Models;

namespace CarSearchAPI.DTOs.Rents
{
    public class RentInfoDto
    {
        public int RentId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public RentStatus Status { get; set; }
    }
}
