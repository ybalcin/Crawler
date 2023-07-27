using System.Linq.Expressions;
using Crawler.Data.Configuration;
using Crawler.Domain.Interfaces;
using Crawler.Domain.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Crawler.Data.Repositories;

public abstract class MongoRepositoryBase<TEntity> : IRepository<TEntity, string> where TEntity : MongoEntityBase, new()
{
    protected readonly IMongoCollection<TEntity> Collection;

    protected MongoRepositoryBase(IOptions<MongoSettings> options)
    {
        var mongoSettings = options.Value;
        var settings = MongoClientSettings.FromConnectionString(mongoSettings.ConnectionUri);
        settings.ServerApi = new ServerApi(ServerApiVersion.V1);
        var client = new MongoClient(settings);
        var db = client.GetDatabase(mongoSettings.Database);
        Collection = db.GetCollection<TEntity>(typeof(TEntity).Name.ToLowerInvariant());
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        var options = new InsertOneOptions {BypassDocumentValidation = false};
        await Collection.InsertOneAsync(entity, options);
        return entity;
    }

    public virtual async Task<bool> AddRangeAsync(IEnumerable<TEntity> entities)
    {
        var entityList = entities.ToList();
        var writeList = entityList.Select(entity => new InsertOneModel<TEntity>(entity)).Cast<WriteModel<TEntity>>().ToList();

        var options = new BulkWriteOptions {IsOrdered = false, BypassDocumentValidation = false};
        return (await Collection.BulkWriteAsync(writeList, options)).IsAcknowledged;
    }

    public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>>? predicate = null)
    {
        return predicate == null 
            ? Collection.AsQueryable()
            : Collection.AsQueryable().Where(predicate);
    }
}