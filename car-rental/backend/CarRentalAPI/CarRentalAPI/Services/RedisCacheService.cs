using CarRentalAPI.Abstractions;
using StackExchange.Redis;

namespace CarRentalAPI.Services;

public class RedisCacheService : ICacheService
{
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

    public Task<bool> GetSetValueTask(string key, string value, TimeSpan? expiry = null)
    {
        return _database.StringSetAsync(key, value, expiry);
    }

    public async Task<string?> GetValueAsync(string key)
    {
        return await _database.StringGetAsync(key);
    }
    
    public async Task<string?> GetValueAndRefreshExpiryAsync(string key, TimeSpan newExpiry)
    {
        var value = await _database.StringGetAsync(key);

        if (!value.IsNull)
        {
            await _database.KeyExpireAsync(key, newExpiry);
        }
        
        return value;
    }
    
    public async Task<string?> GetValueAndDeleteKeyAsync(string key)
    {
        var tran = _database.CreateTransaction();
        var getResult = tran.StringGetAsync(key);
        var deleteTask = tran.KeyDeleteAsync(key);
        
        await tran.ExecuteAsync();
        
        var result = await getResult;
        return result;
    }

    public async Task<bool> DeleteKeyAsync(string key)
    {
        return await _database.KeyDeleteAsync(key);
    }
}