using Crawler.Application.Crawlers;
using Microsoft.Extensions.Options;

namespace Crawler.Application.Abstract;

public abstract class Crawler
{
    protected Crawler(IOptions<CrawlerSettings> options)
    {
        var setting = options.Value.Settings.FirstOrDefault(f => f.Key == GetType().Name);
        if (setting == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        Url = setting.URL;
    }
    
    protected string Url { get; }
}