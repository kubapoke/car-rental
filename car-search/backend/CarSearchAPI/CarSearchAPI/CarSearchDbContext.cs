using CarSearchAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CarSearchAPI
{
    public class CarSearchDbContext : DbContext
    {
        public CarSearchDbContext(DbContextOptions<CarSearchDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> applicationUsers {  get; set; }
    }
}
