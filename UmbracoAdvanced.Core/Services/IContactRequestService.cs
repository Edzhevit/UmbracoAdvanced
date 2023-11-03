using UmbracoAdvanced.Core.Models.NPoco;

namespace UmbracoAdvanced.Core.Services;

public interface IContactRequestService
{
    Task<ContactRequest?> GetById(int id);
    Task<int> GetTotalNumber();
    Task<List<ContactRequest>> GetAll();
    Task<int> SaveContactRequest(string name, string email, string message);
    void SaveAndPublishContactRequest(string name, string email, string message);
}