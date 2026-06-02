using Microsoft.Extensions.Options;

namespace API.Options;

public class DataBaseOptionsSetup : IConfigureOptions<DataBaseOptions>
{
    const String ConfigrationSectionName = "DatabaseOptoons";
    private readonly IConfiguration _configuration;
    public DataBaseOptionsSetup(IConfiguration configuration)
        => _configuration = configuration;

    public void Configure(DataBaseOptions options)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        options.ConnectionString = connectionString;
        _configuration.GetSection(ConfigrationSectionName).Bind(options);
    }
}
