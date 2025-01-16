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

            var url = GetUrlWithoutQuery();          
            
            return await _carService.GetCarListAsync(client, url);
        }

        public async Task<int> GetOfferAmountAsync(GetOfferAmountParametersDto parameters)
        {
            var client = GetClientWithBearerToken();

            var url = GetUrlWithoutQuery();
            
            return await _offerService.GetOfferAmountAsync(client, url, parameters, null);
        }

        public async Task<List<OfferDto>> GetOfferListAsync(GetOfferListParametersDto parameters)
        {
            var client = GetClientWithBearerToken();

            var url = GetUrlWithoutQuery();
            
            return await _offerService.GetOfferListAsync(client, url, parameters, null);
        }

        public async Task<NewSearchRentDto> CreateNewRentAsync(ClaimsPrincipal claimsPrincipal)
        {
            var client = GetClientWithBearerToken();

            var url = GetUrlWithoutQuery();
            
            return await _rentService.CreateNewRentAsync(client, url, claimsPrincipal);
        }        
     
        public async Task<bool> SetRentStatusReadyToReturnAsync(string rentId)
        {
            var client = GetClientWithBearerToken();

            var url = GetUrlWithoutQuery();

            return await _rentService.SetRentStatusReadyToReturnAsync(client, url, rentId);
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

        private string GetUrlWithoutQuery(string endpoint = "")
        {
            var carRentalApiUrl = Environment.GetEnvironmentVariable("CAR_RENTAL_API_URL");
            var url = $"{carRentalApiUrl}{endpoint}";
            return url;
        }
    }
}