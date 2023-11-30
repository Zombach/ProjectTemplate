using Application.Common.Behaviours;
using Domain.Commands.Validations;
using Domain.Configurations;
using FluentValidation;
using Infrastructure.Configurations;
using Kafka.Common.Configurations;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Redis.Common.Configurations;

namespace Application.Configurations;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(IServiceCollection));
        ArgumentNullException.ThrowIfNull(configuration, nameof(IConfiguration));

        services.AddValidatorsFromAssembly(typeof(BaseValidator<>).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        services.AddDomainServices();
        services.AddInfrastructureServices(configuration);
        services.AddRedisServices<RedisOptions>(configuration, "test");
        services.AddKafkaServices(configuration);

        return services;
    }
}