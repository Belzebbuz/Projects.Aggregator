using Application.Contracts.Repository;
using Domain.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Context;

public static class Startup
{
    public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration config)
    {
        var dbSettings = GetDbSettings(config);
        return services
            .AddTransient<AppDbSeeder>()
            .Configure<MockDataSettings>(config.GetSection(nameof(MockDataSettings)))
            .Configure<DatabaseSettings>(config.GetSection(nameof(DatabaseSettings)))
            .AddDbContext<ApplicationDbContext>(m => m.UseDatabase(dbSettings.Provider!, dbSettings.ConnectionString!))
            .AddRepositories();
    }
    public static async Task InitDatabaseAsync<T>(this IApplicationBuilder app) where T : DbContext
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetService<T>();
        await context!.Database.MigrateAsync();
        await scope.ServiceProvider.GetService<AppDbSeeder>()!.SeedDataAsync();
    }
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(AppDbRepository<>));

        foreach (var aggregateRootType in
                 typeof(IAggregateRoot).Assembly.GetExportedTypes()
                     .Where(t => typeof(IAggregateRoot).IsAssignableFrom(t) && t.IsClass)
                     .ToList())
        {
            services.AddScoped(typeof(IReadRepository<>).MakeGenericType(aggregateRootType), sp =>
                sp.GetRequiredService(typeof(IRepository<>).MakeGenericType(aggregateRootType)));
        }
        return services;
    }

    private static DatabaseSettings GetDbSettings(IConfiguration config)
    {
        var databaseSettings = config.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();
        string? rootConnectionString = databaseSettings.ConnectionString;
        if (string.IsNullOrEmpty(rootConnectionString))
        {
            throw new InvalidOperationException("DB ConnectionString is not configured.");
        }

        string? dbProvider = databaseSettings.Provider;
        if (string.IsNullOrEmpty(dbProvider))
        {
            throw new InvalidOperationException("DB Provider is not configured.");
        }
        return databaseSettings;
    }

    internal static DbContextOptionsBuilder UseDatabase(this DbContextOptionsBuilder builder, string dbProvider, string connectionString)
    {
        switch (dbProvider)
        {
            case DbProviderKeys.PostgreSQL:
                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                return builder.UseNpgsql(connectionString, e =>
                    e.MigrationsAssembly("Migrators.PostgreSQL"));

            case DbProviderKeys.SQLServer:
                return builder.UseSqlServer(connectionString, e =>
                    e.MigrationsAssembly("Migrators.SQLServer"));

            case DbProviderKeys.Sqlite:
                return builder.UseSqlite(connectionString, e =>
                    e.MigrationsAssembly("Migrators.Sqlite"));

            case DbProviderKeys.InMemory:
                return builder.UseInMemoryDatabase("MemoryDatabase");
            default:
                throw new InvalidOperationException($"DB Provider {dbProvider} is not supported.");
        }
    }
}
