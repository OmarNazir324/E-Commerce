using Application.DataBaseOptions;
using Microsoft.Extensions.Options;

namespace API.Options;

public class DataBaseOptionsSetup : IConfigureOptions<DataBaseOptions>
{
    private readonly IConfiguration _configuration;
    private const String ConfigurationJWT = "JWT";
    private const String ConfigurationRedis = "Redis";
    const String ConfigrationSectionName = "DatabaseOptions";
    public DataBaseOptionsSetup(IConfiguration configuration)
        => _configuration = configuration;

    public void Configure(DataBaseOptions options)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        options.ConnectionString = connectionString!;
        _configuration.GetSection(ConfigrationSectionName).Bind(options);
        _configuration.GetSection(ConfigurationJWT).Bind(options);
        _configuration.GetSection(ConfigurationRedis).Bind(options);

    }
}
