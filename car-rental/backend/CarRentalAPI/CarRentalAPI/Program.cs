using CarRentalAPI;
using Microsoft.EntityFrameworkCore;
using CarRentalAPI.Services;
using CarRentalAPI.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CarRentalAPI.Repositories;
using CarRentalAPI.Repositories.Abstractions;
using CarRentalAPI.Repositories.Implementations;
using CarRentalAPI.Services.AvailabilityCheckers;
using CarRentalAPI.Services.CacheServices;
using CarRentalAPI.Services.CarServices;
using CarRentalAPI.Services.EmailSenders;
using CarRentalAPI.Services.ManagerServices;
using CarRentalAPI.Services.OfferServices;
using CarRentalAPI.Services.PasswordServices;
using CarRentalAPI.Services.PriceGenerators;
using CarRentalAPI.Services.RentServices;
using CarRentalAPI.Services.StorageManagers;
using CarRentalAPI.Services.TokenManagers;

var builder = WebApplication.CreateBuilder(args);

// Load the environment variables
DotNetEnv.Env.Load();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// Context
builder.Services.AddDbContext<CarRentalDbContext>(options =>
                options.UseSqlServer(
                    Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING"),
                    sqlOptions =>
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null)));

// Model related services
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IOfferService, OfferService>();
builder.Services.AddScoped<IManagerService, ManagerService>();
builder.Services.AddScoped<IRentService, RentService>();

// Other services
builder.Services.AddScoped<ISessionTokenManager, JwtSessionTokenManager>();
builder.Services.AddScoped<IPriceGenerator, PricePerDayToHourGenerator>();
builder.Services.AddScoped<IEmailSender, SendGridEmailSender>();
builder.Services.AddScoped<IPasswordService, Hmacsha256PasswordService>();
builder.Services.AddScoped<IStorageManager, AzureBlobStorageManager>();
builder.Services.AddScoped<IAvailabilityChecker, AvailabilityChecker>();
builder.Services.AddScoped<IManagerRepository, ManagerRepository>();

// Repositories
builder.Services.AddScoped<IRentRepository, RentRepository>();
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IOfferRepository, OfferRepository>();

// Cache service
builder.Services.AddSingleton<ICacheService>(provider =>
{
    var connectionString = Environment.GetEnvironmentVariable("REDIS_DATABASE_CONNECTION");
    return new RedisCacheService(connectionString);
});

// Authentication
builder.Services.AddAuthentication(options => // that is instruction, how to check bearer token
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters // we check only using secret key, write secret key to .env (usually 256 bit key)
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("CAR_RENTAL_SECRET_KEY"))),
            ValidateLifetime = false
        };
    });

// Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Manager", policy => policy.RequireClaim("UserName"));
    options.AddPolicy("Backend", policy => policy.RequireClaim("Backend"));
});

var allowedOrigins = Environment.GetEnvironmentVariable("ALLOWED_ORIGINS")?.Split(',');

if(allowedOrigins != null)
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", corsPolicyBuilder =>
        {
            corsPolicyBuilder.WithOrigins(allowedOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseCors("CorsPolicy");

app.Run();
