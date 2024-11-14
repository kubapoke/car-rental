namespace CarRentalAPI.Abstractions;

public interface IPriceGenerator
{
    public decimal GeneratePrice(decimal basePrice, TimeSpan time);

    public async Task<decimal> GeneratePriceAsync(decimal basePrice, TimeSpan time)
    {
        return await Task.FromResult(GeneratePrice(basePrice, time));
    }
    public decimal GeneratePrice(decimal basePrice, DateTime startDate, DateTime endDate)
    {
        return GeneratePrice(basePrice, endDate.Subtract(startDate));
    }

    public async Task<decimal> GeneratePriceAsync(decimal basePrice, DateTime startDate, DateTime endDate)
    {
        return await Task.FromResult(GeneratePrice(basePrice, endDate.Subtract(startDate)));
    }
}