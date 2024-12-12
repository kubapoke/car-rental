using CarSearchAPI.Abstractions;
using CarSearchAPI.DTOs.CarRental;
using CarSearchAPI.DTOs.CarSearch;
using CarSearchAPI.DTOs.ForwardingParameters;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using CarSearchAPI.DTOs.Users;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CarSearchAPI.Services.DataProviders
{
    public class CarRentalDataProvider : IExternalDataProvider
    {
        // This will get data from an appropriate endpoint (using FromQuery parameters!) from CarRentalAPI
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly string _accessToken;

        public CarRentalDataProvider(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _accessToken = GenerateAccessToken();
        }

        public string GetProviderName()
        {
            return "CarRental";
        }

        public async Task<List<CarDto>> GetCarListAsync()
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            
            var carRentalApiUrl = Environment.GetEnvironmentVariable("CAR_RENTAL_API_URL");
            var endpoint = "/api/Cars/car-list";

            var url = $"{carRentalApiUrl}{endpoint}";
            
            var response = await client.GetAsync(url);
            
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var cars = JsonConvert.DeserializeObject<List<CarDto>>(responseContent);
                return cars;
            }
            else
            {
                throw new Exception($"Error fetching data from {endpoint} at Car Rental API");
            }
        }

        public async Task<List<OfferDto>> GetOfferListAsync(GetOfferListParametersDto parameters)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

            var carRentalApiUrl = Environment.GetEnvironmentVariable("CAR_RENTAL_API_URL");
            var endpoint = "/api/Offers/offer-list";
            
            var queryParameters = new Dictionary<string, string?>()
            {
                { "model", parameters.Model },
                { "brand", parameters.Brand },
                { "startDate", parameters.StartDate.ToString("o") },
                { "endDate", parameters.EndDate.ToString("o") },
                { "location", parameters.Location },
                { "email", parameters.Email },
            };
            
            // this creates an appropriate url
            var url = QueryHelpers.AddQueryString($"{carRentalApiUrl}{endpoint}", 
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
                throw new Exception($"Error fetching data from {endpoint} at Car Rental API");
            }
        }

        public async Task<NewSearchRentDto> CreateNewRentAsync(ClaimsPrincipal claimsPrincipal)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

            string offerId = claimsPrincipal.FindFirst("OfferId")?.Value;
            string email = claimsPrincipal.FindFirst("Email")?.Value;

            NewRentalRentDto newRentDto = new NewRentalRentDto
            {
                OfferId = offerId,
                Email = email,
            };

            var carRentalApiUrl = Environment.GetEnvironmentVariable("CAR_RENTAL_API_URL")
                          ?? throw new InvalidOperationException("CAR_RENTAL_API_URL is not set.");
            const string endpoint = "/api/Rents/create-new-rent";

            Console.WriteLine($"Serialized Payload: {JsonSerializer.Serialize(newRentDto)}");

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(newRentDto),
                Encoding.UTF8,
                "application/json"
            );

            var url = $"{carRentalApiUrl}{endpoint}";

            var response = await client.PostAsync(url, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                NewSearchRentDto newSearchRentDto = await response.Content.ReadFromJsonAsync<NewSearchRentDto>();
                return newSearchRentDto;
            }

            var errorMessage = $"Error fetching data from {endpoint} at Car Rental API. StatusCode: {response.StatusCode}, ReasonPhrase: {response.ReasonPhrase}";
            throw new HttpRequestException(errorMessage);
        }
        
        private string GenerateAccessToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("CAR_RENTAL_SECRET_KEY"));
            List<Claim> claims = new List<Claim>();
            Claim backendClaim = new Claim("Backend", "1");
            claims.Add(backendClaim);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Expires = DateTime.UtcNow.AddMinutes(60)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
