namespace CarSearchAPI.Abstractions
{
    public interface IProviderCustomerService : IProviderService
    {
        public Task<bool> CheckIfClientExists(HttpClient client, string url);
        public Task CreateCustomerAsync(HttpClient client, string url, StringContent? jsonContent);
    }
}
