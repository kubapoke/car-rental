using CarRentalAPI.Abstractions;
using CarRentalAPI.DTOs.CarSearch;
using CarRentalAPI.DTOs.Redis;
using CarRentalAPI.Repositories.Abstractions;

namespace CarRentalAPI.Services.OfferServices
{
    public class OfferService : IOfferService
    {
        private readonly IRentRepository _rentRepository;
        private readonly ICarRepository _carRepository;
        private readonly IAvailabilityChecker _availabilityChecker;
        private readonly IOfferRepository _offerRepository;
        private readonly IPaginationService _paginationService;

        public OfferService(IRentRepository rentRepository, ICarRepository carRepository, 
            IOfferRepository offerRepository, IAvailabilityChecker availabilityChecker,
            IPaginationService paginationService)
        {
            _rentRepository = rentRepository;
            _carRepository = carRepository;
            _offerRepository = offerRepository;
            _availabilityChecker = availabilityChecker;
            _paginationService = paginationService;
        }

        public async Task<int> GetOffersCountAsync(string? brand, string? model, DateTime startDate,
            DateTime endDate, string? location)
        {
            var pairs = await _rentRepository.GetChosenCarActiveRentDatesAsync(brand, model, location);
            var notAvailableCarIds = _availabilityChecker.GetNotAvailableCarIds(pairs, startDate, endDate);
            var availableCars = await _carRepository.GetCarsByIdAndFiltersAsync(notAvailableCarIds, brand, model, location);
            
            return availableCars.Count;
        }
        
        public async Task<List<OfferForCarSearchDto>> GetNewOffersAsync(string? brand, string? model, DateTime startDate,
            DateTime endDate, string? location, string email, string conditions, string companyName, int? page, int? pageSize)
        {
            var pairs = await _rentRepository.GetChosenCarActiveRentDatesAsync(brand, model, location);
            var notAvailableCarIds = _availabilityChecker.GetNotAvailableCarIds(pairs, startDate, endDate);
            var availableCars = await _carRepository.GetCarsByIdAndFiltersAsync(notAvailableCarIds, brand, model, location);
            
            var trimmedAvailableCars = _paginationService.TrimListToPage(availableCars, page, pageSize);
            var newOffers = await _offerRepository.CreateAndRetrieveCachedOffersAsync(trimmedAvailableCars, 
                startDate, endDate, conditions, companyName, page, pageSize);
            
            return ConvertCachedOffersToCarSearchOffers(newOffers, email);
        }

        public async Task<CachedOfferDto?> GetAndDeleteOfferByIdAsync(string offerId)
        {
            var offer = await _offerRepository.GetAndDeleteOfferAsync(offerId);
            return offer;
        }

        private List<OfferForCarSearchDto> ConvertCachedOffersToCarSearchOffers(
            List<(string Guid, CachedOfferDto CachedOffer)> cachedOffers, string email)
        {
            var offers = cachedOffers.Select(r => new OfferForCarSearchDto
            {
                OfferId = r.Guid,
                Brand = r.CachedOffer.Brand,
                Model = r.CachedOffer.Model,
                Price = r.CachedOffer.Price,
                Conditions = r.CachedOffer.Conditions,
                CompanyName = r.CachedOffer.CompanyName,
                Location = r.CachedOffer.Location,
                StartDate = r.CachedOffer.StartDate,
                EndDate = r.CachedOffer.EndDate,
                Email = email,
            }).ToList();

            return offers;
        }
    }
}
