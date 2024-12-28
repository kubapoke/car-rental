using CarRentalAPI.Abstractions;

namespace CarRentalAPI.Services.PriceGenerators;

public class PricePerDayToHourGenerator : IPriceGenerator
{
    public decimal GeneratePrice(decimal basePrice, TimeSpan time)
    {
        return basePrice * (1m + time.Days + ((decimal)time.Hours / 24m));
    }
}