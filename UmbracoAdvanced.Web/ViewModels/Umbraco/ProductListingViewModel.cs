using Umbraco.Cms.Core.Models.PublishedContent;
using UmbracoAdvanced.Core.Models.Umbraco;

namespace UmbracoAdvanced.Web.ViewModels.Umbraco;

public class ProductListingViewModel : Products
{
    public ProductListingViewModel(IPublishedContent content, IPublishedValueFallback publishedValueFallback) 
        : base(content, publishedValueFallback) { }

    public List<Product> Products { get; set; }
}