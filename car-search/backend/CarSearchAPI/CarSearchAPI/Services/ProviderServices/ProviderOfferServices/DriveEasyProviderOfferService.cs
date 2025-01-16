using CarSearchAPI.Abstractions;
using CarSearchAPI.DTOs.CarRental;
using CarSearchAPI.DTOs.ForwardingParameters;

namespace CarSearchAPI.Services.ProviderServices.ProviderOfferServices
{
    public class DriveEasyProviderOfferService : IProviderOfferService
    {
        public string GetProviderName()
        {
            return "DriveEasy";
        }

        public Task<int> GetOfferAmountAsync(HttpClient client, string url, GetOfferAmountParametersDto parameters)
        {
            throw new NotImplementedException();
        }

        public Task<List<OfferDto>> GetOfferListAsync(HttpClient client, string url,
            GetOfferListParametersDto parameters)
        {
            throw new NotImplementedException();
        }
    }
}
