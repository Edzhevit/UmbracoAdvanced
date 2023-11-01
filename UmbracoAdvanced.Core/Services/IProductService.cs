using UmbracoAdvanced.Core.Models;

namespace UmbracoAdvanced.Core.Services;

public interface IProductService
{
    List<ProductDTO> GetAll();
    List<ProductResponseItem> GetUmbracoProducts(int number);
}