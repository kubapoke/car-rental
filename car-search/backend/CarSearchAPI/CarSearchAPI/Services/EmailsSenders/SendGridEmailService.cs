using CarSearchAPI.Abstractions;
using SendGrid.Helpers.Mail;
using SendGrid;
using CarSearchAPI.DTOs.CarRental;

namespace CarSearchAPI.Services.EmailsSenders
{
    public class SendGridEmailService : IEmailSender
    {
        private readonly IConfirmationTokenService _confirmationTokenService;

        public SendGridEmailService(IConfirmationTokenService confirmationTokenService)
        {
            _confirmationTokenService = confirmationTokenService;
        }

        public async Task<bool> SendNewRentEmailAsync(OfferDto info)
        {         

            var from = GetSenderEmailAddress();            
            var to = new EmailAddress(info.Email, info.Email);

            var subject = "Rent confirmation";
            var plainTextContent = "";

            var htmlContent = GetHtmlContentOfNewRentEmail(info);

            string apiKey = GetSendGridApiKey();
            bool isSuccess = await SendEmailAsync(from, to, subject, plainTextContent, htmlContent, apiKey);
            return isSuccess;
        }

        private string  GetHtmlContentOfNewRentEmail(OfferDto info)
        {
            string confirmationLink = CreateConfirmationLink(info);

            string htmlContent = $"<ul>Your rent:" +
                                  $"<li>From: {info.CompanyName}</li>" +
                                  $"<li>Your car: {info.Brand} {info.Model}</li>" +
                                  $"<li>Price: {info.Price}</li>" +
                                  $"<li>Rent starts: {info.StartDate}</li>" +
                                  $"<li>Rent ends: {info.EndDate}</li>" +
                              $"</ul><br/>" + $"<div>Click here to confirm your rent: {confirmationLink}</div>";
            return htmlContent;
        }

        private string CreateConfirmationLink(OfferDto info) 
        {
            var token = _confirmationTokenService.GenerateConfirmationToken(info);
            string myAddress = GetMyAddress();
            string confirmationLink = myAddress + $"/api/Rents/new-rent-confirm?token={Uri.EscapeDataString(token)}";
            return confirmationLink;
        }

        private async Task<bool> SendEmailAsync(EmailAddress from, EmailAddress to, string subject, string plainTextContent, string htmlContent, string apiKey)
        {
            var client = new SendGridClient(apiKey);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            if (response.IsSuccessStatusCode) return true;
            else return false;
        }

        private string GetSendGridApiKey()
        {
            return Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
        }

        private EmailAddress GetSenderEmailAddress()
        {
            return new EmailAddress(Environment.GetEnvironmentVariable("COMPANY_EMAIL"), "CarSearch");
        }

        private string GetMyAddress()
        {
            return Environment.GetEnvironmentVariable("MY_ADDRESS");
        }
    }
}
