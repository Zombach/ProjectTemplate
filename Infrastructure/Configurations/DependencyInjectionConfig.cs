using Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configurations;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(IServiceCollection));
        ArgumentNullException.ThrowIfNull(configuration, nameof(IConfiguration));

        var dataBaseConfig = configuration.GetSection(nameof(DatabaseConfig)).GetConfig<DatabaseConfig>();
        ArgumentNullException.ThrowIfNull(dataBaseConfig, nameof(DatabaseConfig));

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(dataBaseConfig.DefaultConnection);
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