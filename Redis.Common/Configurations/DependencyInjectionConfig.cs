using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Redis.Common.Interfaces;
using Redis.Common.Models;
using StackExchange.Redis;

namespace Redis.Common.Configurations;

public static class DependencyInjectionConfig
{
    /// <summary>
    /// Добавить конфигурация редиса для сущности с типом Т
    /// </summary>
    /// <typeparam name="T">Тип сущности</typeparam>
    /// <param name="services">Коллекция сервисов</param>
    /// <param name="configuration">Конфигурация</param>
    /// <param name="instanceName">Имя сущности</param>
    /// <returns></returns>

    public static IServiceCollection AddRedisServices<T>(this IServiceCollection services, IConfiguration configuration, string instanceName) where T : class
    {
        ArgumentNullException.ThrowIfNull(services, nameof(IServiceCollection));
        ArgumentNullException.ThrowIfNull(configuration, nameof(IConfiguration));

        RedisOptions redisOptions = configuration.GetSection(RedisOptions.SectionKey).GetConfig<RedisOptions>();
        ConfigurationOptions configurationOptions = new()
        {
            EndPoints = { $"{redisOptions.Host}:{redisOptions.Port}" },
            Password = redisOptions.Password,
            AbortOnConnectFail = false
        };

        ConnectionMultiplexer multiplexer = ConnectionMultiplexer.Connect(configurationOptions);
        services.AddSingleton<IConnectionMultiplexer>(multiplexer);

        services.AddSingleton<ICacheService<T>>(provider =>
        {
            IConnectionMultiplexer connectionMultiplexer = provider.GetRequiredService<IConnectionMultiplexer>();
            IKeyProvider<T> keyProvider = provider.GetRequiredService<IKeyProvider<T>>();
            IJsonDeserializer<EntityContainerCached<T>> jsonDeserializer = provider.GetRequiredService<IJsonDeserializer<EntityContainerCached<T>>>();
            ILogger<RedisCacheService<T>> logger = provider.GetRequiredService<ILogger<RedisCacheService<T>>>();
            string instance = redisOptions.GetInstance(instanceName);

            return new RedisCacheService<T>(connectionMultiplexer, keyProvider, jsonDeserializer, instance, logger);
        });

        return services;
    }

    private static T GetConfig<T>(this IConfigurationSection configurationSection) where T : new()
    {
        var config = new T();
        configurationSection.Bind(config);
        ArgumentNullException.ThrowIfNull(config, "Не получить секцию конфигурации");
        return config;
    }
}