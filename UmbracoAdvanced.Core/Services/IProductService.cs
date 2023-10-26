using UmbracoAdvanced.Core.Models;
using UmbracoAdvanced.Core.Models.Records;

namespace UmbracoAdvanced.Core.Services;

public interface IProductService
{
    List<ProductDTO> GetAll();
    List<ProductResponseItem> GetUmbracoProducts(int number);
}