using CarRentalAPI.DTOs.Combinations;
using CarRentalAPI.DTOs.Rents;
using CarRentalAPI.Enums;
using CarRentalAPI.Models;
using CarRentalAPI.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CarRentalAPI.Repositories.Implementations
{
    public class RentRepository : IRentRepository
    {
        private readonly CarRentalDbContext _context;

        public RentRepository(CarRentalDbContext context)
        {
            _context = context;
        }

        public async Task<List<CarIdRentDatesDto>> GetChosenCarActiveRentDatesAsync(string? brand, string? model, string? location) 
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
                throw new InvalidOperationException("Error while fetching rent data.", ex);
            }
        }

        public async Task CreateNewRentAsync(Rent rent)
        {
            await _context.Rents.AddAsync(rent);
            await _context.SaveChangesAsync();
        }

        public async Task<List<RentInfoDto>> GetRentInformationByStatusAsync(RentStatuses? status)
        {
            var rents = await _context.Rents
                .Include(r => r.Car)
                .ThenInclude(c => c.Model)
                .ThenInclude(m => m.Brand)
                .Where(r => status == null || r.Status == status)
                .Select(rent => new RentInfoDto
                {
                    RentId = rent.RentId,
                    BrandName = rent.Car.Model.Brand.Name,
                    ModelName = rent.Car.Model.Name,
                    RentStart = rent.RentStart,
                    RentEnd = rent.RentEnd,
                    RentStatus = rent.Status,
                    ImageUri = rent.ImageUri,
                    Description = rent.Description,
                })
                .ToListAsync();

            return rents;
        }
    }
}
