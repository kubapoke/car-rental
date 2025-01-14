using CarSearchAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using CarSearchAPI.Utilities;
using CarSearchAPI.Abstractions;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using CarSearchAPI.Repositories.Abstractions;
using CarSearchAPI.Repositories.Implementations;
using CarSearchAPI.Services.Authenticators;
using CarSearchAPI.Services.DataProviders;
using CarSearchAPI.Services.TokenManagers;
using CarSearchAPI.Services.EmailsSenders;
using Microsoft.AspNetCore.WebSockets;
using CarSearchAPI.Services.UserServices;
using CarSearchAPI.Services.RentServices;
using CarSearchAPI.Services.OfferServices;
using CarSearchAPI.Services.ProviderServices;
using CarSearchAPI.Services.ProviderServices.ProviderCarServices;
using CarSearchAPI.Services.ProviderServices.ProviderOfferServices;
using CarSearchAPI.Services.ProviderServices.ProviderRentServices;

var builder = WebApplication.CreateBuilder(args);

// Load the environment variables
DotNetEnv.Env.Load();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
    .AddJsonOptions(options =>  // this will add instruction of how to serialize/deserialize different datatypes (not sure of how it works) 
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    });

// context
builder.Services.AddDbContext<CarSearchDbContext>(options =>
                options.UseSqlServer(
                    Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING"),
                    sqlOptions =>
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null)));
builder.Services.AddHttpClient();

// Model related services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRentService, RentService>();

// Token related services
builder.Services.AddScoped<IConfirmationTokenGenerator, JwtConfirmationTokenGenerator>();
builder.Services.AddScoped<IConfirmationTokenValidator, JwtConfirmationTokenValidator>();
builder.Services.AddScoped<ISessionTokenManager, JwtSessionTokenManager>();

// Provider services
builder.Services.AddScoped<IProviderServiceFactory, ProviderServiceFactory>();
builder.Services.AddTransient<IProviderCarService, CarRentalProviderCarService>();
builder.Services.AddTransient<IProviderOfferService, CarRentalProviderOfferService>();
builder.Services.AddTransient<IProviderRentService, CarRentalProviderRentService>();

// Other services
builder.Services.AddScoped<IEmailSender, SendGridEmailService>();
builder.Services.AddScoped<IAuthService, GoogleAuthService>();
builder.Services.AddScoped<IOfferService, OfferService>();
builder.Services.AddScoped<IOfferPageService, OfferPageService>();

// Repositories
builder.Services.AddScoped<IUserRepository, ApplicationUserRepository>();
builder.Services.AddScoped<IRentRepository, RentRepository>();

// External data providers
builder.Services.AddScoped<IExternalDataProvider, CarRentalDataProvider>();

// Authentication
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters 
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_TOKEN_SECRET_KEY"))),
            ValidateLifetime = true
        };
    });

// Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ProtoUser", policy => policy.RequireClaim("ProtoUserClaim"));
    options.AddPolicy("LegitUser", policy => policy.RequireClaim("LegitUserClaim"));
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
