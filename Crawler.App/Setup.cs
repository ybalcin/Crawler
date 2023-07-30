using Crawler.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        services.RegisterCommonServices();
        services.AddCrawlerSettings(configuration);
        services.RegisterCrawlerServices();
        services.RegisterCrawlerLogger();
        services.BuildServiceProvider();
        
        return services.BuildServiceProvider();
    }
}