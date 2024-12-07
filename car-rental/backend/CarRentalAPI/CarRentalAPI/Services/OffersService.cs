using CarRentalAPI.Abstractions.Repositories;
using CarRentalAPI.DTOs.CarSearch;
using CarRentalAPI.DTOs.Combinations;

namespace CarRentalAPI.Services
{
    public class OffersService
    {
        private readonly IRentRepository _rentRepository;

        public OffersService(IRentRepository rentRepository)
        {
            _rentRepository = rentRepository;
        }

        public async Task<List<OfferForCarSearchDto>>  GetNewOffers(string? brand, string? model, DateTime startDate, DateTime endDate, string? location, string email)
        {
            List<CarIdRentDatesDto> pairs = await _rentRepository.GetChosenCarActiveRentDatesAsync(brand, model, location);

            throw new NotImplementedException();
        }
    }
}
