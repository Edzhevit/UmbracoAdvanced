using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.BackOffice.Controllers;

namespace UmbracoAdvanced.Web.Controllers;


/// <summary>
/// UmbracoAuthorizedController is used to authorize the back office users
/// UmbracoAuthorizedApiController is used to authorize the back office users
/// UmbracoAuthorizedJsonController is used to authorize the back office users
/// </summary>
public class AuthorizedController : UmbracoAuthorizedApiController
{
    public IActionResult Index()
    {
        return Ok();
    }
}