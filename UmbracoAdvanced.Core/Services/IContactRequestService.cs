using UmbracoAdvanced.Core.Models.NPoco;

namespace UmbracoAdvanced.Core.Services;

public interface IContactRequestService
{
    Task<ContactRequestDBModel?> GetById(int id);
    Task<int> GetTotalNumber();
    Task<List<ContactRequestDBModel>> GetAll();
    Task<int> SaveContactRequest(string name, string email, string message);
}