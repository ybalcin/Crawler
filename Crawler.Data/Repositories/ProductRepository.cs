using Crawler.Data.Configuration;
using Crawler.Domain.Interfaces;
using Crawler.Domain.Models;
using Microsoft.Extensions.Options;

namespace Crawler.Data.Repositories;

public class ProductRepository : MongoRepositoryBase<Product>, IProductRepository
{
    public ProductRepository(IOptions<MongoSettings> options) : base(options)
    {
    }
}