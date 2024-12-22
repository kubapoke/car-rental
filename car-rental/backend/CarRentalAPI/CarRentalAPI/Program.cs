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

var builder = WebApplication.CreateBuilder(args);

// Load the environment variables
DotNetEnv.Env.Load();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<CarRentalDbContext>(options =>
                options.UseSqlServer(
                    Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING"),
                    sqlOptions =>
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null)));

builder.Services.AddScoped<IPriceGenerator, PricePerDayToHourGeneratorService>();
builder.Services.AddScoped<IEmailSender, SendGridEmailSender>();
builder.Services.AddScoped<PasswordHasher>();
builder.Services.AddScoped<SessionTokenManager>();
builder.Services.AddScoped<IStorageManager, AzureBlobStorageManager>();
builder.Services.AddScoped<IRentRepository, RentRepository>();
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IOfferRepository, OfferRepository>();
builder.Services.AddScoped<AvailabilityChecker>();
builder.Services.AddScoped<OffersService>();
builder.Services.AddScoped<IManagerService, ManagerService>();
builder.Services.AddScoped<IManagerRepository, ManagerRepository>();
builder.Services.AddSingleton<RedisCacheService>(provider =>
{
    var connectionString = Environment.GetEnvironmentVariable("REDIS_DATABASE_CONNECTION");
    return new RedisCacheService(connectionString);
});

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
