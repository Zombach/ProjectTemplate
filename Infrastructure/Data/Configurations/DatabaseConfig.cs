namespace Infrastructure.Data.Configurations;

public sealed class DatabaseConfig
{
    public string DefaultConnection { get; init; } = string.Empty;
    public string DatabaseConnection { get; init; } = string.Empty;
}