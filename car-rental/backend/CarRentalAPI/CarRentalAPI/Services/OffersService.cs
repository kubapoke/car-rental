using CarRentalAPI.Abstractions;
using CarRentalAPI.Abstractions.Repositories;
using CarRentalAPI.DTOs.CarSearch;
using CarRentalAPI.DTOs.Combinations;
using CarRentalAPI.DTOs.Redis;
using CarRentalAPI.Models;
using Newtonsoft.Json;

namespace CarRentalAPI.Services
{
    public class OffersService
    {
        private readonly IRentRepository _rentRepository;
        private readonly ICarRepository _carRepository;
        private readonly AvailabilityChecker _availabilityChecker;
        private readonly IPriceGenerator _priceGenerator;
        private readonly RedisCacheService _redisCacheService;

        public OffersService(IRentRepository rentRepository, ICarRepository carRepository, AvailabilityChecker availabilityChecker, 
            IPriceGenerator priceGenerator, RedisCacheService redisCacheService)
        {
            _rentRepository = rentRepository;
            _carRepository = carRepository;
            _availabilityChecker = availabilityChecker;
            _priceGenerator = priceGenerator;
            _redisCacheService = redisCacheService;
        }

        public async Task<List<OfferForCarSearchDto>> GetNewOffers(string? brand, string? model, DateTime startDate, DateTime endDate, string? location, string email, string conditions, string companyName)
        {
            List<CarIdRentDatesDto> pairs = await _rentRepository.GetChosenCarActiveRentDatesAsync(brand, model, location);
            List<int> notAvailableCarIds = _availabilityChecker.CheckForNotAvailableCars(pairs, startDate, endDate);
            List<Car> availableCars = await _carRepository.GetCarsByIdAsync(notAvailableCarIds, brand, model, location);

            List<OfferForCarSearchDto> newOffers = new List<OfferForCarSearchDto>();
            foreach (var car in availableCars)
            {
                newOffers.Add(await GenerateOfferAsync(car, startDate, endDate, email, conditions, companyName));
            }

            return newOffers;
        }

        private async Task<OfferForCarSearchDto> GenerateOfferAsync(Car car, DateTime startDate, DateTime endDate, string email, string conditions, string companyName)
        {
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

            var offerGuid = Guid.NewGuid().ToString();
            var serializedOffer = JsonConvert.SerializeObject(redisOffer);
                
            await _redisCacheService.SetValueAsync(offerGuid, serializedOffer, TimeSpan.FromMinutes(15));

            OfferForCarSearchDto generatedOffer = new OfferForCarSearchDto
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
            
            return generatedOffer;
        }
    }
}
