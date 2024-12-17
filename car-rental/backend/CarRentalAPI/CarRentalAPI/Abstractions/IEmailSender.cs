using CarRentalAPI.Models;

namespace CarRentalAPI.Abstractions
{
    public interface IEmailSender
    {
        Task<bool> SendReturnConfirmedEmailAsync(Rent rent);
    }
}
