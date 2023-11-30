using Microsoft.Extensions.DependencyInjection;

namespace Domain.Configurations;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(IServiceCollection));

        return services;
    }
}