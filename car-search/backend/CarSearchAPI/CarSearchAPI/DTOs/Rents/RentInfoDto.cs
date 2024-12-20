using CarSearchAPI.Models;
using CarSearchAPI.Enums;

namespace CarSearchAPI.DTOs.Rents
{
    public class RentInfoDto
    {
        public int RentId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public RentStatuses Status { get; set; }
    }
}
