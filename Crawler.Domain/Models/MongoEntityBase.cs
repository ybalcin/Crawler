using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Crawler.Domain.Models;

public abstract class MongoEntityBase : IEntity<string>
{
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonId]
    [BsonElement(Order = 0)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
}