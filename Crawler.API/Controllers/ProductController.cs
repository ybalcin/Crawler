using Crawler.Application.Services.Product;
using Microsoft.AspNetCore.Mvc;

namespace Crawler.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public IActionResult GetAllProducts()
    {
        var products = _productService.GetAll();
        
        return Ok(products);
    }
}