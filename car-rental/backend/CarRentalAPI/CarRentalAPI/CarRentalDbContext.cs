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
                new Brand { BrandId = 3, Name = "Chevrolet" },
                new Brand { BrandId = 4, Name = "BMW" },
                new Brand { BrandId = 5, Name = "Audi" },
                new Brand { BrandId = 6, Name = "Toyota" },
                new Brand { BrandId = 7, Name = "Mercedes-Benz" },
                new Brand { BrandId = 8, Name = "Volkswagen" },
                new Brand { BrandId = 9, Name = "Hyundai" },
                new Brand { BrandId = 10, Name = "Nissan" }
            );

            // Seed Model data
            modelBuilder.Entity<Model>().HasData(
                new Model { ModelId = 1, BrandId = 1, Name = "Model S", BasePrice = 1000 },
                new Model { ModelId = 2, BrandId = 1, Name = "Model X", BasePrice = 1500 },
                new Model { ModelId = 3, BrandId = 1, Name = "Model 3", BasePrice = 900 },
                new Model { ModelId = 4, BrandId = 2, Name = "Mustang", BasePrice = 800, Year = "2023" },
                new Model { ModelId = 5, BrandId = 2, Name = "F-150", BasePrice = 700 },
                new Model { ModelId = 6, BrandId = 3, Name = "Camaro", BasePrice = 900 },
                new Model { ModelId = 7, BrandId = 3, Name = "Silverado", BasePrice = 950, Year = "2024" },
                new Model { ModelId = 8, BrandId = 4, Name = "X5", BasePrice = 950 },
                new Model { ModelId = 9, BrandId = 4, Name = "3 Series", BasePrice = 1050, Year = "2023" },
                new Model { ModelId = 10, BrandId = 5, Name = "A4", BasePrice = 1000 },
                new Model { ModelId = 11, BrandId = 5, Name = "A6", BasePrice = 1100 },
                new Model { ModelId = 12, BrandId = 6, Name = "Corolla", BasePrice = 700 },
                new Model { ModelId = 13, BrandId = 6, Name = "Camry", BasePrice = 800, Year = "2023" },
                new Model { ModelId = 14, BrandId = 7, Name = "C-Class", BasePrice = 1300 },
                new Model { ModelId = 15, BrandId = 7, Name = "E-Class", BasePrice = 1500, Year = "2024" },
                new Model { ModelId = 16, BrandId = 8, Name = "Golf", BasePrice = 650 },
                new Model { ModelId = 17, BrandId = 8, Name = "Passat", BasePrice = 750, Year = "2023" },
                new Model { ModelId = 18, BrandId = 9, Name = "Elantra", BasePrice = 600 },
                new Model { ModelId = 19, BrandId = 9, Name = "Sonata", BasePrice = 750, Year = "2022" },
                new Model { ModelId = 20, BrandId = 10, Name = "Altima", BasePrice = 750 },
                new Model { ModelId = 21, BrandId = 10, Name = "Sentra", BasePrice = 680, Year = "2023" }
            );

            // Seed Car data
            modelBuilder.Entity<Car>().HasData(
                new Car { CarId = 1, ModelId = 1, Mileage = 15000, IsActive = true, Location = "Warsaw" },
                new Car { CarId = 2, ModelId = 1, Mileage = 12000, IsActive = true, Location = "Krakow" },
                new Car { CarId = 3, ModelId = 2, Mileage = 5000, IsActive = true, Location = "Bydgoszcz" },
                new Car { CarId = 4, ModelId = 3, Mileage = 3000, IsActive = true, Location = "Warsaw" },
                new Car { CarId = 5, ModelId = 3, Mileage = 2500, IsActive = true, Location = "Krakow" },
                new Car { CarId = 6, ModelId = 4, Mileage = 8000, IsActive = true, Location = "Bydgoszcz" },
                new Car { CarId = 7, ModelId = 4, Mileage = 15000, IsActive = true, Location = "Warsaw" },
                new Car { CarId = 8, ModelId = 5, Mileage = 20000, IsActive = true, Location = "Krakow" },
                new Car { CarId = 9, ModelId = 5, Mileage = 7000, IsActive = true, Location = "Bydgoszcz" },
                new Car { CarId = 10, ModelId = 6, Mileage = 500, IsActive = true, Location = "Warsaw" },
                new Car { CarId = 11, ModelId = 7, Mileage = 11000, IsActive = true, Location = "Krakow" },
                new Car { CarId = 12, ModelId = 7, Mileage = 13000, IsActive = true, Location = "Bydgoszcz" },
                new Car { CarId = 13, ModelId = 8, Mileage = 18000, IsActive = true, Location = "Warsaw" },
                new Car { CarId = 14, ModelId = 8, Mileage = 7500, IsActive = true, Location = "Krakow" },
                new Car { CarId = 15, ModelId = 9, Mileage = 6200, IsActive = true, Location = "Bydgoszcz" },
                new Car { CarId = 16, ModelId = 9, Mileage = 4000, IsActive = true, Location = "Warsaw" },
                new Car { CarId = 17, ModelId = 10, Mileage = 14000, IsActive = true, Location = "Krakow" },
                new Car { CarId = 18, ModelId = 10, Mileage = 6000, IsActive = true, Location = "Bydgoszcz" },
                new Car { CarId = 19, ModelId = 11, Mileage = 9200, IsActive = true, Location = "Warsaw" },
                new Car { CarId = 20, ModelId = 11, Mileage = 10500, IsActive = true, Location = "Krakow" },
                new Car { CarId = 21, ModelId = 12, Mileage = 8900, IsActive = true, Location = "Bydgoszcz" },
                new Car { CarId = 22, ModelId = 12, Mileage = 10000, IsActive = true, Location = "Warsaw" },
                new Car { CarId = 23, ModelId = 13, Mileage = 4300, IsActive = true, Location = "Krakow" },
                new Car { CarId = 24, ModelId = 13, Mileage = 5100, IsActive = true, Location = "Bydgoszcz" },
                new Car { CarId = 25, ModelId = 14, Mileage = 2200, IsActive = true, Location = "Warsaw" },
                new Car { CarId = 26, ModelId = 15, Mileage = 14500, IsActive = true, Location = "Krakow" },
                new Car { CarId = 27, ModelId = 16, Mileage = 7500, IsActive = true, Location = "Bydgoszcz" },
                new Car { CarId = 28, ModelId = 17, Mileage = 14000, IsActive = true, Location = "Warsaw" },
                new Car { CarId = 29, ModelId = 18, Mileage = 9300, IsActive = true, Location = "Krakow" },
                new Car { CarId = 30, ModelId = 18, Mileage = 6200, IsActive = true, Location = "Bydgoszcz" }
            );
        }

    }
}
