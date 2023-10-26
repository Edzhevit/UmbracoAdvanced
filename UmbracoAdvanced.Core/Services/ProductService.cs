using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;
using UmbracoAdvanced.Core.Models;
using UmbracoAdvanced.Core.Models.Records;
using UmbracoAdvanced.Core.Models.Umbraco;
using UmbracoAdvanced.Core.Services;

namespace UmbracoAdvanced.Core;

public class ProductService : IProductService
{
    private readonly IUmbracoContextFactory _contextFactory;

    public ProductService(IUmbracoContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public List<ProductDTO> GetAll()
    {
        return new List<ProductDTO>()
        {
            new() { Id = 1, Name = "Product 1" },
            new() { Id = 2, Name = "Product 2" },
            new() { Id = 3, Name = "Product 3" },
            new() { Id = 4, Name = "Product 4" },
            new() { Id = 5, Name = "Product 5" }
        };
    }

    public List<ProductResponseItem> GetUmbracoProducts(int number)
    {
        var final = new List<ProductResponseItem>();

        using var cref = _contextFactory.EnsureUmbracoContext();
        var contentCache = cref.UmbracoContext.Content;
        var products = contentCache
            ?.GetAtRoot()
            .FirstOrDefault(x => x.ContentType.Alias == Home.ModelTypeAlias)
            ?.Descendant<Products>()
            ?.Children<Product>()
            ?.Take(number);

        if (products != null && products.Any())
        {
            final = products.Select(x => new ProductResponseItem(x.Id, x.ProductName ?? x.Name, x.Photos?.Url() ?? "#"))
                .ToList();
        }

        return final;
    }
}