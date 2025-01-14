using CarSearchAPI.Abstractions;
using CarSearchAPI.DTOs.CarRental;
using CarSearchAPI.DTOs.ForwardingParameters;
using Newtonsoft.Json;

namespace CarSearchAPI.Services.ProviderServices.ProviderOfferServices
{
    public class CarRentalProviderOfferService : IProviderOfferService
    {
        public async Task<int> GetOfferAmountAsync(HttpClient client, string url)
        {
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

        public async Task<List<OfferDto>> GetOfferListAsync(HttpClient client, string url)
        {
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
    }
}
