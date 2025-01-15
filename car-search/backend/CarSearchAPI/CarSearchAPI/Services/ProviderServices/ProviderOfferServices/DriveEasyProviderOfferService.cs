using CarSearchAPI.Abstractions;
using CarSearchAPI.DTOs.CarRental;

namespace CarSearchAPI.Services.ProviderServices.ProviderOfferServices
{
    public class DriveEasyProviderOfferService : IProviderOfferService
    {
        public string GetProviderName()
        {
            return "DriveEasy";
        }

        public Task<int> GetOfferAmountAsync(HttpClient client, string url)
        {
            throw new NotImplementedException();
        }

        public Task<List<OfferDto>> GetOfferListAsync(HttpClient client, string url)
        {
            throw new NotImplementedException();
        }
    }
}
