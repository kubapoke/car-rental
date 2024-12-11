using StackExchange.Redis;

namespace CarRentalAPI.Services;

public class RedisCacheService
{
    // a service providing all of the necessary basic Redis operations
    
    private readonly ConnectionMultiplexer _connectionMultiplexer;
    private readonly IDatabase _database;

    public RedisCacheService(string redisConnectionString)
    {
        _connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
        _database = _connectionMultiplexer.GetDatabase();
    }

    public async Task SetValueAsync(string key, string value, TimeSpan? expiry = null)
    {
        await _database.StringSetAsync(key, value, expiry);
    }

    public async Task<string?> GetValueAsync(string key)
    {
        return await _database.StringGetAsync(key);
    }
    
    // same as the above method, but also refreshes the objects' expiry date
    public async Task<string?> GetValueAndRefreshExpiryAsync(string key, TimeSpan newExpiry)
    {
        var value = await _database.StringGetAsync(key);

        if (!value.IsNull)
        {
            await _database.KeyExpireAsync(key, newExpiry);
        }
        
        return value;
    }

    public async Task<bool> DeleteKeyAsync(string key)
    {
        return await _database.KeyDeleteAsync(key);
    }
}