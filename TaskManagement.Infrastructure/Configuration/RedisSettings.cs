namespace TaskManagement.Infrastructure.Configuration;

public class RedisSettings
{
    public const string SectionName = "RedisSettings";
    
    public string ConnectionString { get; set; } = string.Empty;
    public int DefaultExpirationTimeInMinutes { get; set; } = 60;
}
