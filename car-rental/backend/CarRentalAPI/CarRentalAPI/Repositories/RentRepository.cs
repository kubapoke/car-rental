using CarRentalAPI.Abstractions.Repositories;
using CarRentalAPI.DTOs.Combinations;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CarRentalAPI.Repositories
{
    public class RentRepository : IRentRepository
    {
        private readonly CarRentalDbContext _context;

        public RentRepository(CarRentalDbContext context)
        {
            _context = context;
        }

        public async Task<List<CarIdRentDatesDto>> GetChosenCarActiveRentDates(string? brand, string? model, string? location) 
        {
            try
            {
                var query = from r in _context.Rents
                            join c in _context.Cars on r.CarId equals c.CarId
                            where
                                (brand.IsNullOrEmpty() || c.Model.Brand.Name == brand) &&
                                (model.IsNullOrEmpty() || c.Model.Name == model) &&
                                (location.IsNullOrEmpty() || c.Location == location) &&
                                (r.RentEnd > DateTime.Today)
                            select new CarIdRentDatesDto { CarId = c.CarId, StartDate = r.RentStart, EndDate = r.RentEnd.Value };
                return await query.ToListAsync();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("With high probability rent without RentEnd appeared in the database for some reason\n");
                throw new InvalidOperationException("Error while fetching rent data.", ex);

            }

        }
    }
}
