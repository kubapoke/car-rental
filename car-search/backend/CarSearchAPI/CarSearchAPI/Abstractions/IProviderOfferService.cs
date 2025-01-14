using CarSearchAPI.DTOs.CarRental;
using CarSearchAPI.DTOs.ForwardingParameters;

namespace CarSearchAPI.Abstractions
{
    public interface IProviderOfferService
    {
        public Task<int> GetOfferAmountAsync(HttpClient client, string url);
        public Task<List<OfferDto>> GetOfferListAsync(HttpClient client, string url);
    }
}
