using Microsoft.Extensions.DependencyInjection;

namespace Domain.Configurations;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddDomainConfiguration(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        return services;
    }
}