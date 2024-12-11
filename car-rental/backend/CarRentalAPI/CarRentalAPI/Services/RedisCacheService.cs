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

    public async Task<bool> DeleteKeyAsync(string key)
    {
        return await _database.KeyDeleteAsync(key);
    }
}