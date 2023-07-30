using Crawler.Application.Abstract;
using Crawler.Application.Crawlers;
using Crawler.Application.Crawlers.ProductCrawler;
using Crawler.Application.Services.Product;
using Crawler.Data.Configuration;
using Crawler.Data.Repositories;
using Crawler.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

    public static IServiceCollection AddCrawlerSettings(this IServiceCollection services, IConfiguration configuration)
    {
        return services.Configure<CrawlerSettings>(opts =>
        {
            var value = configuration.GetSection(nameof(CrawlerSettings)).Get<List<CrawlerSetting>>() ??
                        throw new NotImplementedException();
            opts.Settings = value;
        });
    }
    
    public static IServiceCollection RegisterCommonServices(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductService, ProductService>();

        return services;
    }

    public static IServiceCollection RegisterCrawlerServices(this IServiceCollection services)
    {
        services.AddScoped<ICrawlerFactory, CrawlerFactory>();
        services.AddScoped<IProductCrawler, ProductCrawler>();

        return services;
    }

    public static IServiceCollection RegisterCrawlerLogger(this IServiceCollection services)
    {
        services.AddLogging(cfg =>
        {
            cfg.AddConsole();
        }).AddTransient<ProductCrawler>();
        
        return services;
    }
}