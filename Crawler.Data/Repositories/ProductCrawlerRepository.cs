using Crawler.Data.Configuration;
using Crawler.Domain.Interfaces;
using Crawler.Domain.Models;
using Microsoft.Extensions.Options;

namespace Crawler.Data.Repositories;

public class ProductCrawlerRepository : MongoRepositoryBase<Product>, IProductCrawlerRepository
{
    public ProductCrawlerRepository(IOptions<MongoSettings> options) : base(options)
    {
    }
}