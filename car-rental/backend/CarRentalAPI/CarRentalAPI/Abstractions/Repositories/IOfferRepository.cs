using CarRentalAPI.DTOs.CarSearch;
using CarRentalAPI.DTOs.Redis;
using CarRentalAPI.Models;

namespace CarRentalAPI.Abstractions.Repositories
{
    public interface IOfferRepository
    {
        public Task<List<OfferForCarSearchDto>> CreateAndRetrieveOffersAsync(List<Car> cars, DateTime startDate, DateTime endDate,
            string conditions, string companyName, string email, int? page, int? pageSize);
        public Task<OfferForRedisDto?> GetAndDeleteOfferAsync(string offerId);
    }
}