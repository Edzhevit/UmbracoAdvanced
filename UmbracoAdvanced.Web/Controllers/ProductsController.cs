using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace UmbracoAdvanced.Web.Controllers;

/// <summary>
/// Render Controllers are mostly used for highjacking the route from umbraco and override the result
/// </summary>
public class ProductsController : RenderController
{
    public ProductsController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor) 
        : base(logger, compositeViewEngine, umbracoContextAccessor) { }

    // /products
    public override IActionResult Index()
    {
        var currentPage = CurrentPage;
        return CurrentTemplate(currentPage);
    }
}