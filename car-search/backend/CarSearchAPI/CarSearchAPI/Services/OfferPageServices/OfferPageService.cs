using CarSearchAPI.Abstractions;
using CarSearchAPI.DTOs.CarRental;
using CarSearchAPI.DTOs.CarSearch;
using CarSearchAPI.DTOs.ForwardingParameters;
using System.Security.Claims;

namespace CarSearchAPI.Services.OfferServices
{
    public class OfferPageService : IOfferPageService
    {
        private const int DefaultPageSize = 6;

        private readonly IOfferService _offerService;

        public OfferPageService(IOfferService offerService)
        {
            _offerService = offerService;
        }

        public async Task<OfferPageDto> GetOfferPageAsync
            (GetOfferAmountParametersDto offerAmountParametersDto, GetOfferListParametersDto offerListParametersDto, 
            int page, int pageSize)
        {
            (int totalOfferAmount, List<(IExternalDataProvider provider, int amount)> providerOfferAmount) =
                await _offerService.GetAmountOfOffersFromAllProvidersAsync(offerAmountParametersDto);

            var pageOffers = await IterateThroughAllProvidersAsync(providerOfferAmount, offerListParametersDto, page, pageSize);

            var offerPage = new OfferPageDto();

            offerPage.PageCount = totalOfferAmount / pageSize;
            if (totalOfferAmount % pageSize != 0)
            {
                offerPage.PageCount++;
            }
            offerPage.Offers = pageOffers;

            return offerPage;
        }
        

        public GetOfferAmountParametersDto GetGetOfferAmountParameters
            (string? brand, string? model, DateTime startDate, DateTime endDate, string? location, string? email) 
        {
            var offerAmountParametersDto = new GetOfferAmountParametersDto()
            {
                Brand = brand,
                Model = model,
                StartDate = startDate,
                EndDate = endDate,
                Location = location,
                Email = email,
            };

            return offerAmountParametersDto;
        }

        public GetOfferListParametersDto GetGetOfferListParameters
            (string? brand, string? model, DateTime startDate, DateTime endDate, string? location, string email, int pageSize)
        {
            var offerListParametersDto = new GetOfferListParametersDto()
            {
                Brand = brand,
                Model = model,
                StartDate = startDate,
                EndDate = endDate,
                Location = location,
                Email = email,
                PageSize = pageSize,
            };

            return offerListParametersDto;
        }

        public (int page, int pageSize) GetPageAndPageSize(int? providedPage, int? providedPageSize)
        {
            int page = providedPage ?? 0;
            int pageSize = providedPageSize ?? DefaultPageSize;
            return (page, pageSize);
        }

        private async Task<List<OfferDto>> IterateThroughAllProvidersAsync(
            List<(IExternalDataProvider provider, int amount)> providerOfferAmount, 
            GetOfferListParametersDto offerListParametersDto,
            int page, int pageSize)
        {
            var pageOffers = new List<OfferDto>();

            var cumulativeAmount = 0;
            var offersToBePaged = pageSize;

            foreach (var providerAmount in providerOfferAmount)
            {
                var provider = providerAmount.provider;
                var amount = providerAmount.amount;
                cumulativeAmount += amount;

                if (cumulativeAmount > page * pageSize)
                {

                    if (offersToBePaged == pageSize)
                    {
                        var startingOffer = page * pageSize - (cumulativeAmount - amount);
                        var startingPage = startingOffer / pageSize;

                        offerListParametersDto.Page = startingPage;

                        offersToBePaged -= await _offerService.UpdatePageOffersAndOffersToBePagedAsync(provider, offerListParametersDto,
                            pageOffers, offersToBePaged, startingOffer, pageSize);
                    }
                    if (offersToBePaged > 0)
                    {
                        var startingOffer = page * pageSize - (cumulativeAmount - amount) + (pageSize - offersToBePaged);
                        var startingPage = startingOffer / pageSize;

                        offerListParametersDto.Page = startingPage;

                        offersToBePaged -= await _offerService.UpdatePageOffersAndOffersToBePagedAsync(provider, offerListParametersDto,
                            pageOffers, offersToBePaged, startingOffer, pageSize);
                    }

                    if (offersToBePaged <= 0)
                    {
                        break;
                    }
                }
            }

            return pageOffers;
        }
    }
}
