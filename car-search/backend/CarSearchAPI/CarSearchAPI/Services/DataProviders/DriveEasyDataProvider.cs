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
    private readonly IProviderCustomerService _customerService;

    public DriveEasyDataProvider(IHttpClientFactory httpClientFactory, IProviderServiceFactory providerServiceFactory)
    {
        _httpClientFactory = httpClientFactory;
        _carService = providerServiceFactory.GetProviderCarService(GetProviderName());
        _offerService = providerServiceFactory.GetProviderOfferService(GetProviderName());
        _rentService = providerServiceFactory.GetProviderRentService(GetProviderName());
        _customerService = providerServiceFactory.GetProviderCustomerService(GetProviderName());
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

    public async Task<int> GetOfferAmountAsync(GetOfferAmountParametersDto parameters)
    {
        var client = _httpClientFactory.CreateClient();
        
        var url = GetUrlWithoutQuery();

        if (!(await CheckIfCustomerExistsAsync(parameters.Email)))
        {
            await CreateCustomerAsync(parameters.Email);
        }
        
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

    private async Task<bool> CheckIfCustomerExistsAsync(string? email)
    {
        var client = _httpClientFactory.CreateClient();
        
        var url = GetUrlWithoutQuery();
        
        return await _customerService.CheckIfCustomerExistsAsync(client, url, email);
    }

    private async Task CreateCustomerAsync(string? email)
    {
        var client = _httpClientFactory.CreateClient();
        
        var url = GetUrlWithoutQuery();
        
        await _customerService.CreateCustomerAsync(client, url, email);
    }
}