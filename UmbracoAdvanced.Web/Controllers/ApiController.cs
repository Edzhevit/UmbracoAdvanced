using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.Common.Controllers;

namespace UmbracoAdvanced.Web.Controllers;

/// <summary>
/// API Controllers are mostly used for API calls or get objects from the DB
/// </summary>
public class ApiController : UmbracoApiController
{
    // /umbraco/api/api/getproducts
    public IActionResult GetProducts()
    {
        var products = new List<SimpleProduct>()
        {
            new()
            {
                Id = "abc",
                Title = "Product Title"
            },
            new()
            {
                Id = "def",
                Title = "Product two"
            }
        };

        return Ok(products);
    }

    private class SimpleProduct
    {
        public string Id { get; set; }
        public string Title { get; set; }
    }
}