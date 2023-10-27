using UmbracoAdvanced.Core.Models;
using UmbracoAdvanced.Core.Models.Umbraco;

namespace UmbracoAdvanced.Core.Repository;

public interface IProductRepository
{
    List<Product> GetProducts(string? productSKU, decimal? maxPrice);
    Product Create(ProductCreationItem product);
    bool Delete(int id);
}