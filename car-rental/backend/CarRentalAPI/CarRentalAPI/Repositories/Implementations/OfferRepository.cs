using CarRentalAPI.Abstractions;
using CarRentalAPI.DTOs.CarSearch;
using CarRentalAPI.DTOs.Redis;
using CarRentalAPI.Models;
using CarRentalAPI.Repositories.Abstractions;
using CarRentalAPI.Services;
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

        public async Task<List<OfferForCarSearchDto>> CreateAndRetrieveOffersAsync(List<Car> cars, DateTime startDate, DateTime endDate, 
            string conditions, string companyName, string email, int? page, int? pageSize)
        {
            var redisOffers = new List<(string Guid, CachedOfferDto RedisOffer, decimal Price)>();

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
            
                redisOffers.Add((offerGuid, cachedOffer, price));
            }
            
            var redisTasks = redisOffers.Select(r => 
                _cacheService.GetSetValueTask(r.Guid, JsonConvert.SerializeObject(r.RedisOffer), TimeSpan.FromMinutes(15)));
            await Task.WhenAll(redisTasks);

            var offers = redisOffers.Select(r => new OfferForCarSearchDto
            {
                OfferId = r.Guid,
                Brand = r.RedisOffer.Brand,
                Model = r.RedisOffer.Model,
                Price = r.Price,
                Conditions = conditions,
                CompanyName = companyName,
                Location = r.RedisOffer.Location,
                StartDate = r.RedisOffer.StartDate,
                EndDate = r.RedisOffer.EndDate,
                Email = email,
            }).ToList();

            return offers;
        }

        public async Task<CachedOfferDto?> GetAndDeleteOfferAsync(string offerId)
        {
            var offerJson = await _cacheService.GetValueAndDeleteKeyAsync(offerId);
            var offer = offerJson != null ? JsonConvert.DeserializeObject<CachedOfferDto?>(offerJson) : null;
            
            return offer; 
        }
    }
}
