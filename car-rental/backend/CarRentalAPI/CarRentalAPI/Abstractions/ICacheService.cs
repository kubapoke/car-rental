namespace CarRentalAPI.Abstractions;

public interface ICacheService
{
    public Task SetValueAsync(string key, string value, TimeSpan? expiry = null);
    public Task<bool> GetSetValueTask(string key, string value, TimeSpan? expiry = null);
    public Task<string?> GetValueAsync(string key);
    public Task<string?> GetValueAndRefreshExpiryAsync(string key, TimeSpan newExpiry);
    public Task<string?> GetValueAndDeleteKeyAsync(string key);
    public Task<bool> DeleteKeyAsync(string key);
}