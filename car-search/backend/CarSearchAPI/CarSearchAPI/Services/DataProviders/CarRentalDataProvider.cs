using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using CarSearchAPI.Abstractions;
using CarSearchAPI.DTOs.CarRental;
using CarSearchAPI.DTOs.CarSearch;
using CarSearchAPI.DTOs.ForwardingParameters;
using CarSearchAPI.DTOs.Users;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CarSearchAPI.Services.DataProviders
{
    public class CarRentalDataProvider : IExternalDataProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IProviderCarService _carService;
        private readonly IProviderOfferService _offerService;
        private readonly IProviderRentService _rentService;
        private readonly string _accessToken;

        public CarRentalDataProvider(IHttpClientFactory httpClientFactory, IProviderServiceFactory providerServiceFactory)
        {
            _httpClientFactory = httpClientFactory;
            _carService = providerServiceFactory.GetProviderCarService(GetProviderName());
            _offerService = providerServiceFactory.GetProviderOfferService(GetProviderName());
            _rentService = providerServiceFactory.GetProviderRentService(GetProviderName());
            _accessToken = GenerateAccessToken();
        }

        public string GetProviderName()
        {
            return "CarRental";
        }

        public async Task<List<CarDto>> GetCarListAsync()
        {
            var client = GetClientWithBearerToken();

            var url = GetUrlWithoutQuery("/api/Cars/car-list");          
            
            return await _carService.GetCarListAsync(client, url);
        }

        public async Task<int> GetOfferAmountAsync(GetOfferAmountParametersDto parameters)
        {
            var client = GetClientWithBearerToken();

            var urlWithoutQuery = GetUrlWithoutQuery("/api/Offers/offer-amount");

            var queryParameters = new Dictionary<string, string?>()
            {
                { "model", parameters.Model },
                { "brand", parameters.Brand },
                { "startDate", parameters.StartDate.ToString("o") },
                { "endDate", parameters.EndDate.ToString("o") },
                { "location", parameters.Location },
            };
            
            var url = QueryHelpers.AddQueryString(urlWithoutQuery, 
                queryParameters.Where(p => p.Value != null));
            
            return await _offerService.GetOfferAmountAsync(client, url);
        }

        public async Task<List<OfferDto>> GetOfferListAsync(GetOfferListParametersDto parameters)
        {
            var client = GetClientWithBearerToken();

            var urlWithoutQuery = GetUrlWithoutQuery("/api/Offers/offer-list");
            
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
            
            var url = QueryHelpers.AddQueryString(urlWithoutQuery, 
                queryParameters.Where(p => p.Value != null));
            
            return await _offerService.GetOfferListAsync(client, url);
        }

        public async Task<NewSearchRentDto> CreateNewRentAsync(ClaimsPrincipal claimsPrincipal)
        {
            var client = GetClientWithBearerToken();

            string offerId = claimsPrincipal.FindFirst("OfferId")?.Value;
            string email = claimsPrincipal.FindFirst("Email")?.Value;

            NewRentalRentDto newRentDto = new NewRentalRentDto
            {
                OfferId = offerId,
                Email = email,
            };

            var url = GetUrlWithoutQuery("/api/Rents/create-new-rent");

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(newRentDto),
                Encoding.UTF8,
                "application/json"
            );

            return await _rentService.CreateNewRentAsync(client, url, jsonContent);
        }        
     
        public async Task<bool> SetRentStatusReadyToReturnAsync(int rentId)
        {
            var client = GetClientWithBearerToken();

            var url = GetUrlWithoutQuery("/api/Rents/set-rent-status-ready-to-return");

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(rentId),
                Encoding.UTF8,
                "application/json"
            );

            return await _rentService.SetRentStatusReadyToReturnAsync(client, url, jsonContent);
        }

        private HttpClient GetClientWithBearerToken()
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            return client;
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

        private string GetUrlWithoutQuery(string endpoint)
        {
            var carRentalApiUrl = Environment.GetEnvironmentVariable("CAR_RENTAL_API_URL");
            var url = $"{carRentalApiUrl}{endpoint}";
            return url;
        }
    }
}