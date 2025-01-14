using CarSearchAPI.DTOs.CarRental;
using CarSearchAPI.DTOs.ForwardingParameters;

namespace CarSearchAPI.Abstractions
{
    public interface IProviderOfferService
    {
        public Task<int> GetOfferAmountAsync(GetOfferAmountParametersDto parameters);
        public Task<List<OfferDto>> GetOfferListAsync(GetOfferListParametersDto parameters);
    }
}
