using Infrastructure.Performance;

namespace API.Extensions;

public static class PerformanceProxyExtensions
{
    public static IServiceCollection AddPerformanceProxy<
        TInterface,
        TImplementation>(
        this IServiceCollection services)
        where TInterface : class
        where TImplementation : class, TInterface
    {
        services.AddScoped<TImplementation>();

        services.AddScoped<TInterface>(sp =>
        {
            var implementation =
                sp.GetRequiredService<TImplementation>();

            var loggerFactory =
                sp.GetRequiredService<ILoggerFactory>();

            var logger =
                loggerFactory.CreateLogger(
                    $"Performance.{typeof(TInterface).Name}");

            return PerformanceProxy<TInterface>.Create(
                implementation,
                logger);
        });

        return services;
    }
}