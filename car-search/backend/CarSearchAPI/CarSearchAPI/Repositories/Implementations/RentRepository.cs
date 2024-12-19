using CarSearchAPI.DTOs.Rents;
using CarSearchAPI.Migrations;
using CarSearchAPI.Models;
using CarSearchAPI.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CarSearchAPI.Repositories.Implementations
{
    public class RentRepository : IRentRepository
    {
        private readonly CarSearchDbContext _context;

        public RentRepository(CarSearchDbContext context)
        {
            _context = context;
        }

        public async Task<List<Rent>> GetUserRentsByEmailAsync(string email)
        {
            var rentList = await _context.rents.Where(r => r.UserEmail == email).ToListAsync();
            return rentList;
        }

        public async Task<Rent?> GetRentOrNullByIdAsync(int id)
        {
            var rent = await _context.rents.FirstOrDefaultAsync(r => r.RentId == id);
            return rent;
        }

        public async Task AddNewRentAsync(Rent rent)
        {
            _context.rents.Add(rent);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
