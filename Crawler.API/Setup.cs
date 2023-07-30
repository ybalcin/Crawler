using Crawler.IoC;
using Microsoft.OpenApi.Models;

namespace Crawler.API;

public static class Setup
{
    public static void RegisterServices(IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        services.AddMongoSettings(configuration);
        services.RegisterCommonServices();
        
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product API", Version = "v1" });
        });
    }

    public static void UseSwagger( WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger");
        });
    }
}