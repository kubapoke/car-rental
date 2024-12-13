using CarRentalAPI.DTOs.CarSearch;
using CarRentalAPI.DTOs.Redis;
using CarRentalAPI.Models;

namespace CarRentalAPI.Abstractions.Repositories
{
    public interface IOfferRepository
    {
        public Task<string> CreateOfferForRedisAsync(Car car, DateTime startDate, DateTime endDate,
            string conditions, string companyName);

        public Task<OfferForCarSearchDto?> CreateCarSearchOfferFromRedisOfferAsync(string offerGuid, string email);
        public Task<OfferForRedisDto?> GetAndDeleteOfferAsync(string offerId);
    }
}