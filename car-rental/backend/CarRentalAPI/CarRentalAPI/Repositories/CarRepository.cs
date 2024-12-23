﻿using CarRentalAPI.Abstractions.Repositories;
using CarRentalAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CarRentalAPI.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly CarRentalDbContext _context;

        public CarRepository(CarRentalDbContext context)
        {
            _context = context;
        }

        public async Task<List<Car>> GetCarsByIdAsync(List<int> ids, string? brand, string? model, string? location)
        {
            var cars = await _context.Cars
                .Where(c => ((!ids.Contains(c.CarId))) &&
                             (brand.IsNullOrEmpty() || c.Model.Brand.Name == brand) &&
                             (model.IsNullOrEmpty() || c.Model.Name == model) &&
                             (location.IsNullOrEmpty() || c.Location == location))
                .Include(c => c.Model)
                .ThenInclude(m => m.Brand).ToListAsync();
            return cars;
        }
    }
}
