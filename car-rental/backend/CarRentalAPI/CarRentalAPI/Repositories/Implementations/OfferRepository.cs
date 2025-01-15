using CarRentalAPI.Abstractions;
using CarRentalAPI.DTOs.Redis;
using CarRentalAPI.Models;
using CarRentalAPI.Repositories.Abstractions;
using Newtonsoft.Json;

namespace CarRentalAPI.Repositories.Implementations
{
    public class OfferRepository : IOfferRepository
    {
        private readonly ICacheService _cacheService;
        private readonly IPriceGenerator _priceGenerator;

        public OfferRepository(ICacheService cacheService, IPriceGenerator priceGenerator)
        {
            _cacheService = cacheService;
            _priceGenerator = priceGenerator;
        }

        public async Task<List<(string Guid, CachedOfferDto CachedOffer)>> CreateAndRetrieveCachedOffersAsync(
            List<Car> cars, DateTime startDate,
            DateTime endDate, string conditions, string companyName, int? page, int? pageSize)
        {
            var cachedOffers = new List<(string Guid, CachedOfferDto CachedOffer)>();

            foreach (var car in cars)
            {
                var offerGuid = Guid.NewGuid().ToString();
                var price = await _priceGenerator.GeneratePriceAsync(car.Model.BasePrice, startDate, endDate);
            
                CachedOfferDto cachedOffer = new CachedOfferDto
                {
                    CarId = car.CarId,
                    Brand = car.Model.Brand.Name,
                    Model = car.Model.Year == null ? car.Model.Name : car.Model.Name + " " + car.Model.Year,
                    Price = price,
                    Conditions = conditions,
                    CompanyName = companyName,
                    Location = car.Location,
                    StartDate = startDate,
                    EndDate = endDate,
                };
            
                cachedOffers.Add((offerGuid, cachedOffer));
            }
            
            var cacheTasks = cachedOffers.Select(r => 
                _cacheService.GetSetValueTask(r.Guid, JsonConvert.SerializeObject(r.CachedOffer), TimeSpan.FromMinutes(15)));
            await Task.WhenAll(cacheTasks);

            return cachedOffers;
        }

        public async Task<CachedOfferDto?> GetAndDeleteOfferAsync(string offerId)
        {
            var offerJson = await _cacheService.GetValueAndDeleteKeyAsync(offerId);
            var offer = offerJson != null ? JsonConvert.DeserializeObject<CachedOfferDto?>(offerJson) : null;
            
            return offer; 
        }

        public async Task<CachedOfferDto?> GetOfferAsync(string offerId)
        {
            var offerJson = await _cacheService.GetValueAsync(offerId);
            var offer = offerJson != null ? JsonConvert.DeserializeObject<CachedOfferDto?>(offerJson) : null;
            
            return offer; 
        }
    }
}
