using Clients.MAUI.Application;
using Clients.MAUI.Application.Contracts.Services;
using Clients.MAUI.Infrastructure.Authentication;
using Clients.MAUI.Infrastructure.Constants;
using Clients.MAUI.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using System.Reflection;

namespace Clients.MAUI.Infrastructure;

public static class Startup
{
    private static string _baseAddress = "http://localhost:5195";

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

        services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("MauiClient"))
            .AddHttpClient("MauiClient", async client =>
            {
                client.Timeout = TimeSpan.FromMinutes(10);
				var baseAddress = await SecureStorage.GetAsync(StorageConstants.ServerURL);
                if(baseAddress == null)
                {
                    baseAddress = _baseAddress;
                    await SecureStorage.SetAsync(StorageConstants.ServerURL, _baseAddress);
                }
				client.BaseAddress = new Uri(baseAddress);
            })
            .AddHttpMessageHandler<AuthenticationHeaderHandler>();
        return services;

    }
}
