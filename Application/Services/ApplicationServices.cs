using Application.Features.CategoryFeatuure.Interfaces;
using Application.Features.CategoryFeatuure.Service;
using Application.Features.CustomerFeature.InterFaces;
using Application.Features.CustomerFeature.Service;
using Application.Features.Email.Interfaces;
using Application.Features.Email.Service;
using Application.Features.LoginFeature.Interfaces;
using Application.Features.LoginFeature.Service;
using Application.Features.OrderFeature.InterFace;
using Application.Features.OrderFeature.Service;
using Application.Features.Product.Interfaces;
using Application.Features.ProductFeature.Service;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
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
        services.AddScoped<ILoginService, LoginService>();
        services.AddSingleton<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IEmailService, EmailService>();
        return services;
    }
}
