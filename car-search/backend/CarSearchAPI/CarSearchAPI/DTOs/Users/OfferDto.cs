﻿namespace CarSearchAPI.DTOs.Users;

public class OfferDto
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public string Conditions { get; set; }
    public string CompanyName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}