﻿using Microsoft.EntityFrameworkCore;

namespace CarSearchAPI
{
    public class CarSearchDbContext : DbContext
    {
        public CarSearchDbContext(DbContextOptions<CarSearchDbContext> options) : base(options) { }
    }
}
