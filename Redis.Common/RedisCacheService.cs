using System.Text.Json;
using Microsoft.Extensions.Logging;
using Redis.Common.Extensions;
using Redis.Common.Interfaces;
using Redis.Common.Models;
using StackExchange.Redis;

namespace Redis.Common;

internal class RedisCacheService<T>
(
    IConnectionMultiplexer redisConnection,
    IKeyProvider<T> keyProvider,
    IJsonDeserializer<EntityContainerCached<T>> jsonDeserializer,
    string instanceName,
    ILogger<RedisCacheService<T>> logger
)
    : ICacheService<T>
    where T : class
{
    private readonly IDatabase _redisDb = redisConnection.GetDatabase();

    public async Task<string> AddOrUpdateCachedEntityAsync(T value, TimeSpan? expiry)
    => await AddOrUpdateEntityAsync(value, expiry);

    public async Task AddOrUpdateRangeAsync(IEnumerable<T> entities, TimeSpan? expiry)
    {
        foreach (T entity in entities)
        {
            await AddOrUpdateEntityAsync(entity, expiry);
        }
    }

    public async Task<EntityContainerCached<T>> GetCachedEntityByKeyAsync(string key)
    {
        string? cachedSerialized = await _redisDb.StringGetAsync(key);

        return !string.IsNullOrWhiteSpace(cachedSerialized) ? jsonDeserializer.Deserialize(cachedSerialized) : null;
    }

    public async IAsyncEnumerable<EntityContainerCached<T>> GetAllContainerEntitiesByPartialKey(string partialKey)
    {
        IEnumerable<string> keys = _redisDb.GetAllKeys($"{instanceName}:{partialKey}");

        foreach (string key in keys)
        {
            yield return await GetCachedEntityByKeyAsync(key);
        }
    }

    public async Task DeleteCachedEntityAsync(T value)
    {
        string key = keyProvider.GetKey(value);
        await _redisDb.KeyDeleteAsync(key);
    }

    public async Task DeleteCachedEntityByKeyAsync(string key)
    => await _redisDb.KeyDeleteAsync(key);

    private async Task<(string Key, string Value)> GetKeyValuePairAsync(T value)
    {
        string key = keyProvider.GetKey(value);
        string lastMessageCachedSerialized = await _redisDb.StringGetAsync(key);
        return (key, lastMessageCachedSerialized);
    }

    private static EntityContainerCached<T> ConvertToCachedEntity(T originalMessage)
    {
        return new EntityContainerCached<T>()
        {
            Entity = originalMessage,
            LastUpdatedAtUtc = DateTimeOffset.UtcNow
        };
    }

    private async Task<string> AddOrUpdateEntityAsync(T value, TimeSpan? expiry)
    {
        (string key, string lastMessageCachedSerialized) = await GetKeyValuePairAsync(value);

        if (!string.IsNullOrEmpty(lastMessageCachedSerialized))
        {
            EntityContainerCached<T> messageCached = jsonDeserializer.Deserialize(lastMessageCachedSerialized);

            if (keyProvider.CheckEntityUpdateCondition == null || keyProvider.CheckEntityUpdateCondition(messageCached.Entity, value))
            {
                string updatedTrackMessageCachedSerialized = JsonSerializer.Serialize(ConvertToCachedEntity(value));
                await _redisDb.StringSetAsync(key, updatedTrackMessageCachedSerialized, expiry);
            }
        }
        else
        {
            string newTrackMessageCachedSerialized = JsonSerializer.Serialize(ConvertToCachedEntity(value));
            await _redisDb.StringSetAsync(key, newTrackMessageCachedSerialized, expiry);
        }

        return key;
    }

    public async Task<IEnumerable<T>> GetAllEntitiesByPartialKeyAsync(string partialKey)
    {
        IEnumerable<string> keys = _redisDb.GetAllKeys($"{instanceName}:{partialKey}");

        List<T> entities = new List<T>();

        foreach (string key in keys)
        {
            entities.Add((await GetCachedEntityByKeyAsync(key)).Entity);
        }

        return entities;
    }
}