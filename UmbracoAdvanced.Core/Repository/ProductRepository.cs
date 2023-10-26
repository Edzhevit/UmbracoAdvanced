using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;
using UmbracoAdvanced.Core.Models.Umbraco;

namespace UmbracoAdvanced.Core.Repository;

public class ProductRepository : IProductRepository
{
    private readonly IUmbracoContextFactory _contextFactory;

    public ProductRepository(IUmbracoContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public List<Product> GetProducts()
    {
        var products = GetProductsRootPage();
        var final = new List<Product>();

        if (products is { } productsRoot)
        {
            final = productsRoot.Children<Product>()?.Where(x => x.IsPublished()).ToList() ?? new List<Product>();
        }

        return final;
    }

    private Products? GetProductsRootPage()
    {
        using var cref = _contextFactory.EnsureUmbracoContext();
        var rootNode = cref.UmbracoContext.Content?.GetAtRoot()
            .FirstOrDefault(x => x.ContentType.Alias == Home.ModelTypeAlias);

        return rootNode?.Descendant<Products>();
    }
}