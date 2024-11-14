    namespace CarRentalAPI.Abstractions
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync();
    }
}
