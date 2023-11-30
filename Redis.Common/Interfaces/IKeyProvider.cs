namespace Redis.Common.Interfaces;

public interface IKeyProvider<in T> where T : class
{
    public string GetKey(T message);

    public string GetKey(string partialKey);

    public bool CheckEntityUpdateCondition(T cachedEntity, T newMessage);
}
