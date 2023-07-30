using Crawler.Domain.Interfaces;

namespace Crawler.Application.Services.Product;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public IEnumerable<Domain.Models.Product> GetAll()
    {
        return _repository.Get().ToList();
    }

    public Task<bool> AddRangeAsync(IEnumerable<Domain.Models.Product> entities, CancellationToken cancellationToken = default)
    {
        return _repository.AddRangeAsync(entities, cancellationToken);
    }
}