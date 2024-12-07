using CarRentalAPI.Abstractions.Repositories;
using CarRentalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly CarRentalDbContext _context;

        public CarRepository(CarRentalDbContext context)
        {
            _context = context;
        }

        public async Task<List<Car>> GetCarsByIdAsync(List<int> ids)
        {
            var cars = await _context.Cars
                .Where(c => ids.Contains(c.CarId))
                .Include(c => c.Model)
                .ThenInclude(m => m.Brand).ToListAsync();
            return cars;
        }
    }
}
