using CarSearchAPI.DTOs.CarRental;

namespace CarSearchAPI.Abstractions
{
    public interface IEmailSender
    {
        Task<bool> SendNewRentEmailAsync(OfferDto recipientEmail);
    }
}
