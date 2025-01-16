namespace CarSearchAPI.Abstractions
{
    public interface IProviderCustomerService : IProviderService
    {
        public Task<bool> CheckIfCustomerExistsAsync(HttpClient client, string url, string? email);
        public Task CreateCustomerAsync(HttpClient client, string url, string? email);
    }
}
