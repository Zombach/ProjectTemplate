using StackExchange.Redis;

namespace Redis.Common.Extensions;

public static class RedisExtensions
{
    public static IEnumerable<string> GetAllKeys(this IDatabase database, string keyPattern)
    {
        IServer server = database.Multiplexer.GetServer(database.IdentifyEndpoint());
        IEnumerable<RedisKey> keys = server.Keys(database.Database, $"{keyPattern}*");

        return keys.Select(k => k.ToString());
    }
}