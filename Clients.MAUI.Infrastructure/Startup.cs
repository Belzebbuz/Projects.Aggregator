using Clients.MAUI.Application;
using Clients.MAUI.Infrastructure.Authentication;
using Clients.MAUI.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using System.Reflection;

namespace Clients.MAUI.Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        return services
			.AddMudServices()
		    .AddSingleton<IFilePicker>(FilePicker.Default)
            .AddAuthorizationCore()
            .AddScoped<LocalAuthenticationStateProvider>()
            .AddScoped<AuthenticationStateProvider, LocalAuthenticationStateProvider>()
            .AddTransient<AuthenticationHeaderHandler>()
            .AddServices()
            .AddHttpClient();
    }

    public static IServiceCollection AddHttpClient(this IServiceCollection services)
    {

        services.AddScoped(sp =>
            sp.GetRequiredService<IHttpClientFactory>()
            .CreateClient("MauiClient"))
            .AddHttpClient("MauiClient", client =>
            {
                client.BaseAddress = new Uri("http://localhost:5195");
            })
            .AddHttpMessageHandler<AuthenticationHeaderHandler>();
        return services;

    }
}
