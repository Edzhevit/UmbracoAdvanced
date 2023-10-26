using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Web.Common.Controllers;
using UmbracoAdvanced.Core.Models.Umbraco;
using UmbracoAdvanced.Core.Repository;
using UmbracoAdvanced.Web.ViewModels;

namespace UmbracoAdvanced.Web.Controllers;

// /umbraco/api/productapi/{action}
public class ProductApiController : UmbracoApiController
{
    private readonly IProductRepository _productRepository;
    private readonly IUmbracoMapper _umbracoMapper;

    public ProductApiController(IProductRepository productRepository, IUmbracoMapper umbracoMapper)
    {
        _productRepository = productRepository;
        _umbracoMapper = umbracoMapper;
    }

    // /api/products
    [HttpGet("api/products")]
    public IActionResult Read()
    {
        var mapped = _umbracoMapper
            .MapEnumerable<Product, ProductApiResponseItem>(_productRepository.GetProducts());
        return Ok(mapped);
    }

    // /api/products
    [HttpPost("api/products")]
    public IActionResult Create()
    {
        return Ok("Create");
    }

    // /api/products
    [HttpPut("api/products")]
    public IActionResult Update()
    {
        return Ok("Update");
    }

    // /api/products
    [HttpDelete("api/products")]
    public IActionResult Delete()
    {
        return Ok("Delete");
    }
}