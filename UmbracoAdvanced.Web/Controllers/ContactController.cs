using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;
using Umbraco.SampleSite;
using UmbracoAdvanced.Core.Services;

namespace UmbracoAdvanced.Web.Controllers;

/// <summary>
/// Surface Controllers are mostly used for Forms
/// </summary>
public class ContactController : SurfaceController
{
    private readonly IContactRequestService _contactRequestService;
    public ContactController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider, IContactRequestService contactRequestService) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
    {
        _contactRequestService = contactRequestService;
    }

    public async Task<IActionResult> GetContact(int id)
    {
        var contact = await _contactRequestService.GetById(id);
        if (contact == null)
        {
            return NotFound(new {error = "Contact not found"});
        }
        return Ok(contact);
    }

    // /umbraco/surface/Contact/Submit
    public async Task<IActionResult> Submit(ContactFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return CurrentUmbracoPage();
        }

        await _contactRequestService.SaveContactRequest(model.Name, model.Email, model.Message);

        TempData["Message"] = "Submitted successfully";
        return RedirectToCurrentUmbracoPage(QueryString.Create("submit", "true"));
    }

    // /umbraco/surface/Contact/TestMethod?id=idvalue
    public IActionResult TestMethod(string id)
    {
        return Ok(id);
    }
}