using CarRentalAPI.Abstractions;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace CarRentalAPI.Services
{
    public class SendGridEmailService : IEmailSender
    {
        public async Task<bool> SendEmailAsync(string recipientEmail)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(Environment.GetEnvironmentVariable("COMPANY_EMAIL"), "SendGridEnjoyer");
            var subject = "WHAT A PLEASURE TO USE SENDGRID";
            var to = new EmailAddress(recipientEmail, "Egor");
            var plainTextContent = "What could be better than sitting with this for 3 hours";
            var htmlContent = "<strong>What could be better than sitting with this for 3 hours</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            if (response.IsSuccessStatusCode) return true;
            else return false;
        }
    }
}
