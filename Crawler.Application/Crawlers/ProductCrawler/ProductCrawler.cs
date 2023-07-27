using Crawler.Application.Abstract;
using Crawler.Application.Extensions;
using Crawler.Domain.Interfaces;
using Crawler.Domain.Models;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace Crawler.Application.Crawlers.ProductCrawler;

public class ProductCrawler : Abstract.Crawler, IProductCrawler
{
    private readonly IProductCrawlerRepository _repository;

    public ProductCrawler(IProductCrawlerRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Start()
    {
        using var client = new HttpClient();
        client.Timeout = TimeSpan.FromSeconds(60);

        var resp = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get,
            "https://crawling-coding-challenge-properti-ag.vercel.app/"));
        if (!resp.IsSuccessStatusCode)
        {
            return false;
        }

        var respBody = await resp.Content.ReadAsStringAsync();

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(respBody);

        var scriptTag =
            htmlDocument.DocumentNode.SelectSingleNode("//script[@id='__NEXT_DATA__' and @type='application/json']");
        var productListing = JsonConvert.DeserializeObject<ProductListingPropsDto>(scriptTag.InnerHtml);
        if (productListing == null)
        {
            return false;
        }

        var tasks = new List<Task<Product?>>();
        foreach (var productDto in productListing.Props.PageProps.Listings)
        {
            tasks.Add(FindAndFillProduct(productDto));
        }

        var products = await Task.WhenAll(tasks);

        var productList = products.Where(product => product != null).Cast<Product>().ToList();

        return await _repository.AddRangeAsync(productList);
    }

    private async Task<Product?> FindAndFillProduct(ProductDto dto)
    {
        using var client = new HttpClient();
        client.Timeout = TimeSpan.FromSeconds(60);

        var url = $"https://crawling-coding-challenge-properti-ag.vercel.app/{dto.Id}";

        var resp = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
        if (!resp.IsSuccessStatusCode)
        {
            return null;
        }

        var respBody = await resp.Content.ReadAsStringAsync();

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(respBody);

        var street = htmlDocument.FindDivInnerHtmlByClass("street");
        var zipCity = htmlDocument.FindDivInnerHtmlByClass("zip-city");
        var company = htmlDocument.FindDivInnerHtmlByClass("company");
        var name = htmlDocument.FindDivInnerHtmlByClass("name");
        var email = htmlDocument.FindDivInnerHtmlByClass("email");
        var phone = htmlDocument.FindDivInnerHtmlByClass("phone");
        
        var p = new Product
        {
            PId = dto.Id,
            Company = company,
            Name = name,
            Price = dto.Price,
            Street = street,
            Title = dto.Title,
            ZipCity = zipCity,
            Email = email,
            Phone = phone
        };

        Console.WriteLine($"Crawled: {p}");

        return p;
    }
}