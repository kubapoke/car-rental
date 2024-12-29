using CarRentalAPI.Abstractions;
using CarRentalAPI.DTOs.Redis;
using CarRentalAPI.Models;
using CarRentalAPI.Repositories.Abstractions;

namespace CarRentalAPI.Services.CarServices;

public class CarLookupService : ICarLookupService
{
    private readonly ICarRepository _carRepository;

    public CarLookupService(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }
    
    public async Task<Car?> GetCarOrNullFromOfferAsync(CachedOfferDto offer)
    {
        var car = await _carRepository.GetCarOrNullByIdAsync(offer.CarId);

        return car;
    }
}