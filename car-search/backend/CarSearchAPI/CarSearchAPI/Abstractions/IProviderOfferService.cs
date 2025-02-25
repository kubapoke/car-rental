﻿using CarSearchAPI.DTOs.CarRental;
using CarSearchAPI.DTOs.ForwardingParameters;

namespace CarSearchAPI.Abstractions
{
    public interface IProviderOfferService : IProviderService
    {
        public Task<int> GetOfferAmountAsync(HttpClient client, string url, GetOfferAmountParametersDto parameters,
            string? customerId);
        public Task<List<OfferDto>> GetOfferListAsync(HttpClient client, string url,
            GetOfferListParametersDto parameters, string? customerId);
    }
}
