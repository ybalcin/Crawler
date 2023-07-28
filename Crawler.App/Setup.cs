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
        services.AddCrawlerSettings(configuration);
        services.RegisterServices();
        services.BuildServiceProvider();
        return services.BuildServiceProvider();
    }
}