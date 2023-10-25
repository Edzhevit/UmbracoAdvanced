using UmbracoAdvanced.Core.Models;

namespace UmbracoAdvanced.Core;

public interface IProductService
{
    List<ProductDTO> GetAll();
}