namespace Crawler.Application.Crawlers;

public class CrawlerSettings
{
    public CrawlerSettings()
    {
        Settings = new List<CrawlerSetting>();
    }
    
    public List<CrawlerSetting> Settings { get; set; }
}

public class CrawlerSetting
{
    public string URL { get; set; }
    public string Key { get; set; }
}