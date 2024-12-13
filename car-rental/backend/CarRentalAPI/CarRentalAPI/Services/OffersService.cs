﻿using CarRentalAPI.Abstractions;
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

        public async Task<List<OfferForCarSearchDto>> GetNewOffers(string? brand, string? model, DateTime startDate, DateTime endDate, string? location, string email, string conditions, string companyName)
        {
            List<CarIdRentDatesDto> pairs = await _rentRepository.GetChosenCarActiveRentDatesAsync(brand, model, location);
            List<int> notAvailableCarIds = _availabilityChecker.CheckForNotAvailableCars(pairs, startDate, endDate);
            List<Car> availableCars = await _carRepository.GetCarsByIdAsync(notAvailableCarIds, brand, model, location);

            List<OfferForCarSearchDto> newOffers = new List<OfferForCarSearchDto>();
            foreach (var car in availableCars)
            {
                var guid = await _offerRepository.CreateOfferForRedisAsync(car, startDate, endDate, conditions, companyName);
                var newOffer = await _offerRepository.CreateCarSearchOfferFromRedisOfferAsync(guid, email);
                newOffers.Add(newOffer);
            }

            return newOffers;
        }
    }
}
