using CarRentalAPI.Abstractions;

namespace CarRentalAPI.Services;

public class PricePerDayToHourGeneratorService : IPriceGenerator
{
    public decimal GeneratePrice(decimal basePrice, TimeSpan time)
    {
        return basePrice * (1m + time.Days + ((decimal)time.Hours / 24m));
    }
}