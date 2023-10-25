using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.BackOffice.Controllers;
using UmbracoAdvanced.Core.Models.Umbraco;

namespace UmbracoAdvanced.Web.Controllers.BackOffice;

/// <summary>
/// route of the API
/// /umbraco/backoffice/api/ProductListing/GetProducts?number=X
/// </summary>
public class ProductListingController : UmbracoAuthorizedApiController
{
    private IUmbracoContextFactory _contextFactory;

    public ProductListingController(IUmbracoContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public IActionResult GetProducts(int number)
    {
        var final = new List<ProductResponse>();

        using var cref = _contextFactory.EnsureUmbracoContext();
        var contentCache = cref.UmbracoContext.Content;
        var products = contentCache
            ?.GetAtRoot()
            .FirstOrDefault(x => x.ContentType.Alias == Home.ModelTypeAlias)
            ?.Descendant<Products>()
            ?.Children<Product>()
            ?.Take(number);

        if (products != null && products.Any())
        {
            final = products.Select(x => new ProductResponse(x.Id, x.ProductName ?? x.Name, x.Photos?.Url() ?? "#"))
                .ToList();
        }

        return Ok(final);
    }

    private record ProductResponse(int id, string name, string imageUrl);
}