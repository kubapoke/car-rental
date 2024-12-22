using CarRentalAPI.Enums;
using CarRentalAPI.Models;

namespace CarRentalAPI.DTOs.Rents;

public class RentInfoDto
{
    public int RentId { get; set; }
    public string ModelName { get; set; }
    public string BrandName { get; set; }
    public DateTime RentStart { get; set; }
    public DateTime? RentEnd { get; set; }
    public RentStatuses RentStatus { get; set; }
    public string? ImageUri { get; set; }
    public string? Description { get; set; }
}