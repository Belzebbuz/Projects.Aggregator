using System.Reflection;
using Application.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class Startup
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration config)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddAppOptions(config);
        return services;
    }
}
