namespace CarSearchAPI.Abstractions
{
    public interface IProviderCustomerService : IProviderService
    {
        public Task<bool> CheckIfCustomerExistsAsync(HttpClient client, string url, string? customerId);
        public Task CreateCustomerAsync(HttpClient client, string url, string? customerId, string? email);
        public Task<string> GetCustomerIdAsync(HttpClient client, string url, string? email);
    }
}
