using API.Options;
using Application.DataBaseOptions;

namespace API.APIServices
{
    public static class APIService
    {
        static DataBaseOptions _options;
        public static IServiceCollection AddAPIService(this IServiceCollection services, IConfiguration configuration,DataBaseOptions options)
        {
            _options = options;
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = _options.CashingUrl;
                   
            });

            return services;
        }
    }
}
