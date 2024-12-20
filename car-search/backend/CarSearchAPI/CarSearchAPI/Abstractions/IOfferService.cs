using CarSearchAPI.DTOs.CarRental;
using CarSearchAPI.DTOs.ForwardingParameters;

namespace CarSearchAPI.Abstractions
{
    public interface IOfferService
    {
        public Task<(int, List<(IExternalDataProvider provider, int amount)>)> GetAmountOfOffersFromAllProvidersAsync
            (GetOfferAmountParametersDto offerAmountParametersDto);
        public Task<int> UpdatePageOffersAndOffersToByPagedAsync(
            IExternalDataProvider provider,
            GetOfferListParametersDto offerListParametersDto,
            List<OfferDto> pageOffers,
            int offersToBePaged,
            int startingOffer,
            int pageSize);
    }
}
