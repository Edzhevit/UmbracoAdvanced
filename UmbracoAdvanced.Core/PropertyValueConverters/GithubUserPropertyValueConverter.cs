using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using UmbracoAdvanced.Core.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace UmbracoAdvanced.Core.PropertyValueConverters;

public class GithubUserPropertyValueConverter : PropertyValueConverterBase
{
    public override bool IsConverter(IPublishedPropertyType propertyType)
    {
        return propertyType.EditorAlias.Equals("githubUser");
    }

    public override object ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType,
        PropertyCacheLevel referenceCacheLevel, object inter, bool preview)
    {
        if (inter == null)
            return null;

        return JsonSerializer.Deserialize<GithubUserDTO>((string)inter);
    }
}