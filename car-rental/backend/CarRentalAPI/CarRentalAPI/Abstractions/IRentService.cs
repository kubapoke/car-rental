using CarRentalAPI.DTOs.Redis;
using CarRentalAPI.DTOs.Rents;
using CarRentalAPI.Enums;
using CarRentalAPI.Models;

namespace CarRentalAPI.Abstractions
{
    public interface IRentService
    {
        public Task<Rent> CreateAndGetNewRentAsync(CachedOfferDto offer, string userEmail);
        public Task<List<RentInfoDto>> GetRentInformationByStatusAsync(RentStatuses? status);
        public Task<Rent> GetRentByIdAsync(int id);
        public Task CloseRentAsync(Rent rent, DateTime actualStartDate, DateTime actualEndDate, 
            string imageUri, string description);
        public Task MarkRentAsReadyToReturnAsync(int id);
    }   
}