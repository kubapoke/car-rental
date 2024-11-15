namespace CarSearchAPI.Abstractions
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(string recipientEmail);
    }
}
