using Microsoft.OpenApi.Models;

namespace Api.Configurations;

public static class SwaggerConfig
{
    public static void AddSwaggerConfiguration(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddSwaggerGen(s =>
        {
            s.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Project",
                Description = "API Swagger surface",
                Contact = new OpenApiContact { Name = "ZLoo", Email = "Kozlov.s.v.1992@gmail.com", Url = new Uri("https://github.com/Zombach") },
                License = new OpenApiLicense { Name = "MIT", Url = new Uri("https://github.com/Zombach/Project/blob/master/LICENSE") }
            });
        });
    }

    public static void UseSwaggerSetup(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app, nameof(app));

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        });
    }
}