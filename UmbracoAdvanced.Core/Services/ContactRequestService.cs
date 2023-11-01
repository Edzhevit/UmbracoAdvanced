using Umbraco.Cms.Infrastructure.Scoping;
using UmbracoAdvanced.Core.Models.NPoco;

namespace UmbracoAdvanced.Core.Services;

public class ContactRequestService : IContactRequestService
{
    private readonly IScopeProvider _scopeProvider;

    public ContactRequestService(IScopeProvider scopeProvider)
    {
        _scopeProvider = scopeProvider;
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
}