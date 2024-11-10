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
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Brand data
            modelBuilder.Entity<Brand>().HasData(
                new Brand { BrandId = 1, Name = "Tesla" },
                new Brand { BrandId = 2, Name = "Ford" },
                new Brand { BrandId = 3, Name = "Chevrolet" }
            );

            // Seed Model data
            modelBuilder.Entity<Model>().HasData(
                new Model { ModelId = 1, BrandId = 1, Name = "Model S", BasePrice = 1000, Year = "2024" },
                new Model { ModelId = 2, BrandId = 1, Name = "Model X", BasePrice = 1500, Year = "2024" },
                new Model { ModelId = 3, BrandId = 2, Name = "Mustang", BasePrice = 800, Year = "2023" },
                new Model { ModelId = 4, BrandId = 3, Name = "Camaro", BasePrice = 900, Year = "2023" }
            );

            // Seed Car data
            modelBuilder.Entity<Car>().HasData(
                new Car { CarId = 1, ModelId = 1, Mileage = 15000, IsActive = true },
                new Car { CarId = 2, ModelId = 1, Mileage = 12000, IsActive = false },
                new Car { CarId = 3, ModelId = 2, Mileage = 5000, IsActive = true },
                new Car { CarId = 4, ModelId = 3, Mileage = 3000, IsActive = true },
                new Car { CarId = 5, ModelId = 4, Mileage = 2500, IsActive = false }
            );
        }

    }
}
