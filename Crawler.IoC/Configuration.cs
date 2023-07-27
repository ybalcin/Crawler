using Crawler.Application.Abstract;
using Crawler.Application.Crawlers.ProductCrawler;
using Crawler.Data.Configuration;
using Crawler.Data.Repositories;
using Crawler.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crawler.IoC;

public static class Configuration
{
    public static IServiceCollection AddMongoSettings(this IServiceCollection services, IConfiguration configuration)
    {
        return services.Configure<MongoSettings>(opts =>
        {
            opts.ConnectionUri = configuration
                .GetSection(nameof(MongoSettings) + ":" + MongoSettings.ConnectionUriValue).Value;
            opts.Database = configuration
                .GetSection(nameof(MongoSettings) + ":" + MongoSettings.DatabaseValue).Value;
        });
    }

    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        // register services

        services.AddScoped<IProductCrawlerRepository, ProductCrawlerRepository>();
        services.AddScoped<IProductCrawler, ProductCrawler>();
        
        return services;
    }
}