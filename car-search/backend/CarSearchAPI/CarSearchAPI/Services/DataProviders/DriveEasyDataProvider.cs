using System.Security.Claims;
using CarSearchAPI.Abstractions;
using CarSearchAPI.DTOs.CarRental;
using CarSearchAPI.DTOs.CarSearch;
using CarSearchAPI.DTOs.ForwardingParameters;
using CarSearchAPI.DTOs.Users;

namespace CarSearchAPI.Services.DataProviders;

public class DriveEasyDataProvider : IExternalDataProvider
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IProviderCarService _carService;
    private readonly IProviderOfferService _offerService;
    private readonly IProviderRentService _rentService;

    public DriveEasyDataProvider(IHttpClientFactory httpClientFactory, IProviderServiceFactory providerServiceFactory)
    {
        _httpClientFactory = httpClientFactory;
        _carService = providerServiceFactory.GetProviderCarService(GetProviderName());
        _offerService = providerServiceFactory.GetProviderOfferService(GetProviderName());
        _rentService = providerServiceFactory.GetProviderRentService(GetProviderName());
    }
    public string GetProviderName()
    {
        return "DriveEasy";
    }

    public async Task<List<CarDto>> GetCarListAsync()
    {
        var client = _httpClientFactory.CreateClient();
        
        var url = GetUrlWithoutQuery(); 
        
        return await _carService.GetCarListAsync(client, url);
    }

    public Task<int> GetOfferAmountAsync(GetOfferAmountParametersDto parameters)
    {
        throw new NotImplementedException();
    }

    public Task<List<OfferDto>> GetOfferListAsync(GetOfferListParametersDto parameters)
    {
        throw new NotImplementedException();
    }

    public Task<NewSearchRentDto> CreateNewRentAsync(ClaimsPrincipal claimsPrincipal)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SetRentStatusReadyToReturnAsync(int rentId)
    {
        throw new NotImplementedException();
    }
    
    private string GetUrlWithoutQuery(string endpoint = "")
    {
        var driveEasyApiUrl = Environment.GetEnvironmentVariable("DRIVE_EASY_API_URL");
        var url = $"{driveEasyApiUrl}{endpoint}";
        return url;
    }
}