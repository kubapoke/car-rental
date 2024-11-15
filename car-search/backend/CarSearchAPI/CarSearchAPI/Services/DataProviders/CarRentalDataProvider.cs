using CarSearchAPI.Abstractions;

namespace CarSearchAPI.Services.DataProviders
{
    public class CarRentalDataProvider : IExternalDataProvider
    {
        // This will get data from an appropriate endpoint (using FromQuery parameters!) from CarRentalAPI
        private readonly IHttpClientFactory _httpClientFactory;

        public CarRentalDataProvider(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        
        public async Task<string> GetDataAsync(string endpoint, Dictionary<string, string> parameters)
        {
            var client = _httpClientFactory.CreateClient();
            
            var queryString = string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"));
            
            // Gets the url of the desired endpoint
            var url = $"http://{Environment.GetEnvironmentVariable("CAR_RENTAL_API_URL")}/api/{endpoint}?{queryString}";
            
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception($"Error fetching data from {endpoint} at Car Rental API");
            }
        }
    }
}
