﻿using Microsoft.AspNetCore.Mvc;
using UmbracoAdvanced.Core;
using UmbracoAdvanced.Web.ViewModels;

namespace UmbracoAdvanced.Web.ViewComponents;

public class ProductListingViewComponent : ViewComponent
{
    private readonly IProductService _productService;

    public ProductListingViewComponent(IProductService productService)
    {
        _productService = productService;
    }

    public IViewComponentResult Invoke(int number)
    {
        var model = new ProductListingViewModel()
        {
            Products = _productService.GetUmbracoProducts(number)
        };

        return View(model);
    }
}