using System.Diagnostics.CodeAnalysis;
using System.Net;
using Crawler.Application.Crawlers;
using Crawler.Application.Crawlers.ProductCrawler;
using Crawler.Application.Services.Product;
using Crawler.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace Crawler.UnitTest;

[ExcludeFromCodeCoverage]
public class ProductCrawlerTests
{
    [Fact]
    public async Task Start_ShouldAddProducts_WhenDataIsAvailable()
    {
        var mockService = new Mock<IProductService>();
        var mockLogger = new Mock<ILogger<ProductCrawler>>();
        var mockSettings = Options.Create(new CrawlerSettings
            { Settings = new List<CrawlerSetting> { new() { URL = "https://crawling-coding-challenge-properti-ag.vercel.app/", Key = nameof(ProductCrawler)} } });

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("<!DOCTYPE html><html lang=\"en\"><head>	<meta charSet=\"utf-8\" />	<meta name=\"viewport\" content=\"width=device-width\" />	<meta name=\"next-head-count\" content=\"2\" />	<link rel=\"preload\" href=\"/_next/static/css/ec8dcae3e44a8a56.css\" as=\"style\" />	<link rel=\"stylesheet\" href=\"/_next/static/css/ec8dcae3e44a8a56.css\" data-n-g=\"\" /><noscript		data-n-css=\"\"></noscript>	<script defer=\"\" nomodule=\"\" src=\"/_next/static/chunks/polyfills-c67a75d1b6f99dc8.js\"></script>	<script src=\"/_next/static/chunks/webpack-87b3a303122f2f0d.js\" defer=\"\"></script>	<script src=\"/_next/static/chunks/framework-2c79e2a64abdb08b.js\" defer=\"\"></script>	<script src=\"/_next/static/chunks/main-f11614d8aa7ee555.js\" defer=\"\"></script>	<script src=\"/_next/static/chunks/pages/_app-d0d83664b124ae49.js\" defer=\"\"></script>	<script src=\"/_next/static/chunks/456-c4980c26ae1c292c.js\" defer=\"\"></script>	<script src=\"/_next/static/chunks/90-ec535ffcbe4912a6.js\" defer=\"\"></script>	<script src=\"/_next/static/chunks/pages/index-5a4a185b71ef7c8c.js\" defer=\"\"></script>	<script src=\"/_next/static/AKAcdrXEusJfrMV3jPwwM/_buildManifest.js\" defer=\"\"></script>	<script src=\"/_next/static/AKAcdrXEusJfrMV3jPwwM/_ssgManifest.js\" defer=\"\"></script></head><body>	<div id=\"__next\">		<div style=\"height:400px;width:100%\"></div>	</div>	<script id=\"__NEXT_DATA__\" type=\"application/json\">		{\"props\":{\"pageProps\":{\"listings\":[{\"id\":5635814,\"title\":\"Refined Frozen Towels\",\"price\":\"1807868.00\"}]},\"__N_SSG\":true},\"page\":\"/\",\"query\":{},\"buildId\":\"AKAcdrXEusJfrMV3jPwwM\",\"isFallback\":false,\"gsp\":true,\"scriptLoader\":[]}	</script></body></html>")
            });
        var client = new HttpClient(mockHttpMessageHandler.Object);

        var productCrawler = new ProductCrawler(mockSettings, mockLogger.Object, mockService.Object);

        await productCrawler.Start(cancellationToken: CancellationToken.None, client);

        mockService.Verify(service => service.AddRangeAsync(It.IsAny<IEnumerable<Product>>(), CancellationToken.None),
            Times.Once());
    }
}