using CarSearchAPI.DTOs.ForwardingParameters;
using System.Security.Claims;

namespace CarSearchAPI.Abstractions;

public interface IExternalDataProvider
{
    // This interface exists in order to create a generalized structure for getting data from different APIs
    public Task<string> GetCarListAsync();
    public Task<string> GetOfferListAsync(GetOfferListParametersDto parameters);
    public Task<bool> CreateNewRent(ClaimsPrincipal claimsPrincipal);
    public string GetProviderName();
}