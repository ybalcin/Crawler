namespace Crawler.Application.Abstract;

public interface ICrawler
{
    Task<bool> Start();
}