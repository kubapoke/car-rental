using CarRentalAPI.Abstractions;
using CarRentalAPI.DTOs.CarSearch;
using CarRentalAPI.DTOs.Redis;
using CarRentalAPI.Enums;
using CarRentalAPI.Models;
using CarRentalAPI.Repositories.Abstractions;

namespace CarRentalAPI.Services
{   
    public class RentService : IRentService
    {
        private readonly ICarService _carService;
        private readonly IRentRepository _rentRepository;
        private const RentStatuses DefaultStatus = RentStatuses.Active;

        public RentService(ICarService carService, IRentRepository rentRepository)
        {
            _carService = carService;
            _rentRepository = rentRepository;
        }
        
        public async Task<NewSearchRentDto> CreateAndGetNewRentAsync(CachedOfferDto offer, string userEmail)
        {
            var car = await _carService.GetCarOrNullFromOfferAsync(offer);

            if (car is null)
                throw new KeyNotFoundException("Error fetching car from database");

            var newRent = GetRent(car.CarId, userEmail, offer.StartDate, offer.EndDate);

            await _rentRepository.CreateNewRentAsync(newRent);

            var newSearchRent = GetNewSearchRentDto(car.Model.Brand.Name, car.Model.Name, userEmail,
                offer.StartDate, offer.EndDate, newRent.RentId);

            return newSearchRent;
        }

        private Rent GetRent(int carId, string userEmail, DateTime rentStart, DateTime rentEnd)
        {
            return new Rent()
            {
                CarId = carId,
                UserEmail = userEmail,
                RentStart = rentStart,
                RentEnd = rentEnd,
                Status = DefaultStatus,
            };
        }

        private NewSearchRentDto GetNewSearchRentDto(string brand, string model, string email,
            DateTime startDate, DateTime endDate, int rentalCompanyRentId)
        {
            return new NewSearchRentDto()
            {
                Brand = brand,
                Model = model,
                Email = email,
                StartDate = startDate,
                EndDate = endDate,
                RentalCompanyRentId = rentalCompanyRentId,
            };
        }
    }
}