using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Application.Common.Behaviours;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Infrastructure.Configurations;

namespace Application.Configurations;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        });

        services.AddInfrastructureServices(configuration);

        return services;
    }
}