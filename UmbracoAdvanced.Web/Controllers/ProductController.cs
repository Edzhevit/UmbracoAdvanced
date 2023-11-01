using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using UmbracoAdvanced.Core.Services;
using UmbracoAdvanced.Web.ViewModels;

namespace UmbracoAdvanced.Web.Controllers;

/// <summary>
/// UmbracoPageController can be used without document types in the back office
/// Used for getting objects from DB and show them in the razor views
/// </summary>
public class ProductController : UmbracoPageController, IVirtualPageController
{
    private readonly IProductService _productService;
    private readonly IUmbracoContextAccessor _contextAccessor;
    public ProductController(ILogger<UmbracoPageController> logger, ICompositeViewEngine compositeViewEngine, IProductService productService, IUmbracoContextAccessor contextAccessor) 
        : base(logger, compositeViewEngine)
    {
        _productService = productService;
        _contextAccessor = contextAccessor;
    }

    // attribute routing must inherit IVirtualPageController
    [Route("[controller]/[action]/{id?}")]
    [HttpGet]
    public IActionResult Details(int id)
    {
        var product = _productService.GetAll().FirstOrDefault(x => x.Id == id);
        if (product == null || CurrentPage == null)
        {
            return NotFound();
        }

        var model = new ProductViewModel(CurrentPage)
        {
            ProductName = product.Name
        };
        return View(model);
    }

    public IPublishedContent FindContent(ActionExecutingContext actionExecutingContext)
    {
        var homepage = _contextAccessor.GetRequiredUmbracoContext()?.Content?.GetAtRoot()
            .FirstOrDefault();

        var productListingPage = homepage?.FirstChildOfType("products");

        return productListingPage ?? homepage;
    }
}