using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace UmbracoAdvanced.Web.Controllers;

/// <summary>
/// Surface Controllers are mostly used for Forms
/// </summary>
public class ContactController : SurfaceController
{
    public ContactController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
    {
    }

    // /umbraco/surface/Contact/Submit
    public IActionResult Submit()
    {
        if (!ModelState.IsValid)
        {
            return CurrentUmbracoPage();
        }

        TempData["Message"] = "Submitted successfully";
        return RedirectToCurrentUmbracoPage(QueryString.Create("submit", "true"));
    }

    // /umbraco/surface/Contact/TestMethod?id=idvalue
    public IActionResult TestMethod(string id)
    {
        return Ok(id);
    }
}