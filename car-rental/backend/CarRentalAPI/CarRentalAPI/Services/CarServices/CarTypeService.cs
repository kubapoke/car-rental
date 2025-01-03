using CarRentalAPI.Abstractions;
using CarRentalAPI.DTOs.CarSearch;
using CarRentalAPI.Models;
using CarRentalAPI.Repositories.Abstractions;

namespace CarRentalAPI.Services.CarServices
{
    public class CarTypeService : ICarTypeService
    {
        private readonly ICarRepository _carRepository;

        public CarTypeService(ICarRepository carRepository)
        {
            _carRepository = carRepository;
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
                    IsActive = group.Any(car => car.IsActive)
                })
                .ToList();
            
            return distinctCars;
        }
        
        private async Task<List<Car>> GetAllCarsAsync()
        {
            return await _carRepository.GetAllCarsAsync();
        }
    }
}