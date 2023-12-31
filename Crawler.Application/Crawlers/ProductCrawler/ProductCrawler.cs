using Crawler.Application.Extensions;
using Crawler.Application.Services.Product;
using Crawler.Domain.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Crawler.Application.Crawlers.ProductCrawler;

public class ProductCrawler : Abstract.Crawler<ProductCrawler>, IProductCrawler
{
    private readonly IProductService _service;

    public ProductCrawler(IOptions<CrawlerSettings> setting,
        ILogger<ProductCrawler> logger, IProductService service) : base(setting, logger)
    {
        _service = service ?? throw new NullReferenceException(nameof(service));
    }

    public async Task<bool> Start(CancellationToken cancellationToken = default, HttpClient? client = default)
    {
        using var cc = client ?? new HttpClient();
        cc.Timeout = TimeSpan.FromSeconds(60);

        HttpResponseMessage resp;
        try
        {
            resp = await cc.SendAsync(new HttpRequestMessage(HttpMethod.Get, Url), cancellationToken);
            if (!resp.IsSuccessStatusCode)
            {
                return false;
            }
        }
        catch (OperationCanceledException)
        {
            Logger.LogError("request is cancelled for {Url}", Url);
            return false;
        }

        if (!resp.IsSuccessStatusCode)
        {
            Logger.LogError("received non-OK HTTP status for request {Url}, status: {resp.StatusCode}", Url,
                resp.StatusCode);
            return false;
        }

        var respBody = await resp.Content.ReadAsStringAsync(cancellationToken);

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(respBody);

        var scriptTag =
            htmlDocument.DocumentNode.SelectSingleNode("//script[@id='__NEXT_DATA__' and @type='application/json']");
        if (scriptTag == null)
        {
            Logger.LogError("//script[@id='__NEXT_DATA__' and @type='application/json'] not found");
            return false;
        }

        var productListing = JsonConvert.DeserializeObject<ProductListingPropsDto>(scriptTag.InnerHtml);
        if (productListing == null)
        {
            Logger.LogError("products not found");
            return false;
        }

        var tasks = new List<Task<Product?>>();
        foreach (var productDto in productListing.Props.PageProps.Listings)
        {
            tasks.Add(FindAndFillProduct(productDto, null, cancellationToken));
        }

        var products = (await Task.WhenAll(tasks)).Where(p => p != null).Cast<Product>().ToList();

        return await _service.AddRangeAsync(products, cancellationToken);
    }

    private async Task<Product?> FindAndFillProduct(ProductDto dto, HttpClient? client = default,
        CancellationToken cancellationToken = default)
    {
        using var cc = client ?? new HttpClient();
        cc.Timeout = TimeSpan.FromSeconds(60);

        var requestUri = $"{Url}/{dto.Id}";

        HttpResponseMessage resp;
        try
        {
            resp = await cc.SendAsync(new HttpRequestMessage(HttpMethod.Get, requestUri), cancellationToken);
        }
        catch (OperationCanceledException)
        {
            Logger.LogError("request is cancelled for {requestUri}", requestUri);
            return null;
        }

        if (!resp.IsSuccessStatusCode)
        {
            Logger.LogError("received non-OK HTTP status for request {requestUri}, status: {resp.StatusCode}",
                requestUri, resp.StatusCode);
            return null;
        }

        var respBody = await resp.Content.ReadAsStringAsync(cancellationToken);

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

        Logger.LogInformation("crawled: {p}", p);

        return p;
    }
}