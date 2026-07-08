namespace TaskManagement.Infrastructure.Services;

using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Infrastructure.Configuration;

public class RedisCacheService : ICacheService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly RedisSettings _settings;

    public RedisCacheService(IConnectionMultiplexer redis, IOptions<RedisSettings> options)
    {
        _redis = redis;
        _settings = options.Value;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var db = _redis.GetDatabase();
        var cachedValue = await db.StringGetAsync(key);

        if (!cachedValue.HasValue)
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(cachedValue.ToString());
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expirationTime = null)
    {
        var db = _redis.GetDatabase();
        var serializedValue = JsonSerializer.Serialize(value);
        
        var expiry = expirationTime ?? TimeSpan.FromMinutes(_settings.DefaultExpirationTimeInMinutes);
        
        await db.StringSetAsync(key, serializedValue, expiry);
    }

    public async Task RemoveAsync(string key)
    {
        var db = _redis.GetDatabase();
        await db.KeyDeleteAsync(key);
    }
}
