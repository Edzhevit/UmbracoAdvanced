using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using UmbracoAdvanced.Core.Models.Umbraco;
using UmbracoAdvanced.Web.ViewModels.Umbraco;

namespace UmbracoAdvanced.Web.Controllers;

/// <summary>
/// Render Controllers are mostly used for highjacking the route from umbraco and override the result
/// </summary>
public class ProductsController : RenderController
{
    private IPublishedValueFallback _publishedValueFallback;
    public ProductsController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor, IPublishedValueFallback publishedValueFallback) 
        : base(logger, compositeViewEngine, umbracoContextAccessor)
    {
        _publishedValueFallback = publishedValueFallback;
    }

    [HttpGet]
    // /products
    public IActionResult Index([FromQuery(Name = "maxprice")] decimal? maxPrice)
    {
        var products = (Products)CurrentPage;
        var allProducts = products.Children<Product>();

        if (maxPrice != null)
        {
            allProducts = allProducts.Where(x => x.Price <= maxPrice);
        }

        var model = new ProductListingViewModel(CurrentPage, _publishedValueFallback)
        {
            Products = allProducts.ToList()
        };
        return CurrentTemplate(model);
    }
}