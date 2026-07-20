
namespace Application.DataBaseOptions;

public class DataBaseOptions
{
    public String ConnectionString { get; set; } = String.Empty;
    public int CommandTimeOut { get; set; }
    public int RetryOnFailure { get; set; }
    public bool EnableDetailedErrors { get; set; }
    public bool EnableSenstiveDataLogging { get; set; }
    public String ValidAudience { get; set; }
    public String ValidIssuer { get; set; }
    public String Secret { get; set; }
    public String AccessTokenMinutesForDevelopment { get; set; } = String.Empty;

    public String AccessTokenMinutes { get; set; }
    public String RefreshTokenDays { get; set; }
    public String CashingUrl { get; set; }

}

