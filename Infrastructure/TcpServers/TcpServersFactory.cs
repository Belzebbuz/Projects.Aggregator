using Application.TcpServerHandlers;
using BeetleX;
using BeetleX.EventArgs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SharedLibrary.BeetlexMessages;

namespace Infrastructure.TcpServers;

public class TcpServersFactory : BackgroundService
{
    private readonly IServer _uploadServer;

    public TcpServersFactory(IOptions<TcpServerOptions> options, IServiceProvider serviceProvider)
    {
        _uploadServer = SocketFactory.CreateTcpServer(new UploadReleaseHandler(serviceProvider), new ProtobufPacket());
		_uploadServer.Options.LogLevel = LogType.Warring;
		_uploadServer.Options.BufferSize = 1024 * 8;
		_uploadServer.Options.DefaultListen.Port = options.Value.UploadPort;
	}
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
		Task.Run(() =>
		{
			_uploadServer.Open();
			Thread.Sleep(-1);
		});
		return Task.CompletedTask;
	}
}
