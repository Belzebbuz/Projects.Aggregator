using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
namespace Infrastructure.TcpServers;

public static class Startup
{
    public static IServiceCollection AddTcpServers(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TcpServerOptions>(configuration.GetSection(nameof(TcpServerOptions)));
        services.AddHostedService<TcpServersFactory>();
        return services;
    }
}
