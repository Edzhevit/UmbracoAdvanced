using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Web.Common.Controllers;
using UmbracoAdvanced.Core.Models;
using UmbracoAdvanced.Core.Models.Umbraco;
using UmbracoAdvanced.Core.Repository;
using UmbracoAdvanced.Web.ViewModels.Api;

namespace UmbracoAdvanced.Web.Controllers;

// If route is not specified the auto route is -> /umbraco/api/productapi/{action}

// /api/products
[Route("api/products")]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
public class ProductApiController : UmbracoApiController
{
    private readonly IProductRepository _productRepository;
    private readonly IUmbracoMapper _umbracoMapper;

    public ProductApiController(IProductRepository productRepository, IUmbracoMapper umbracoMapper)
    {
        _productRepository = productRepository;
        _umbracoMapper = umbracoMapper;
    }

    public record ProductReadRequest(string? productSKU, decimal? maxPrice);

    [HttpGet]
    public IActionResult Read([FromQuery] ProductReadRequest request)
    {
        var mapped = _umbracoMapper
            .MapEnumerable<Product, ProductApiResponseItem>(_productRepository.GetProducts(request.productSKU, request.maxPrice));
        return Ok(mapped);
    }

    [HttpPost]
    public IActionResult Create([FromBody]ProductCreationItem request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Fields error");
        }

        var product = _productRepository.Create(request);

        return Ok(_umbracoMapper.Map<Product, ProductApiResponseItem>(product));
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody]ProductUpdateItem request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (_productRepository.Get(id) == null)
        {
            return NotFound();
        }

        var product = _productRepository.Update(id, request);
        return Ok(_umbracoMapper.Map<Product, ProductApiResponseItem>(product));
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        if (_productRepository.Get(id) == null)
        {
            return NotFound();
        }
        var result = _productRepository.Delete(id);
        return result ? Ok() : NotFound();
    }
}