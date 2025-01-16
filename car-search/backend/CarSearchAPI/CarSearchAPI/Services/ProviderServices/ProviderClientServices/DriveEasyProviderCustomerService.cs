using CarSearchAPI.Abstractions;

namespace CarSearchAPI.Services.ProviderServices.ProviderClientServices
{
    public class DriveEasyProviderCustomerService : IProviderCustomerService
    {
        public string GetProviderName()
        {
            return "DriveEasy";
        }

        public async Task<bool> CheckIfClientExists(HttpClient client, string url)
        {
            var response = await client.GetAsync(url);
            
            var responseString = await response.Content.ReadAsStringAsync();
            
            return responseString == "Client found";
        }

        public async Task CreateCustomerAsync(HttpClient client, string url, StringContent? jsonContent)
        {
            await client.PostAsync(url, jsonContent);
        }
    } 
}

