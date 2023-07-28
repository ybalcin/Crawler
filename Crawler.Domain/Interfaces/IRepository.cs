using System.Linq.Expressions;
using Crawler.Domain.Models;

namespace Crawler.Domain.Interfaces;

public interface IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>, new() where TKey : IEquatable<TKey>
{
    Task<bool> AddAsync(TEntity entity, CancellationToken cancellationToken);
    Task<bool> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
    IQueryable<TEntity> Get(Expression<Func<TEntity, bool>>? predicate = null);
}