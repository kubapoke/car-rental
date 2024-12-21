using CarSearchAPI.DTOs.CarRental;
using CarSearchAPI.DTOs.ForwardingParameters;

namespace CarSearchAPI.Abstractions
{
    public interface IOfferService
    {
        public Task<(int totalOfferAmount, List<(IExternalDataProvider provider, int amount)> providerOfferAmount)> GetAmountOfOffersFromAllProvidersAsync
            (GetOfferAmountParametersDto offerAmountParametersDto);
        public Task<int> UpdatePageOffersAndOffersToBePagedAsync(
            IExternalDataProvider provider,
            GetOfferListParametersDto offerListParametersDto,
            List<OfferDto> pageOffers,
            int offersToBePaged,
            int startingOffer,
            int pageSize);
    }
}
