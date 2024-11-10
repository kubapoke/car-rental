using CarRentalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI
{
    public class CarRentalDbContext : DbContext
    {
        public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options) : base(options) { }
        
        public DbSet<Car> Cars { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Rent> Rents { get; set; }
    }
}
