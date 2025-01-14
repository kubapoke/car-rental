using CarSearchAPI.Abstractions;
using CarSearchAPI.DTOs.CarRental;
using CarSearchAPI.DTOs.ForwardingParameters;

namespace CarSearchAPI.Services.ProviderServices.ProviderOfferServices
{
    public class CarRentalProviderOfferService : IProviderOfferService
    {
        public Task<int> GetOfferAmountAsync(GetOfferAmountParametersDto parameters)
        {
            throw new NotImplementedException();
        }

        public Task<List<OfferDto>> GetOfferListAsync(GetOfferListParametersDto parameters)
        {
            throw new NotImplementedException();
        }
    }
}
