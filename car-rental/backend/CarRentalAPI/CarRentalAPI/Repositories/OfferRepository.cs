﻿using CarRentalAPI.Abstractions;
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

        public async Task<string> CreateOfferForRedisAsync(Car car, DateTime startDate, DateTime endDate, string conditions,
            string companyName)
        {
            var offerGuid = Guid.NewGuid().ToString();
            
            OfferForRedisDto redisOffer = new OfferForRedisDto
            {
                CarId = car.CarId,
                Brand = car.Model.Brand.Name,
                Model = car.Model.Year == null ? car.Model.Name : car.Model.Name + " " + car.Model.Year,
                Price = await _priceGenerator.GeneratePriceAsync(car.Model.BasePrice, startDate, endDate),
                Conditions = conditions,
                CompanyName = companyName,
                Location = car.Location,
                StartDate = startDate,
                EndDate = endDate,
            };
            
            var serializedOffer = JsonConvert.SerializeObject(redisOffer);
            
            await _redisCacheService.SetValueAsync(offerGuid, serializedOffer, TimeSpan.FromMinutes(15));

            return offerGuid;
        }

        public async Task<OfferForCarSearchDto?> CreateCarSearchOfferFromRedisOfferAsync(string offerGuid, string email)
        {
            var offerJson = await _redisCacheService.GetValueAndRefreshExpiryAsync(offerGuid, TimeSpan.FromMinutes(15));

            if (offerJson == null)
            {
                throw new ArgumentNullException(nameof(offerJson));
            }
            
            var redisOffer = JsonConvert.DeserializeObject<OfferForRedisDto>(offerJson);

            if (redisOffer == null)
            {
                throw new ArgumentNullException(nameof(offerJson));
            }
            
            OfferForCarSearchDto offer = new OfferForCarSearchDto
            {
                OfferId = offerGuid,
                Brand = redisOffer.Brand,
                Model = redisOffer.Model,
                Price = redisOffer.Price,
                Conditions = redisOffer.Conditions,
                CompanyName = redisOffer.CompanyName,
                Location = redisOffer.Location,
                StartDate = redisOffer.StartDate,
                EndDate = redisOffer.EndDate,
                Email = email,
            };
            
            return offer;
        }

        public async Task<List<OfferForCarSearchDto>> CreateAndRetrieveOffersAsync(List<Car> cars, DateTime startDate, DateTime endDate, 
            string conditions, string companyName, string email)
        {
            List<OfferForCarSearchDto> offers = new List<OfferForCarSearchDto>();
            List<string> guids = new List<string>();
            List<Task<bool>> addTasks = new List<Task<bool>>();

            foreach (var car in cars)
            {
                var offerGuid = Guid.NewGuid().ToString();
                guids.Add(offerGuid);
            
                OfferForRedisDto redisOffer = new OfferForRedisDto
                {
                    CarId = car.CarId,
                    Brand = car.Model.Brand.Name,
                    Model = car.Model.Year == null ? car.Model.Name : car.Model.Name + " " + car.Model.Year,
                    Price = await _priceGenerator.GeneratePriceAsync(car.Model.BasePrice, startDate, endDate),
                    Conditions = conditions,
                    CompanyName = companyName,
                    Location = car.Location,
                    StartDate = startDate,
                    EndDate = endDate,
                };
            
                var serializedOffer = JsonConvert.SerializeObject(redisOffer);
            
                addTasks.Add(_redisCacheService.GetSetValueTask(offerGuid, serializedOffer, TimeSpan.FromMinutes(15)));
            }
            Task.WaitAll(addTasks.ToArray());

            int id = 0;
            
            foreach (var car in cars)
            {
                OfferForCarSearchDto offer = new OfferForCarSearchDto
                {
                    OfferId = guids[id++],
                    Brand = car.Model.Brand.Name,
                    Model = car.Model.Year == null ? car.Model.Name : car.Model.Name + " " + car.Model.Year,
                    Price = await _priceGenerator.GeneratePriceAsync(car.Model.BasePrice, startDate, endDate),
                    Conditions = conditions,
                    CompanyName = companyName,
                    Location = car.Location,
                    StartDate = startDate,
                    EndDate = endDate,
                    Email = email,
                };
                
                offers.Add(offer);
            }
            
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
