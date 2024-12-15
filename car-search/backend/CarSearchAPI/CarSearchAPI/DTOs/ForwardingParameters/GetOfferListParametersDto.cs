namespace CarSearchAPI.DTOs.ForwardingParameters;

public class GetOfferListParametersDto
{
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Location { get; set; }
    public string? Email { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}