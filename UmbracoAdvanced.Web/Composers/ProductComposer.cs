using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Web.Common.ApplicationBuilder;
using UmbracoAdvanced.Web.Controllers;
using UmbracoAdvanced.Core;

namespace UmbracoAdvanced.Web.Composers;

public class ProductComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.Services.AddScoped<IProductService, ProductService>();

        // USE IT WITH CUSTOM ROUTING, REMOVE IF YOU USE ATTRIBUTE ROUTING
        // builder.Services.Configure<UmbracoPipelineOptions>(opt =>
        // {
        //     opt.AddFilter(new UmbracoPipelineFilter(nameof(ProductController))
        //     {
        //         Endpoints = app => app.UseEndpoints(endpoints =>
        //         {
        //             endpoints.MapControllerRoute("Product Controller", "/product/{action}/{id?}",
        //                 new { Controller = "Product", Action = "Index" });
        //         })
        //     });
        // });
    }
}