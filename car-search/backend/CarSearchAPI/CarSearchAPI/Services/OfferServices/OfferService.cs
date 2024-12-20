using CarSearchAPI.Abstractions;
using CarSearchAPI.DTOs.CarRental;
using CarSearchAPI.DTOs.ForwardingParameters;
using System.Collections.Generic;

namespace CarSearchAPI.Services.OfferServices
{
    public class OfferService : IOfferService
    {
        private readonly IEnumerable<IExternalDataProvider> _dataProviders;

        public OfferService(IEnumerable<IExternalDataProvider> dataProviders)
        {
            _dataProviders = dataProviders;
        }

        public async Task<(int, List<(IExternalDataProvider provider, int amount)>)> GetAmountOfOffersFromAllProvidersAsync
            (GetOfferAmountParametersDto offerAmountParametersDto)
        {
            var totalOfferAmount = 0;
            var providerOfferAmount = new List<(IExternalDataProvider provider, int amount)>();


            // get the amount of offers matching the search data from each provider
            foreach (var provider in _dataProviders)
            {
                try
                {
                    var offerAmount = await provider.GetOfferAmountAsync(offerAmountParametersDto);
                    totalOfferAmount += offerAmount;
                    providerOfferAmount.Add((provider, offerAmount));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting offer amount from provider: {ex.Message}");
                }
            }

            return (totalOfferAmount, providerOfferAmount);
        }

        public async Task<int> UpdatePageOffersAndOffersToByPagedAsync(
            IExternalDataProvider provider,
            GetOfferListParametersDto offerListParametersDto,
            List<OfferDto> pageOffers,
            int offersToBePaged,
            int startingOffer,
            int pageSize)
        {
            try
            {
                var startingOfferPage = await provider.GetOfferListAsync(offerListParametersDto);
                var takenOffers = startingOfferPage.Skip(startingOffer % pageSize).Take(offersToBePaged).ToList();
                pageOffers.AddRange(takenOffers);
                return takenOffers.Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data from provider {provider.GetProviderName()}: {ex.Message}");
                return 0;
            }
        }
    }
}
