namespace API.Options;

public class DataBaseOptions
{
    public String ConnectionString { get; set; } = String.Empty;
    public int CommandTimeOut { get; set; }
    public int RetryOnFailure { get; set; }
    public bool EnableDetailedErrors { get; set; }
    public bool EnableSenstiveDataLogging { get; set; }
}
