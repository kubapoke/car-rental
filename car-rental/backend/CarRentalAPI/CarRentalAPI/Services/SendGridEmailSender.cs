using CarRentalAPI.Abstractions;
using CarRentalAPI.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using SendGrid.Helpers.Mail.Model;

namespace CarRentalAPI.Services
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly IPriceGenerator _priceGenerator;

        public SendGridEmailSender(IPriceGenerator pricePerDayToHourGeneratorService)
        {
            _priceGenerator = pricePerDayToHourGeneratorService;
        }

        public async Task<bool> SendReturnConfirmedEmailAsync(Rent rent)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress(Environment.GetEnvironmentVariable("COMPANY_EMAIL"), "Car");
            var subject = "Return confirmation";
            var to = new EmailAddress(rent.UserEmail, rent.UserEmail);
            var plainTextContent = "";

            var htmlContent = "<ul>Your car returned, your rent was:" +
                                  $"<li>Your car: {rent.Car.Model.Brand.Name} {rent.Car.Model.Name}</li>" +
                                  $"<li>Price: {_priceGenerator.GeneratePrice(rent.Car.Model.BasePrice, rent.RentEnd.Value - rent.RentStart)}</li>" +
                                  $"<li>Rent starts: {rent.RentStart}</li>" +
                                  $"<li>Rent ends: {rent.RentEnd}</li>" +
                                  $"Thank you for using CarRental</div>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            return response.IsSuccessStatusCode;
        }
    }
}
