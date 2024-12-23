using CarRentalAPI.DTOs.CarSearch;

namespace CarRentalAPI.Abstractions
{   
    public interface IOfferService
    {
        public Task<int> GetOffersCountAsync(string? brand, string? model, DateTime startDate,
            DateTime endDate, string? location);
        public Task<List<OfferForCarSearchDto>> GetNewOffersAsync(string? brand, string? model, DateTime startDate,
            DateTime endDate, string? location, string email, string conditions, string companyName, int? page,
            int? pageSize);
    }
}