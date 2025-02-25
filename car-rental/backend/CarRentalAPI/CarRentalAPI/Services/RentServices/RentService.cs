﻿using CarRentalAPI.Abstractions;
using CarRentalAPI.DTOs.Redis;
using CarRentalAPI.DTOs.Rents;
using CarRentalAPI.Enums;
using CarRentalAPI.Models;
using CarRentalAPI.Repositories.Abstractions;

namespace CarRentalAPI.Services.RentServices
{   
    public class RentService : IRentService
    {
        private readonly ICarLookupService _carLookupService;
        private readonly IRentRepository _rentRepository;
        private const RentStatuses DefaultStatus = RentStatuses.Active;

        public RentService(ICarLookupService carLookupService, IRentRepository rentRepository)
        {
            _carLookupService = carLookupService;
            _rentRepository = rentRepository;
        }
        
        public async Task<Rent> CreateAndGetNewRentAsync(CachedOfferDto offer, string userEmail)
        {
            var car = await _carLookupService.GetCarOrNullFromOfferAsync(offer);

            if (car is null)
                throw new KeyNotFoundException("Error fetching car from database");

            var newRent = GetRent(car.CarId, userEmail, offer.StartDate, offer.EndDate);

            await _rentRepository.CreateNewRentAsync(newRent);

            return newRent;
        }

        public async Task<List<RentInfoDto>> GetRentInformationByStatusAsync(RentStatuses? status)
        {
            return await _rentRepository.GetRentInformationByStatusAsync(status);
        }

        public async Task<Rent> GetRentByIdAsync(int id)
        {
            var rent = await _rentRepository.GetRentOrNullByIdAsync(id);

            if (rent is null)
                throw new KeyNotFoundException("Error fetching rent from database");
            if (rent.Status == RentStatuses.Active || rent.Status == RentStatuses.Returned)
                throw new InvalidOperationException("Rent is not ready to be returned");

            return rent;
        }

        public async Task CloseRentAsync(Rent rent, DateTime actualStartDate, DateTime actualEndDate, string imageUri, string description)
        {
            rent.RentStart = actualStartDate;
            rent.RentEnd = actualEndDate;
            rent.ImageUri = imageUri;
            rent.Description = description;
            rent.Status = RentStatuses.Returned;

            await _rentRepository.SaveRepositoryChangesAsync();
        }

        public async Task MarkRentAsReadyToReturnAsync(int id)
        {
            var rent = await _rentRepository.GetRentOrNullByIdAsync(id);

            if (rent is null)
                throw new KeyNotFoundException("Error fetching rent from database");
            if (rent.Status == RentStatuses.ReadyToReturn || rent.Status == RentStatuses.Returned)
                throw new InvalidOperationException("Rent can't be marked as ready to return");
            
            rent.Status = RentStatuses.ReadyToReturn;

            await _rentRepository.SaveRepositoryChangesAsync();
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
    }
}