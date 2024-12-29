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
            
            availableCars = _paginationService.TrimToPage(availableCars, page, pageSize);
            var newOffers = await _offerRepository.CreateAndRetrieveOffersAsync(availableCars, 
                startDate, endDate, conditions, companyName, email, page, pageSize);
            
            return newOffers;
        }

        public async Task<CachedOfferDto?> GetAndDeleteOfferByIdAsync(string offerId)
        {
            var offer = await _offerRepository.GetAndDeleteOfferAsync(offerId);
            return offer;
        }
    }
}
