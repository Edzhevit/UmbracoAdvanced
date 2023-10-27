using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Strings;
using UmbracoAdvanced.Core.Models.Umbraco;
using IContentBase = Umbraco.Cms.Core.Models.IContentBase;

namespace UmbracoAdvanced.Web.Routing;

public class ProductPageUrlSegmentProvider : IUrlSegmentProvider
{
    private readonly IUrlSegmentProvider _urlSegmentProvider;
    private readonly IPublishedSnapshotAccessor _publishedSnapshotAccessor;

    private readonly string _skuAlias;

    public ProductPageUrlSegmentProvider(IShortStringHelper stringHelper, IPublishedSnapshotAccessor publishedSnapshotAccessor)
    {
        _urlSegmentProvider = new DefaultUrlSegmentProvider(stringHelper);
        _publishedSnapshotAccessor = publishedSnapshotAccessor;

        _skuAlias = Product.GetModelPropertyType(_publishedSnapshotAccessor, x => x.Sku).Alias;
    }

    public string GetUrlSegment(IContentBase content, string culture = null)
    {
        // only apply this rule for product pages
        if (content.ContentType.Alias != Product.ModelTypeAlias)
        {
            return null;
        }

        var currentSegment = _urlSegmentProvider.GetUrlSegment(content, culture);
        var productSKU = content.GetValue<string>(_skuAlias).ToLower();

        return !string.IsNullOrEmpty(productSKU) ? $"{currentSegment}--{productSKU}" : currentSegment;
    }
}