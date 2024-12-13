using CarRentalAPI.Abstractions;
using CarRentalAPI.Abstractions.Repositories;
using CarRentalAPI.DTOs.CarSearch;
using CarRentalAPI.DTOs.Redis;
using CarRentalAPI.Models;
using CarRentalAPI.Services;
using Newtonsoft.Json;

namespace CarRentalAPI.Repositories
{
    public class OfferRepository : IOfferRepository
    {
        private readonly RedisCacheService _redisCacheService;
        private readonly IPriceGenerator _priceGenerator;

        public OfferRepository(RedisCacheService redisCacheService, IPriceGenerator priceGenerator)
        {
            _redisCacheService = redisCacheService;
            _priceGenerator = priceGenerator;
        }

        public async Task<List<OfferForCarSearchDto>> CreateAndRetrieveOffersAsync(List<Car> cars, DateTime startDate, DateTime endDate, 
            string conditions, string companyName, string email)
        {
            var redisOffers = new List<(string Guid, OfferForRedisDto RedisOffer, decimal Price)>();

            foreach (var car in cars)
            {
                var offerGuid = Guid.NewGuid().ToString();
                var price = await _priceGenerator.GeneratePriceAsync(car.Model.BasePrice, startDate, endDate);
            
                OfferForRedisDto redisOffer = new OfferForRedisDto
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
            
                redisOffers.Add((offerGuid, redisOffer, price));
            }
            
            var redisTasks = redisOffers.Select(r => 
                _redisCacheService.GetSetValueTask(r.Guid, JsonConvert.SerializeObject(r.RedisOffer), TimeSpan.FromMinutes(15)));
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

        public async Task<OfferForRedisDto?> GetAndDeleteOfferAsync(string offerId)
        {
            var offerJson = await _redisCacheService.GetValueAndDeleteKeyAsync(offerId);
            var offer = offerJson != null ? JsonConvert.DeserializeObject<OfferForRedisDto?>(offerJson) : null;
            
            return offer; 
        }
    }
}
