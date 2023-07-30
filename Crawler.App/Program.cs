using Crawler.App;
using Crawler.Application.Abstract;
using Crawler.Application.Crawlers;
using Crawler.Application.Crawlers.ProductCrawler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var serviceProvider = Setup.Services();

var crawlerFactory = serviceProvider.GetService<ICrawlerFactory>();
if (crawlerFactory == null)
{
    throw new ArgumentNullException(nameof(crawlerFactory));
}

var cancellationTokenSource = new CancellationTokenSource();
Console.CancelKeyPress += (sender, eventArgs) =>
{
    Console.WriteLine("stopping...");
    cancellationTokenSource.Cancel();
    eventArgs.Cancel = true;
};

var crawlerSettings = serviceProvider.GetService<IOptions<CrawlerSettings>>();
if (crawlerSettings == null)
{
    throw new ArgumentNullException(nameof(crawlerSettings));
}
var crawlers = crawlerSettings.Value.Settings.Select(s => s.Key).ToList();

try
{
    var tasks = crawlers.Select(crawlerName => RunCrawlerAsync(crawlerName, cancellationTokenSource.Token));
    await Task.WhenAll(tasks);
}
catch (Exception e)
{
    Console.WriteLine(e);
}

async Task RunCrawlerAsync(string crawlerName, CancellationToken cancellationToken)
{
    var crawler = crawlerFactory.Generate(crawlerName);

    while (!cancellationToken.IsCancellationRequested)
    {
        await crawler.Start(cancellationToken);
    }
}