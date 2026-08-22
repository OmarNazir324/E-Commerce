using API.Extensions;
using API.Options;
using Application.DataBaseOptions;
using Domain.Entities;
using InfraStructure.Repositories.Generic;

namespace API.APIServices
{
    public static class APIService
    {
        public static IServiceCollection AddAPIService(
                        this IServiceCollection services,
                        IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration =
                    configuration.GetSection(nameof(DataBaseOptions))
                                 .Get<DataBaseOptions>()!
                                 .CashingUrl;
            });
            services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
            return services;
        }
    }
}
