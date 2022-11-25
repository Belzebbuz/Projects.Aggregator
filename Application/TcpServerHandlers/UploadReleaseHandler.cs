using BeetleX;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SharedLibrary.BeetlexMessages;
using System.Collections.Concurrent;

namespace Application.TcpServerHandlers;

public class UploadReleaseHandler : ServerHandlerBase
{
	private readonly ConcurrentDictionary<string, FileTransfer> _fileStreams = new (StringComparer.OrdinalIgnoreCase);
    private IServiceProvider _serviceProvider;
	private ILogger<UploadReleaseHandler> _logger;
	private const string ReleaseUploadFolder = "Files\\Projects";
	public UploadReleaseHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
		_logger = _serviceProvider.GetRequiredService<ILogger<UploadReleaseHandler>>();
    }

	protected override void OnReceiveMessage(IServer server, ISession session, object message)
	{
		if (message is FileContentBlock block)
		{
			var sessionId = $"{block.ProjectId}{block.ReleaseId}";
			try
			{
				string path = GetDownloadPath(block);
				_fileStreams.TryGetValue(sessionId, out FileTransfer value);

				if (block.Index == 0)
				{
					value = HandleFirstBlock(block, path, value, sessionId);
				}

				value?.Stream.Write(block.Data, 0, block.Data.Length);

				if (block.Eof)
				{
					value?.Dispose();
					_fileStreams.TryRemove(sessionId, out value);
				}
			}
			catch (Exception ex)
			{
				session?.Dispose();
				_fileStreams.TryRemove(sessionId, out FileTransfer value);
				_logger.LogError($"{ex.Message}\n{ex.StackTrace}");
			}

		}
		base.OnReceiveMessage(server, session, message);
	}

	private string GetDownloadPath(FileContentBlock block)
	{
		var projectDirectory = Path.Combine(ReleaseUploadFolder, block.ProjectId.ToString());
		if(!Directory.Exists(projectDirectory))
			Directory.CreateDirectory(projectDirectory);

		return Path.Combine(projectDirectory, $"{block.ReleaseId}-{block.FileName}");
	}

	private FileTransfer HandleFirstBlock(FileContentBlock block, string path, FileTransfer fileTransferStream, string sessionId)
	{
		if (fileTransferStream != null)
		{
			fileTransferStream.Dispose();
		}
		fileTransferStream = new FileTransfer(path);
		_fileStreams[sessionId] = fileTransferStream;
		return fileTransferStream;
	}
}
