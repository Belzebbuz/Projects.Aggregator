using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Options;

public static class Startup
{
    public static IServiceCollection AddAppOptions(this IServiceCollection services, IConfiguration config)
    {
        return services
            .Configure<ReleaseOptions>(config.GetSection(nameof(ReleaseOptions)));
    }
}
