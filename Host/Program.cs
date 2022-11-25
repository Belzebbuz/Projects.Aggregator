using Application;
using Infrastructure;
using Infrastructure.Common;
using Microsoft.Extensions.Hosting.WindowsServices;
using Serilog;

StaticLogger.EnsureInitialized();
Log.Information("Server Booting Up...");
if(WindowsServiceHelpers.IsWindowsService())
    Directory.SetCurrentDirectory(AppContext.BaseDirectory);
try
{
	var options = new WebApplicationOptions
	{
		Args = args,
		ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default,
	};
	var builder = WebApplication.CreateBuilder(options);
	builder.Host.UseWindowsService();
	builder.Host.UseSerilog((_, config) =>
    {
        config.WriteTo.Console()
        .ReadFrom.Configuration(builder.Configuration);
    });
	builder.Services.AddControllers();
    builder.Services.AddApplication(builder.Configuration);
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    await app.UseInfrastructure(app.Configuration);
    app.MapControllers();
    app.Run();
}
catch (Exception ex) when (!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
{
    StaticLogger.EnsureInitialized();
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    StaticLogger.EnsureInitialized();
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}

