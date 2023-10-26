using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.BackOffice.Controllers;
using UmbracoAdvanced.Core;

namespace UmbracoAdvanced.Web.Controllers.BackOffice;

/// <summary>
/// route of the API
/// /umbraco/backoffice/api/ProductListing/GetProducts?number=X
/// </summary>
public class ProductListingController : UmbracoAuthorizedApiController
{
    private IProductService _productService;

    public ProductListingController(IProductService productService)
    {
        _productService = productService;
    }

    public IActionResult GetProducts(int number)
    {
        return Ok(_productService.GetUmbracoProducts(number));
    }
}