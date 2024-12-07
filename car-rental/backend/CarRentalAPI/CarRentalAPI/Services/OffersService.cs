using CarRentalAPI.Abstractions;
using CarRentalAPI.Abstractions.Repositories;
using CarRentalAPI.DTOs.CarSearch;
using CarRentalAPI.DTOs.Combinations;
using CarRentalAPI.Models;

namespace CarRentalAPI.Services
{
    public class OffersService
    {
        private readonly IRentRepository _rentRepository;
        private readonly ICarRepository _carRepository;
        private readonly AvailabilityChecker _availabilityChecker;
        private readonly IPriceGenerator _priceGenerator;

        public OffersService(IRentRepository rentRepository, ICarRepository carRepository, AvailabilityChecker availabilityChecker, IPriceGenerator priceGenerator)
        {
            _rentRepository = rentRepository;
            _carRepository = carRepository;
            _availabilityChecker = availabilityChecker;
            _priceGenerator = priceGenerator;
        }

        public async Task<List<OfferForCarSearchDto>>  GetNewOffers(string? brand, string? model, DateTime startDate, DateTime endDate, string? location, string email, string conditions, string companyName)
        {
            List<CarIdRentDatesDto> pairs = await _rentRepository.GetChosenCarActiveRentDatesAsync(brand, model, location);
            List<int> availableCarIds = _availabilityChecker.CheckForAvailableCars(pairs, startDate, endDate);
            List<Car> availableCars = await _carRepository.GetCarsByIdAsync(availableCarIds);

            List<OfferForCarSearchDto> newOffers = new List<OfferForCarSearchDto>();
            foreach (var car in availableCars)
            {
                newOffers.Add(new OfferForCarSearchDto
                {
                    CarId = car.CarId,
                    Brand = car.Model.Brand.Name,
                    Model = car.Model.Year == null ? car.Model.Name : car.Model.Name + " " + car.Model.Year,
                    Price = _priceGenerator.GeneratePrice(car.Model.BasePrice, startDate, endDate),
                    Conditions = conditions,
                    CompanyName = companyName,
                    Location = car.Location,
                    StartDate = startDate,
                    EndDate = endDate,
                    Email = email
                });
            }

            return newOffers;
        }
    }
}
