using Application.Features.CategoryFeatuure.Interfaces;
using Application.Features.CategoryFeatuure.Service;
using Application.Features.CustomerFeature.InterFaces;
using Application.Features.CustomerFeature.Service;
using Application.Features.Order_ItemsFeature.InterFace;
using Application.Features.Order_ItemsFeature.Service;
using Application.Features.OrderFeature.InterFace;
using Application.Features.OrderFeature.Service;
using Application.Features.Product.Interfaces;
using Application.Features.ProductFeature.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services;

public static class ApplicationServices 
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrder_ItemsService, Order_ItemsService>();
        return services;
    }
}
