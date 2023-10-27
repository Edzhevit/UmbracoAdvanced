using Umbraco.Cms.Core;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;
using UmbracoAdvanced.Core.Models;
using UmbracoAdvanced.Core.Models.Umbraco;

namespace UmbracoAdvanced.Core.Repository;

public class ProductRepository : IProductRepository
{
    private readonly Guid _productsMediaFolder = Guid.Parse("1ad4e9a3-4ad5-476c-aba2-9602e34e323f");

    private readonly IUmbracoContextFactory _contextFactory;
    private readonly IContentService _contentService;
    // IMediaService is for creating the Media Object
    private readonly IMediaService _mediaService;
    // These interfaces are needed for passing it to MediaService in order to create umbraco object
    private readonly MediaFileManager _mediaFileManager;
    private readonly IShortStringHelper _shortStringHelper;
    private readonly MediaUrlGeneratorCollection _mediaUrlGeneratorCollection;
    private readonly IContentTypeBaseServiceProvider _contentTypeBaseServiceProvider;
    // This is needed to retrieve property alias from an umbraco object
    private readonly IPublishedSnapshotAccessor _publishedSnapshotAccessor;

    public ProductRepository(IUmbracoContextFactory contextFactory, IContentService contentService, IMediaService mediaService, MediaFileManager mediaFileManager, IShortStringHelper shortStringHelper, MediaUrlGeneratorCollection mediaUrlGeneratorCollection, IContentTypeBaseServiceProvider contentTypeBaseServiceProvider, IPublishedSnapshotAccessor publishedSnapshotAccessor)
    {
        _contextFactory = contextFactory;
        _contentService = contentService;
        _mediaService = mediaService;
        _mediaFileManager = mediaFileManager;
        _shortStringHelper = shortStringHelper;
        _mediaUrlGeneratorCollection = mediaUrlGeneratorCollection;
        _contentTypeBaseServiceProvider = contentTypeBaseServiceProvider;
        _publishedSnapshotAccessor = publishedSnapshotAccessor;
    }

    public List<Product> GetProducts(string? productSKU, decimal? maxPrice)
    {
        var products = GetProductsRootPage();
        var final = new List<Product>();

        if (products is { } productsRoot)
        {
            var filteredProducts = productsRoot.Children<Product>()?.Where(x => x.IsPublished());
            if (!string.IsNullOrEmpty(productSKU))
            {
                filteredProducts = filteredProducts.Where(x => x.Sku.InvariantEquals(productSKU));
            }
            if (maxPrice is { } MaxPrice)
            {
                filteredProducts = filteredProducts?.Where(x => x.Price <= MaxPrice);
            }
            final = filteredProducts?.ToList() ?? new List<Product>();
        }

        return final;
    }

    public Product Create(ProductCreationItem product)
    {
        return null;
    }

    public bool Delete(int id)
    {
        var product = _contentService.GetById(id);
        if (product != null)
        {
            var result = _contentService.Delete(product);
            return result.Success;
        }

        return false;
    }

    private Products? GetProductsRootPage()
    {
        using var cref = _contextFactory.EnsureUmbracoContext();
        var rootNode = cref.UmbracoContext.Content?.GetAtRoot()
            .FirstOrDefault(x => x.ContentType.Alias == Home.ModelTypeAlias);

        return rootNode?.Descendant<Products>();
    }

    private GuidUdi? CreateProductImage(string filename, string photo)
    {
        return null;
    }
}