using CarRentalAPI.Abstractions;

namespace CarRentalAPI.Services
{
    public class SendGridEmailService : IEmailSender
    {
        public async Task<bool> SendEmailAsync()
        {
            return true;
        }
    }
}
