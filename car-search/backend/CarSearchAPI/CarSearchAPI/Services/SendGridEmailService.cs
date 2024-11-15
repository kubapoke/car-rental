using CarSearchAPI.Abstractions;
using SendGrid.Helpers.Mail;
using SendGrid;
using CarSearchAPI.DTOs.CarRental;

namespace CarSearchAPI.Services
{
    public class SendGridEmailService : IEmailSender
    {
        private readonly IConfirmationTokenService _confirmationTokenService;

        public SendGridEmailService(IConfirmationTokenService confirmationTokenService)
        {
            _confirmationTokenService = confirmationTokenService;
        }

        public async Task<bool> SendEmailAsync(OfferDto info)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress(Environment.GetEnvironmentVariable("COMPANY_EMAIL"), "CarSearch");
            var subject = "Rent confirmation";
            var to = new EmailAddress(info.Email, info.Email);
            var plainTextContent = "";

            var token = _confirmationTokenService.GenerateConfirmationToken(info);
            string myAddress = Environment.GetEnvironmentVariable("MY_ADDRESS");
            string confirmationLink = myAddress + $"/api/Rents/new-rent-confirm?token={Uri.EscapeDataString(token)}";

            var htmlContent = $"<ul>Your rent:" +
                                  $"<li>From: {info.CompanyName}</li>" +
                                  $"<li>Your car: this is model name, which we currently don't have</li>" +
                                  $"<li>Price: {info.Price}</li>" +
                                  $"<li>Rent starts: {info.StartDate}</li>" +
                                  $"<li>Rent ends: {info.EndDate}</li>" +                                   
                              $"</ul><br/>" + $"<div>Click here to confirm your rent: {confirmationLink}</div>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            if (response.IsSuccessStatusCode) return true;
            else return false;
        }
    }
}
