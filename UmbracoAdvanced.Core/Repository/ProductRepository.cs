using SixLabors.ImageSharp.Formats.Jpeg;
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
using Image = UmbracoAdvanced.Core.Models.Umbraco.Image;

namespace UmbracoAdvanced.Core.Repository;

public class ProductRepository : IProductRepository
{
    private readonly Guid _productsMediaFolder = Guid.Parse("1ad4e9a3-4ad5-476c-aba2-9602e34e323f");
    private readonly string _productNameAlias;
    private readonly string _productPriceAlias;
    private readonly string _productCategoriesAlias;
    private readonly string _productDescriptionAlias;
    private readonly string _productSkuAlias;
    private readonly string _productPhotoAlias;

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

    public ProductRepository(IUmbracoContextFactory contextFactory, IContentService contentService, IMediaService mediaService,
        MediaFileManager mediaFileManager, IShortStringHelper shortStringHelper, MediaUrlGeneratorCollection mediaUrlGeneratorCollection, 
        IContentTypeBaseServiceProvider contentTypeBaseServiceProvider, IPublishedSnapshotAccessor publishedSnapshotAccessor)
    {
        _contextFactory = contextFactory;
        _contentService = contentService;
        _mediaService = mediaService;
        _mediaFileManager = mediaFileManager;
        _shortStringHelper = shortStringHelper;
        _mediaUrlGeneratorCollection = mediaUrlGeneratorCollection;
        _contentTypeBaseServiceProvider = contentTypeBaseServiceProvider;
        var snapshotAccessor = publishedSnapshotAccessor;

        _productNameAlias = Product.GetModelPropertyType(snapshotAccessor, x => x.ProductName)?.Alias;
        _productPriceAlias = Product.GetModelPropertyType(snapshotAccessor, x => x.Price)?.Alias;
        _productCategoriesAlias = Product.GetModelPropertyType(snapshotAccessor, x => x.Category)?.Alias;
        _productDescriptionAlias = Product.GetModelPropertyType(snapshotAccessor, x => x.Description)?.Alias;
        _productSkuAlias = Product.GetModelPropertyType(snapshotAccessor, x => x.Sku)?.Alias;
        _productPhotoAlias = Product.GetModelPropertyType(snapshotAccessor, x => x.Photos)?.Alias;
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
                filteredProducts = filteredProducts?.Where(x => x.Sku.InvariantEquals(productSKU));
            }
            if (maxPrice is { } MaxPrice)
            {
                filteredProducts = filteredProducts?.Where(x => x.Price <= MaxPrice);
            }
            final = filteredProducts?.ToList() ?? new List<Product>();
        }

        return final;
    }

    public Product Get(int id)
    {
        using var cref = _contextFactory.EnsureUmbracoContext();
        var content = cref.UmbracoContext.Content;
        return (Product)content?.GetById(id);
    }

    public Product Create(ProductCreationItem product)
    {
        var productImage = CreateProductImage(product.PhotoFileName, product.Photo);
        if (productImage == null)
        {
            return null;
        }

        var productsRoot = GetProductsRootPage();
        var productContent = _contentService.Create(product.ProductName, productsRoot.Key, Product.ModelTypeAlias);

        productContent.SetValue(_productNameAlias, product.ProductName);
        productContent.SetValue(_productPriceAlias, product.Price);
        productContent.SetValue(_productCategoriesAlias, string.Join(",", product.Categories));
        productContent.SetValue(_productDescriptionAlias, product.Description);
        productContent.SetValue(_productSkuAlias, product.SKU);
        productContent.SetValue(_productPhotoAlias, productImage);

        _contentService.SaveAndPublish(productContent);

        return Get(productContent.Id);
    }

    public Product Update(int id, ProductUpdateItem product)
    {
        var productContent = _contentService.GetById(id);
        if (!string.IsNullOrEmpty(product.ProductName))
        {
            productContent?.SetValue(_productNameAlias, product.ProductName);
        }
        if (product.Price != null)
        {
            productContent?.SetValue(_productPriceAlias, product.Price);
        }
        if (product.Categories != null && product.Categories.Any())
        {
            productContent?.SetValue(_productCategoriesAlias, string.Join(",", product.Categories));
        }
        if (!string.IsNullOrEmpty(product.Description))
        {
            productContent?.SetValue(_productDescriptionAlias, product.Description);
        }
        if (!string.IsNullOrEmpty(product.SKU))
        {
            productContent?.SetValue(_productSkuAlias, product.SKU);
        }

        if (!string.IsNullOrEmpty(product.PhotoFileName) && !string.IsNullOrEmpty(product.Photo))
        {
            var productImage = CreateProductImage(product.PhotoFileName, product.Photo);
            if (productImage != null)
            {
                productContent?.SetValue(_productPhotoAlias, productImage);
            }
        }

        _contentService.SaveAndPublish(productContent);

        return Get(id);
    }

    public bool Delete(int id)
    {
        var product = _contentService.GetById(id);
        if (product == null) return false;
        var result = _contentService.Delete(product);
        return result.Success;
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
        //Save image to a tmp path
        var tmpFilePath = Path.GetTempFileName();
        using var image = SixLabors.ImageSharp.Image.Load(Convert.FromBase64String(photo));
        image.Save(tmpFilePath, new JpegEncoder());

        //load file into a file stream
        var fileInfo = new FileInfo(tmpFilePath);
        var fileStream = fileInfo.OpenReadWithRetry();
        if (fileStream == null)
        {
            throw new InvalidOperationException("Could not acquire file stream");
        }

        var umbracoMedia = _mediaService.CreateMedia(filename, _productsMediaFolder, Image.ModelTypeAlias);

        using (fileStream)
        {
            umbracoMedia.SetValue(_mediaFileManager, _mediaUrlGeneratorCollection, _shortStringHelper, 
                _contentTypeBaseServiceProvider, Constants.Conventions.Media.File, filename, fileStream);

            var result = _mediaService.Save(umbracoMedia);
            if (!result.Success)
            {
                return null;
            }
        }

        return umbracoMedia.GetUdi();
    }
}