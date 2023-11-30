namespace Redis.Common.Models;

public class EntityContainerCached<T> where T : class
{
    public T Entity { get; set; }

    public DateTimeOffset LastUpdatedAtUtc { get; set; }
}
