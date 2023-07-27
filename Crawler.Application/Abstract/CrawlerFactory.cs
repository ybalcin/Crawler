using Crawler.Application.Crawlers.ProductCrawler;
using Crawler.Domain.Interfaces;

namespace Crawler.Application.Abstract;

public interface ICrawlerFactory
{
    Crawler Generate(string key);
}

public class CrawlerFactory : ICrawlerFactory
{
    private readonly IProductCrawlerRepository _productRepository;

    public CrawlerFactory(IProductCrawlerRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Crawler Generate(string key)
    {
        return new ProductCrawler(_productRepository);
    }
}