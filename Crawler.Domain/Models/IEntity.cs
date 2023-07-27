namespace Crawler.Domain.Models;

public interface IEntity
{
}

public interface IEntity<TKey> : IEntity where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; }
}