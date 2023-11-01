using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Web.BackOffice.Controllers;
using UmbracoAdvanced.Core.Models.NPoco;
using UmbracoAdvanced.Core.Services;
using UmbracoAdvanced.Web.ViewModels.Api;

namespace UmbracoAdvanced.Web.Controllers.BackOffice;

public class ContactRequestsApiController : UmbracoAuthorizedApiController
{
    private readonly IContactRequestService _contactRequestService;
    private readonly IUmbracoMapper _umbracoMapper;

    public ContactRequestsApiController(IContactRequestService contactRequestService, IUmbracoMapper umbracoMapper)
    {
        _contactRequestService = contactRequestService;
        _umbracoMapper = umbracoMapper;
    }

    public async Task<IActionResult> GetTotalNumber()
    {
        return Ok(await _contactRequestService.GetTotalNumber());
    }

    public async Task<IActionResult> GetAll()
    {
        var contacts = await _contactRequestService.GetAll();
        return Ok(_umbracoMapper.MapEnumerable<ContactRequest, ContactRequestResponseItem>(contacts));
    }
}