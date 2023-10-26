using UmbracoAdvanced.Core.Models.Umbraco;

namespace UmbracoAdvanced.Core.Repository;

public interface IProductRepository
{
    List<Product> GetProducts();
}