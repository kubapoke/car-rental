using System.Diagnostics;
using CarRentalAPI.Abstractions;
using CarRentalAPI.Abstractions.Repositories;
using CarRentalAPI.DTOs.CarSearch;
using CarRentalAPI.DTOs.Combinations;
using CarRentalAPI.DTOs.Redis;
using CarRentalAPI.Models;
using Newtonsoft.Json;

namespace CarRentalAPI.Services
{
    public class OffersService
    {
        private readonly IRentRepository _rentRepository;
        private readonly ICarRepository _carRepository;
        private readonly AvailabilityChecker _availabilityChecker;
        private readonly IOfferRepository _offerRepository;

        public OffersService(IRentRepository rentRepository, ICarRepository carRepository, AvailabilityChecker availabilityChecker, 
            IOfferRepository offerRepository)
        {
            _rentRepository = rentRepository;
            _carRepository = carRepository;
            _availabilityChecker = availabilityChecker;
            _offerRepository = offerRepository;
        }

        public async Task<int> GetOffersCountAsync(string? brand, string? model, DateTime startDate,
            DateTime endDate, string? location)
        {
            var pairs = await _rentRepository.GetChosenCarActiveRentDatesAsync(brand, model, location);
            var notAvailableCarIds = _availabilityChecker.CheckForNotAvailableCars(pairs, startDate, endDate);
            var availableCars = await _carRepository.GetCarsByIdAsync(notAvailableCarIds, brand, model, location);
            
            return availableCars.Count;
        }
        
        public async Task<List<OfferForCarSearchDto>> GetNewOffersAsync(string? brand, string? model, DateTime startDate,
            DateTime endDate, string? location, string email, string conditions, string companyName, int? page, int? pageSize)
        {
            var pairs = await _rentRepository.GetChosenCarActiveRentDatesAsync(brand, model, location);
            var notAvailableCarIds = _availabilityChecker.CheckForNotAvailableCars(pairs, startDate, endDate);
            var availableCars = await _carRepository.GetCarsByIdAsync(notAvailableCarIds, brand, model, location);
            
            var newOffers = await _offerRepository.CreateAndRetrieveOffersAsync(availableCars, 
                startDate, endDate, conditions, companyName, email, page, pageSize);
            
            return newOffers;
        }
    }
}
