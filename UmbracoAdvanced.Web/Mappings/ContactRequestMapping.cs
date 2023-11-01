using Umbraco.Cms.Core.Mapping;
using UmbracoAdvanced.Core.Models.NPoco;
using UmbracoAdvanced.Web.ViewModels.Api;

namespace UmbracoAdvanced.Web.Mappings;

public class ContactRequestMapping : IMapDefinition
{
    public void DefineMaps(IUmbracoMapper mapper)
    {
        mapper.Define<ContactRequest, ContactRequestResponseItem>((source, context) => new ContactRequestResponseItem(), Map);
    }

    private void Map(ContactRequest source, ContactRequestResponseItem target, MapperContext mapperContext)
    {
        target.Id = source.Id;
        target.Name = source.Name;
        target.Email = source.Email;
        target.Message = source.Message;
    }
}