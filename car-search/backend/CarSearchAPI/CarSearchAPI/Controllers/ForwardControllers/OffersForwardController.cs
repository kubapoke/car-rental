using System.Security.Claims;
using CarSearchAPI.Abstractions;
using CarSearchAPI.DTOs.CarRental;
using CarSearchAPI.DTOs.CarSearch;
using CarSearchAPI.DTOs.ForwardingParameters;
using CarSearchAPI.DTOs.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Exception = System.Exception;
using IExternalDataProvider = CarSearchAPI.Abstractions.IExternalDataProvider;

namespace CarSearchAPI.Controllers.ForwardControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffersForwardController : ControllerBase
    {
        // this contains all of the external data providers
        private readonly IEnumerable<IExternalDataProvider> _dataProviders;
        private const int DefaultPageSize = 6;

        public OffersForwardController(IEnumerable<IExternalDataProvider> dataProviders)
        {
            _dataProviders = dataProviders;
        }
        
        [Authorize(Policy = "LegitUser")]
        [HttpGet("offer-list")]
        public async Task<IActionResult> OfferList(
        [FromQuery] string? brand,
        [FromQuery] string? model,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] string? location,
        [FromQuery] int? page,
        [FromQuery] int? pageSize)
        {
            int _page = page ?? 0;
            int _pageSize = pageSize ?? DefaultPageSize;
            
            var offerAmountParametersDto = new GetOfferAmountParametersDto()
            {
                Brand = brand,
                Model = model,
                StartDate = startDate,
                EndDate = endDate,
                Location = location,
            };

            (int totalOfferAmount, List<(IExternalDataProvider provider, int amount)> providerOfferAmount) = 
                await GetAmountOfOffersFromAllProvidersAsync(offerAmountParametersDto);
            

            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            var pageOffers = new List<OfferDto>();
            
            var cumulativeAmount = 0;
            var offersToBePaged = _pageSize;

            foreach (var providerAmount in providerOfferAmount)
            {
                var provider = providerAmount.provider;
                var amount = providerAmount.amount;
                cumulativeAmount += amount;

                if (cumulativeAmount > _page * _pageSize)
                {
                    // initialization
                    var offerListParametersDto = new GetOfferListParametersDto()
                    {
                        Brand = brand,
                        Model = model,
                        StartDate = startDate,
                        EndDate = endDate,
                        Location = location,
                        Email = email,
                        PageSize = _pageSize,
                    };

                    if (offersToBePaged == _pageSize)
                    {
                        // we need to start from the offer at the appropriate page, ignoring the ones
                        // we've already taken from other providers
                        var startingOffer = _page * _pageSize - (cumulativeAmount - amount);
                        var startingPage = startingOffer / _pageSize; 
                        
                        offerListParametersDto.Page = startingPage;
                       
                        try
                        {
                            var startingOfferPage = await provider.GetOfferListAsync(offerListParametersDto);
                            var takenOffers = startingOfferPage.Skip(startingOffer % _pageSize).Take(offersToBePaged).ToList();
                            pageOffers.AddRange(takenOffers);
                            offersToBePaged -= takenOffers.Count;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error fetching data from provider {provider.GetProviderName()}: {ex.Message}");        
                        }
                    }
                    if (offersToBePaged > 0)
                    {
                        // we need to start from the offer at the appropriate page, ignoring the ones
                        // we've already taken from other providers, and the aleady taken offers
                        var startingOffer = _page * _pageSize - (cumulativeAmount - amount) + (_pageSize - offersToBePaged);
                        var startingPage = startingOffer / _pageSize;

                        offerListParametersDto.Page = startingPage;

                        try
                        {
                            var startingOfferPage = await provider.GetOfferListAsync(offerListParametersDto);
                            var takenOffers = startingOfferPage.Skip(startingOffer % _pageSize).Take(offersToBePaged).ToList();
                            pageOffers.AddRange(takenOffers);
                            offersToBePaged -= takenOffers.Count;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error fetching data from provider {provider.GetProviderName()}: {ex.Message}");        
                        }
                    }

                    if (offersToBePaged <= 0)
                    {
                        break;
                    }
                }
            }

            var offerPage = new OfferPageDto();

            offerPage.PageCount = totalOfferAmount / _pageSize;
            if(totalOfferAmount % _pageSize != 0)
                offerPage.PageCount++;
            offerPage.Offers = pageOffers;
            
            return Ok(offerPage);
        }

        private async Task<(int, List<(IExternalDataProvider provider, int amount)>)> GetAmountOfOffersFromAllProvidersAsync(GetOfferAmountParametersDto offerAmountParametersDto)
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
    }
}

