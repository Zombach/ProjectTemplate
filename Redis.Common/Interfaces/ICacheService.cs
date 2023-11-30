using Redis.Common.Models;

namespace Redis.Common.Interfaces;

public interface ICacheService<T> where T : class
{
    Task<string> AddOrUpdateCachedEntityAsync(T entity, TimeSpan? expiry = null);

    Task AddOrUpdateRangeAsync(IEnumerable<T> entities, TimeSpan? expiry = null);

    Task<EntityContainerCached<T>> GetCachedEntityByKeyAsync(string key);

    IAsyncEnumerable<EntityContainerCached<T>> GetAllContainerEntitiesByPartialKey(string partialKey);

    Task<IEnumerable<T>> GetAllEntitiesByPartialKeyAsync(string partialKey);

    Task DeleteCachedEntityAsync(T value);

    Task DeleteCachedEntityByKeyAsync(string key);
}
