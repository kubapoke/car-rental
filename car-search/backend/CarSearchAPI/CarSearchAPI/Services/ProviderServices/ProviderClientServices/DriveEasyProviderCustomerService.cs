using System.Text;
using System.Text.Json;
using CarSearchAPI.Abstractions;
using CarSearchAPI.DTOs.DriveEasy;

namespace CarSearchAPI.Services.ProviderServices.ProviderClientServices
{
    public class DriveEasyProviderCustomerService : IProviderCustomerService
    {
        private const string ClientExistsEndpoint = "/api/Client/checkClient/";
        private const string CreateClientEndpoint = "/api/Client/createClient";
        
        public async Task<bool> CheckIfCustomerExistsAsync(HttpClient client, string url, string? customerId)
        {
            url = $"{url}{ClientExistsEndpoint}{customerId}";
            
            var response = await client.GetAsync(url);
            
            var responseString = await response.Content.ReadAsStringAsync();
            
            return responseString == "Client found";
        }

        public async Task CreateCustomerAsync(HttpClient client, string url, string? customerId, string? email)
        {
            url += CreateClientEndpoint;

            var customerCreationParameters = new DriveEasyCustomerCreationParametersDto()
            {
                Id = customerId,
                Name = email,
                Surname = "Car Rental Customer",
                Email = email,
            };
            
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(customerCreationParameters),
                Encoding.UTF8,
                "application/json"
            );
            
            await client.PostAsync(url, jsonContent);
        }

        public async Task<string> GetCustomerIdAsync(HttpClient client, string url, string? email)
        {
            var id = CreateIdFromEmail(email);

            if (!(await CheckIfCustomerExistsAsync(client, url, id)))
            {
                await CreateCustomerAsync(client, url, id, email);
            }

            return id;
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

