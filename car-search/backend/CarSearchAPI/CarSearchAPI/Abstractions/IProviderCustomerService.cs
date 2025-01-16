namespace CarSearchAPI.Abstractions
{
    public interface IProviderCustomerService : IProviderService
    {
        public Task<bool> CheckIfClientExists(HttpClient client, string url, string customerEmail);
        public Task CreateCustomerAsync(HttpClient client, string url);
    }
}
