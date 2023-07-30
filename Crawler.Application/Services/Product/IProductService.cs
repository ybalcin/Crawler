namespace Crawler.Application.Services.Product;

public interface IProductService
{
    IEnumerable<Domain.Models.Product> GetAll();
    Task<bool> AddRangeAsync(IEnumerable<Domain.Models.Product> entities, CancellationToken cancellationToken);
}