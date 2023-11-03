using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Cms.Web.Common;
using Umbraco.Extensions;
using UmbracoAdvanced.Core.Models.NPoco;

namespace UmbracoAdvanced.Core.Services;

public class ContactRequestService : IContactRequestService
{
    private readonly IScopeProvider _scopeProvider;
    private readonly UmbracoHelper _helper;
    private readonly IContentService _contentService;

    public ContactRequestService(IScopeProvider scopeProvider, UmbracoHelper helper, IContentService contentService)
    {
        _scopeProvider = scopeProvider;
        _helper = helper;
        _contentService = contentService;
    }

    public async Task<ContactRequest?> GetById(int id)
    {
        using var scope = _scopeProvider.CreateScope();
        return await scope.Database.FirstOrDefaultAsync<ContactRequest>("SELECT * FROM ContactRequest WHERE ID=@0", id);
    }

    public async Task<int> GetTotalNumber()
    {
        using var scope = _scopeProvider.CreateScope(autoComplete:true);
        return await scope.Database.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM ContactRequest");
    }

    public async Task<List<ContactRequest>> GetAll()
    {
        using var scope = _scopeProvider.CreateScope(autoComplete: true);
        return await scope.Database.FetchAsync<ContactRequest>("SELECT * FROM ContactRequest");
    }

    public async Task<int> SaveContactRequest(string name, string email, string message)
    {
        var contactRequest = new ContactRequest { Name = name, Email = email, Message = message };

        using var scope = _scopeProvider.CreateScope();
        await scope.Database.InsertAsync(contactRequest);
        scope.Complete();
        return contactRequest.Id;
    }

    public void SaveAndPublishContactRequest(string name, string email, string message)
    {
        var contactForms = _helper.ContentAtRoot().DescendantsOrSelfOfType("contactForms").FirstOrDefault();
        if (contactForms != null)
        {
            var newContactForm = _contentService.Create("Contact", contactForms.Id, "contactForm");
            newContactForm.SetValue("contactName", name);
            newContactForm.SetValue("contactEmail", email);
            newContactForm.SetValue("contactComment", message);
            _contentService.SaveAndPublish(newContactForm);
        }
    }
}