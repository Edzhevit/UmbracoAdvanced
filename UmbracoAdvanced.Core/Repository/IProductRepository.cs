using UmbracoAdvanced.Core.Models;
using UmbracoAdvanced.Core.Models.Umbraco;

namespace UmbracoAdvanced.Core.Repository;

public interface IProductRepository
{
    List<Product> GetProducts(string? productSKU, decimal? maxPrice);
    Product Get(int id);
    Product Create(ProductCreationItem product);
    Product Update(int id, ProductUpdateItem product);
    bool Delete(int id);
}