using CarSearchAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using CarSearchAPI.Utilities;
using CarSearchAPI.Services;
using CarSearchAPI.Abstractions;
using CarSearchAPI.Services.DataProviders;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using CarSearchAPI.Repositories.Abstractions;
using CarSearchAPI.Repositories.Implementations;

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
builder.Services.AddDbContext<CarSearchDbContext>(options =>
                options.UseSqlServer(
                    Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING"),
                    sqlOptions =>
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null)));
builder.Services.AddHttpClient();

// Services
builder.Services.AddScoped<IEmailSender, SendGridEmailService>();
builder.Services.AddScoped<IConfirmationTokenService, JwtConfirmationTokenService>();
builder.Services.AddScoped<IAuthService, GoogleAuthService>();
builder.Services.AddScoped<ISessionTokenManager, JwtSessionTokenManager>();

// Repositories
builder.Services.AddScoped<IUserRepository, ApplicationUserRepository>();

// Register external data providers
builder.Services.AddScoped<IExternalDataProvider, CarRentalDataProvider>();

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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_TOKEN_SECRET_KEY"))),
            ValidateLifetime = true
        };
    });

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
