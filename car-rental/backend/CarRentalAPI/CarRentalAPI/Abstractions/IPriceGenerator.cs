namespace CarRentalAPI.Abstractions;

public interface IPriceGenerator
{
    public decimal GeneratePrice(decimal basePrice, TimeSpan time);

    public decimal GeneratePrice(decimal basePrice, DateTime startDate, DateTime endDate)
    {
        return GeneratePrice(basePrice, endDate.Subtract(startDate));
    }
}