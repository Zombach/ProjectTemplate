using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kafka.Common.Configurations;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddKafkaServices(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(IServiceCollection));
        var consumerConfig = configuration.GetSection(nameof(ConsumerConfig)).GetConfig<ConsumerConfig>();
        CheckConsumerConfig(consumerConfig);
        services.AddSingleton(consumerConfig);

        var producerConfig = configuration.GetSection(nameof(ProducerConfig)).GetConfig<ProducerConfig>();
        CheckProducerConfig(producerConfig);
        services.AddSingleton(producerConfig);

        return services;
    }

    private static void CheckConsumerConfig(ConsumerConfig consumerConfig)
    {
        if
        (
            string.IsNullOrEmpty(consumerConfig.BootstrapServers)
            || consumerConfig.AutoOffsetReset is null
            || string.IsNullOrEmpty(consumerConfig.GroupId)
        )
        {
            throw new Exception($"Не корректные данные для секции: {nameof(ConsumerConfig)}");
        }
    }

    private static void CheckProducerConfig(ProducerConfig producerConfig)
    {
        if (string.IsNullOrEmpty(producerConfig.BootstrapServers))
        {
            throw new Exception($"Не корректные данные для секции: {nameof(ProducerConfig)}");
        }
    }
    
    private static T GetConfig<T>(this IConfigurationSection configurationSection) where T : new()
    {
        var config = new T();
        configurationSection.Bind(config);
        ArgumentNullException.ThrowIfNull(config, "Не получить секцию конфигурации");
        return config;
    }
}