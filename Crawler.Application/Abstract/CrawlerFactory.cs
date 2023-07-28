using Crawler.Application.Crawlers.ProductCrawler;
using Microsoft.Extensions.DependencyInjection;

namespace Crawler.Application.Abstract;

public interface ICrawlerFactory
{
    ICrawler Generate(string key);
}

public class CrawlerFactory : ICrawlerFactory
{
    private readonly IServiceProvider _serviceProvider;

    public CrawlerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public ICrawler Generate(string key)
    {
        switch (key)
        {
            case nameof(ProductCrawler):
                var c = _serviceProvider.GetService<IProductCrawler>();
                if (c == null)
                {
                    throw new ArgumentNullException(nameof(ProductCrawler));
                }
                return c;
            
            default:
                throw new NotImplementedException($"{key} is not implemented");
        }
    }
}