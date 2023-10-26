using UmbracoAdvanced.Core.Models;
using UmbracoAdvanced.Core.Models.Records;

namespace UmbracoAdvanced.Core;

public interface IProductService
{
    List<ProductDTO> GetAll();
    List<ProductResponseItem> GetUmbracoProducts(int number);
}