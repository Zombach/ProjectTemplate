namespace Redis.Common.Configurations;

public class RedisOptions
{
    public const string SectionKey = "RedisOptions";

    public string Host { get; set; }

    public string Port { get; set; }

    public string Password { get; set; }

    public string InstancePrefix { get; set; }

    public Dictionary<string, string> Instances { get; set; }

    public string GetInstance(string instanceName)
    {
        string? instance = Instances.GetValueOrDefault(instanceName);
        if (string.IsNullOrEmpty(instance))
        {
            throw new ArgumentNullException(instanceName);
        }

        return string.IsNullOrEmpty(InstancePrefix) ? instance : $"{InstancePrefix}:{instance}";
    }
}
