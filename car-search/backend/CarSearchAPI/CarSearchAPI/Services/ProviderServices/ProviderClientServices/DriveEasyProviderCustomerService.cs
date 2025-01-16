using CarSearchAPI.Abstractions;

namespace CarSearchAPI.Services.ProviderServices.ProviderClientServices
{
    public class DriveEasyProviderCustomerService : IProviderCustomerService
    {
        private const string ClientExistsEndpoint = "/api/Client/checkClient/";
        public async Task<bool> CheckIfClientExists(HttpClient client, string url, string customerEmail)
        {
            url = $"{url}{ClientExistsEndpoint}{CreateIdFromEmail(customerEmail)}";
            
            var response = await client.GetAsync(url);
            
            var responseString = await response.Content.ReadAsStringAsync();
            
            return responseString == "Client found";
        }

        public async Task CreateCustomerAsync(HttpClient client, string url)
        {
            await client.PostAsync(url, null);
        }
        
        public string GetProviderName()
        {
            return "DriveEasy";
        }

        private string CreateIdFromEmail(string email)
        {
            return $"{email}-CAR-RENTAL";
        }
    } 
}

