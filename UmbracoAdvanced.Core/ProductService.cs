using UmbracoAdvanced.Core.Models;

namespace UmbracoAdvanced.Core;

public class ProductService : IProductService
{
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
}