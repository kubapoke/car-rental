using CarRentalAPI.Abstractions;
using CarRentalAPI.DTOs.CarSearch;
using CarRentalAPI.DTOs.Redis;
using CarRentalAPI.Models;
using CarRentalAPI.Repositories.Abstractions;

namespace CarRentalAPI.Services.CarServices
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;

        public CarService(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }
        
        public async Task<List<Car>> GetAllCarsAsync()
        {
            return await _carRepository.GetAllCarsAsync();
        }

        public async Task<List<CarInfoDto>> GetAllDistinctCarTypesAsync()
        {
            var cars = await GetAllCarsAsync();
            
            var distinctCars = cars
                .GroupBy(car => new { ModelName = car.Model.Name, BrandName = car.Model.Brand.Name, Location = car.Location })  // Group by Model and Brand Name
                .Select(group => new CarInfoDto()
                {
                    ModelName = group.Key.ModelName,
                    BrandName = group.Key.BrandName,
                    Location = group.Key.Location,
                    IsActive = group.Any(car => car.IsActive) // True if any car of this model is active in this location
                })
                .ToList();
            
            return distinctCars;
        }

        public async Task<Car?> GetCarOrNullFromOfferAsync(CachedOfferDto offer)
        {
            var car = await _carRepository.GetCarOrNullByIdAsync(offer.CarId);

            return car;
        }
    }
}