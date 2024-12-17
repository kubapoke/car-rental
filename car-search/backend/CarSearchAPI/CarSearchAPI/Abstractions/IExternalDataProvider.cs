using CarSearchAPI.DTOs.CarSearch;
using CarSearchAPI.DTOs.ForwardingParameters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CarSearchAPI.DTOs.CarRental;
using CarSearchAPI.DTOs.Users;
using CarSearchAPI.Models;

namespace CarSearchAPI.Abstractions;

public interface IExternalDataProvider
{
    // This interface exists in order to create a generalized structure for getting data from different APIs
    public Task<List<CarDto>> GetCarListAsync();
    public Task<List<OfferDto>> GetOfferListAsync(GetOfferListParametersDto parameters);
    public Task<NewSearchRentDto> CreateNewRentAsync(ClaimsPrincipal claimsPrincipal);
    public string GetProviderName(); 
    public Task<HttpResponseMessage> UpdateRentStatusAsync(int rentId, RentStatus status);
}