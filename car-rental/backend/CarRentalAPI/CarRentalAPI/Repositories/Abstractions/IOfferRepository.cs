using CarRentalAPI.DTOs.Redis;
using CarRentalAPI.Models;

namespace CarRentalAPI.Repositories.Abstractions
{
    public interface IOfferRepository
    {
        public Task<List<(string Guid, CachedOfferDto CachedOffer)>> CreateAndRetrieveCachedOffersAsync(List<Car> cars,
            DateTime startDate,
            DateTime endDate, string conditions, string companyName, int? page, int? pageSize);
        public Task<CachedOfferDto?> GetAndDeleteOfferAsync(string offerId);
    }
}