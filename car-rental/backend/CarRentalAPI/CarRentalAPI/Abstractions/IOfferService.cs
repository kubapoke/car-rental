using CarRentalAPI.DTOs.CarSearch;
using CarRentalAPI.DTOs.Redis;

namespace CarRentalAPI.Abstractions
{   
    public interface IOfferService
    {
        public Task<int> GetOffersCountAsync(string? brand, string? model, DateTime startDate,
            DateTime endDate, string? location);
        public Task<List<OfferForCarSearchDto>> GetNewOffersAsync(string? brand, string? model, DateTime startDate,
            DateTime endDate, string? location, string email, string conditions, string companyName, int? page,
            int? pageSize);
        public Task<CachedOfferDto?> GetAndDeleteOfferByIdAsync(string offerId);
        public Task<CachedOfferDto?> GetOfferByIdAsync(string offerId);
    }
}