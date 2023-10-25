using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.Common.Controllers;

namespace UmbracoAdvanced.Web.Controllers;


/// <summary>
/// UmbracoAuthorizedController is used to authorize the back office users
/// UmbracoAuthorizedApiController is used to authorize the back office users
/// UmbracoAuthorizedJsonController is used to authorize the back office users
/// </summary>
public class AuthorizedController : UmbracoAuthorizedController
{
    public IActionResult Index()
    {
        return Ok();
    }
}