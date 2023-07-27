namespace Crawler.Application.Crawlers.ProductCrawler;

public class ProductListingPropsDto
{
    public ProductListingPropsDto()
    {
        Props = new PropsDto();
    }
    
    public PropsDto Props { get; set; }
}

public class PropsDto
{
    public PropsDto()
    {
        PageProps = new PagePropsDto();
    }
    
    public PagePropsDto PageProps { get; set; }
}

public class PagePropsDto {
    public PagePropsDto()
    {
        Listings = new List<ProductDto>();
    }
    
    public ICollection<ProductDto> Listings { get; set; }
}

public class ProductDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Price { get; set; }
}