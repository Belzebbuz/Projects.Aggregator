namespace Infrastructure.Context;

public class DatabaseSettings
{
    public string ConnectionString { get; set; }
    public string Provider { get; set; }
}

public class DbProviderKeys
{
    public const string SQLServer = nameof(SQLServer);
    public const string PostgreSQL = nameof(PostgreSQL);
    public const string Sqlite = nameof(Sqlite);
    public const string InMemory = nameof(InMemory);

}