using CarRentalAPI.Abstractions.Repositories;
using CarRentalAPI.DTOs.Combinations;

namespace CarRentalAPI.Repositories
{
    public class RentRepository : IRentRepository
    {
        private readonly CarRentalDbContext _context;

        public RentRepository(CarRentalDbContext context)
        {
            _context = context;
        }

        public List<CarIdRentDatesDto> GetChosenCarActiveRentDates(string? brand, string? model, DateTime startDate, DateTime endDate, string? location) 
        {
            throw new NotImplementedException();
        }
    }
}
