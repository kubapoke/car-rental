using CarRentalAPI.Abstractions;

namespace CarRentalAPI.Services;

public class PricePerDayToHourGeneratorService : IPriceGenerator
{
    // implementation assumes, that price given is per day, but the result is given with accuracy to an hour
    public decimal GeneratePrice(decimal basePrice, TimeSpan time)
    {
        return basePrice * (decimal)time.Hours / 24m;
    }
}