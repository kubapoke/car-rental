using CarSearchAPI.Abstractions;
using CarSearchAPI.DTOs.CarRental;
using CarSearchAPI.DTOs.ForwardingParameters;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace CarSearchAPI.Services.ProviderServices.ProviderOfferServices
{
    public class CarRentalProviderOfferService : IProviderOfferService
    {
        private const string OfferAmountEndpoint = "/api/Offers/offer-amount";
        private const string OfferListUrl = "/api/Offers/offer-list";
        
        public async Task<int> GetOfferAmountAsync(HttpClient client, string url,
            GetOfferAmountParametersDto parameters)
        {
            url += OfferAmountEndpoint;
            
            var queryParameters = new Dictionary<string, string?>()
            {
                { "model", parameters.Model },
                { "brand", parameters.Brand },
                { "startDate", parameters.StartDate.ToString("o") },
                { "endDate", parameters.EndDate.ToString("o") },
                { "location", parameters.Location },
            };
            
            url = QueryHelpers.AddQueryString(url, 
                queryParameters.Where(p => p.Value != null));
            
            var response = await client.GetAsync(url);
            
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var amount = int.Parse(responseContent);
                return amount;
            }
            else
            {
                throw new Exception($"Error fetching data from \"/api/Offers/offer-amount\" at Car Rental API");
            }
        }

        public async Task<List<OfferDto>> GetOfferListAsync(HttpClient client, string url,
            GetOfferListParametersDto parameters)
        {
            url += OfferListUrl;
            
            var queryParameters = new Dictionary<string, string?>()
            {
                { "model", parameters.Model },
                { "brand", parameters.Brand },
                { "startDate", parameters.StartDate.ToString("o") },
                { "endDate", parameters.EndDate.ToString("o") },
                { "location", parameters.Location },
                { "email", parameters.Email },
                { "page", parameters.Page.ToString() },
                { "pageSize", parameters.PageSize.ToString() },
            };
            
            url = QueryHelpers.AddQueryString(url, 
                queryParameters.Where(p => p.Value != null));
            
            var response = await client.GetAsync(url);
            
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var offers = JsonConvert.DeserializeObject<List<OfferDto>>(responseContent);
                return offers;
            }
            else
            {
                throw new Exception($"Error fetching data from \"/api/Offers/offer-list\" at Car Rental API");
            }
        }
        
        public string GetProviderName()
        {
            return "CarRental";
        }
    }
}
