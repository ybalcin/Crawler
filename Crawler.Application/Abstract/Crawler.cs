using Crawler.Application.Crawlers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Crawler.Application.Abstract;

public abstract class Crawler<T> where T : class
{
    protected readonly ILogger<T> Logger;

    protected Crawler(IOptions<CrawlerSettings> options, ILogger<T> logger)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        var setting = options.Value.Settings.FirstOrDefault(f => f.Key == GetType().Name) ??
                      throw new ArgumentNullException(nameof(options));
        Url = setting.URL;
    }

    protected string Url { get; }
}