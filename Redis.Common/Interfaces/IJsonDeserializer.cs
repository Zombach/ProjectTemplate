namespace Redis.Common.Interfaces;

public interface IJsonDeserializer<out T>
{
    T Deserialize(string jsonString);
}