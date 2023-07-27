using MongoDB.Bson.Serialization.Attributes;

namespace Crawler.Domain.Models;

public class Product : MongoEntityBase
{
    [BsonElement("pid")] public int PId { get; set; }
    [BsonElement("title")] public string Title { get; set; }
    [BsonElement("price")] public string Price { get; set; }
    [BsonElement("street")] public string Street { get; set; }
    [BsonElement("zip_city")] public string ZipCity { get; set; }
    [BsonElement("company")] public string Company { get; set; }
    [BsonElement("name")] public string Name { get; set; }
    [BsonElement("phone")] public string Phone { get; set; }

    [BsonElement("email")] public string Email { get; set; }

    public override string ToString()
    {
        return
            $"PId: {PId}, Title: {Title}, Price: {Price}, Street: {Street}, Zip-City: {ZipCity}, Company: {Company}, Name: {Name}, Phone: {Phone}";
    }
}