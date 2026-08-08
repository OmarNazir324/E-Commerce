using API.Options;
using Application.DataBaseOptions;

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

            return services;
        }
    }
}
