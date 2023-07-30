using Crawler.Domain.Models;

namespace Crawler.Domain.Interfaces;

public interface IProductRepository : IRepository<Product, string>
{
}