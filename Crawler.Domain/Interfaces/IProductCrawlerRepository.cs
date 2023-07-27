using Crawler.Domain.Models;

namespace Crawler.Domain.Interfaces;

public interface IProductCrawlerRepository : IRepository<Product, string>
{
}