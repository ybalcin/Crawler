using HtmlAgilityPack;

namespace Crawler.Application.Extensions;

public static class HtmlUtility
{
    public static string FindDivInnerHtmlByClass(this HtmlDocument htmlDocument, string @class)
    {
        var val = htmlDocument.DocumentNode.SelectSingleNode($"//div[@class='{@class}']")?.InnerHtml;
        return !string.IsNullOrEmpty(val) ? val.Replace("<!-- -->", "") : "";
    }
}