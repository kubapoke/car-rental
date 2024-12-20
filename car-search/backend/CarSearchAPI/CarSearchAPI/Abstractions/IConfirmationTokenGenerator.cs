using System.Security.Claims;
using CarSearchAPI.DTOs.CarRental;

namespace CarSearchAPI.Abstractions
{
    public interface IConfirmationTokenGenerator
    {
        public string GenerateConfirmationToken(OfferDto info);
    }
}
