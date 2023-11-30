using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Application.Common.Behaviours;
using Domain.Configurations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Infrastructure.Configurations;
using Domain.Commands.Validations;

namespace Application.Configurations;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        services.AddValidatorsFromAssembly(typeof(BaseValidator<>).Assembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        services.AddDomainConfiguration();
        services.AddInfrastructureServices(configuration);

        return services;
    }
}