namespace Crawler.Data.Configuration;

public class MongoSettings
{
    public string? ConnectionUri { get; set; }
    public string? Database { get; set; }
    
    public const string ConnectionUriValue = nameof(ConnectionUri);
    public const string DatabaseValue = nameof(Database);
}