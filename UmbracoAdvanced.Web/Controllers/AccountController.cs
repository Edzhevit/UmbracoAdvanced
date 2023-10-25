using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.Filters;
using Umbraco.Cms.Web.Website.Controllers;

namespace UmbracoAdvanced.Web.Controllers;
/// <summary>
/// UmbracoMemberAuthorize filter is used for authenticate and authorize
/// the members of the front end
/// </summary>
[UmbracoMemberAuthorize]
public class AccountController : SurfaceController
{
    public AccountController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider)
        : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider) { }

    [HttpGet]
    public IActionResult GetUserDetails(int userId)
    {
        return Ok();
    }
}