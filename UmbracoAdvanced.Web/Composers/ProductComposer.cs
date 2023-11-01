using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Mapping;
using UmbracoAdvanced.Core.Repository;
using UmbracoAdvanced.Core.Services;
using UmbracoAdvanced.Web.Mappings;
using UmbracoAdvanced.Web.Routing;

namespace UmbracoAdvanced.Web.Composers;

public class ProductComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.UrlSegmentProviders().Insert<ProductPageUrlSegmentProvider>();

        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();

        builder.WithCollectionBuilder<MapDefinitionCollectionBuilder>().Add<ProductMapping>();

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