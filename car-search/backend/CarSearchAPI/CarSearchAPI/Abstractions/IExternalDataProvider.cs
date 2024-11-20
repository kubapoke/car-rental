using CarSearchAPI.DTOs.CarSearch;
using CarSearchAPI.DTOs.ForwardingParameters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarSearchAPI.Abstractions;

public interface IExternalDataProvider
{
    // This interface exists in order to create a generalized structure for getting data from different APIs
    public Task<string> GetCarListAsync();
    public Task<string> GetOfferListAsync(GetOfferListParametersDto parameters);
    public Task<NewSearchRentDto> CreateNewRentAsync(ClaimsPrincipal claimsPrincipal);
    public string GetProviderName();

}