using Crawler.Application.Crawlers.ProductCrawler;
using Crawler.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Crawler.App;

public static class Setup
{
    public static IServiceProvider Services()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        var services = new ServiceCollection();
        services.AddMongoSettings(configuration);
        services.AddCrawlerSettings(configuration);
        services.RegisterServices();
        services.AddLogging(cfg =>
        {
            cfg.AddConsole();
        }).AddTransient<ProductCrawler>();
        
        services.BuildServiceProvider();
        return services.BuildServiceProvider();
    }
}