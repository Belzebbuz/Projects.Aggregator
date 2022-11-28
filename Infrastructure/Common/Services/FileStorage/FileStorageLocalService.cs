using Application.Contracts.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using SharedLibrary.Helpers;
using SharedLibrary.Wrapper;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Net.Mime;

namespace Infrastructure.Common.Services.FileStorage;

internal class FileStorageLocalService : IFileStorageService
{
	private const string RootProjectsFolder = "Files\\Projects";
	private readonly string _tempFolder = $"Files\\Temp\\{Guid.NewGuid()}";
	private const string BugReportsImagesFolder = "Files\\BugReports";
	private const string AvailableFileExtension = ".zip";
	private readonly IHttpContextAccessor _httpContext;
	private readonly ILogger<FileStorageLocalService> _logger;

	public FileStorageLocalService(IHttpContextAccessor httpContext, ILogger<FileStorageLocalService> logger)
	{
		_httpContext = httpContext;
		_logger = logger;
	}
	public void DeleteFiles(IEnumerable<string> filePaths)
	{
        foreach (string? url in filePaths.Where(url => File.Exists(url)))
        {
            File.Delete(url);
        }
    }

	public void DeleteSingleFile(string url)
	{
		if (File.Exists(url))
			File.Delete(url);
	}

	public FileStream DownloadAsync(string url)
	{
		ThrowHelper.FileNotExists(url);
		return new FileStream(url, FileMode.Open, FileAccess.Read);
	}

	public async Task<IResult<string>> SaveFileStreamingAsync(string folder)
	{
		try
		{
			var request = _httpContext.HttpContext!.Request;
			var boundary = HeaderUtilities.RemoveQuotes(
				MediaTypeHeaderValue.Parse(request.ContentType).Boundary
			).Value;

			var reader = new MultipartReader(boundary, request.Body);
			var section = await reader.ReadNextSectionAsync();

			string projectFolder = Path.Combine(RootProjectsFolder, folder);
			if (!Directory.Exists(projectFolder))
				Directory.CreateDirectory(projectFolder);

			var saveFileResult = await SaveFileAsync(reader, section, projectFolder);
			if (!saveFileResult.Succeeded)
				return Result<string>.Fail(saveFileResult.Messages);

			return Result<string>.Success(data: saveFileResult.Data);

		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private async Task<IResult<string>> SaveFileAsync(MultipartReader reader, MultipartSection? section, string folderPath)
	{
		try
		{
			string fileFullPath = string.Empty;
			while (section != null)
			{
				var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(
					section.ContentDisposition, out var contentDisposition
				);
                ThrowHelper.NotNull(contentDisposition, nameof(section.ContentDisposition));

                if (hasContentDispositionHeader && contentDisposition!.DispositionType.Equals("form-data") &&
                    (!string.IsNullOrEmpty(contentDisposition.FileName.Value) ||
                    !string.IsNullOrEmpty(contentDisposition.FileNameStar.Value)))
                {
                    fileFullPath = CreateUploadFileFullPath(folderPath, contentDisposition.FileName.Value);
                    await using var fs = File.Create(fileFullPath);
                    await section.Body.CopyToAsync(fs);
                    await fs.FlushAsync();
                }
                section = await reader.ReadNextSectionAsync();
			}
			return Result<string>.Success(data: fileFullPath);
		}
		catch (Exception ex)
		{
			return Result<string>.Fail(ex.Message);
		}
		
	}

	private string CreateUploadFileFullPath(string folderPath, string fileName)
	{
		var fileExtension = Path.GetExtension(fileName);
		return Path.Combine(folderPath, $"{Guid.NewGuid()}{fileExtension}");
	}

	public async Task<IResult<IExeFileVersionInfo>> GetExeFileVersionAsync(string zipFilePath, string exeFileName)
	{
        try
		{
			if (!Directory.Exists(_tempFolder))
				Directory.CreateDirectory(_tempFolder);

			using var zipArchive = new ZipArchive(File.OpenRead(zipFilePath));
			var exefile = zipArchive.Entries.SingleOrDefault(x => x.Name == exeFileName);
			if (exefile == null)
			{
				zipArchive?.Dispose();
				return await Result<IExeFileVersionInfo>.FailAsync($"Exe file：{exeFileName} not found!");
			}

			var tempExeFile = Path.Combine(_tempFolder, Path.GetFileName(exefile.Name));
			exefile.ExtractToFile(tempExeFile);

			var fileVersion = FileVersionInfo.GetVersionInfo(tempExeFile).FileVersion;
			var productVersion = FileVersionInfo.GetVersionInfo(tempExeFile).ProductVersion;
			if (productVersion != null)
			{
				var gitBranch = productVersion.Contains("Branch")
				? productVersion.Split("Branch")[1].Split("Sha")[0].Replace(".", "")
				: String.Empty;
				var gitSha = productVersion.Contains("Sha")
					? productVersion.Split("Sha")[1].Replace(".", "")
					: String.Empty;
				return Result<IExeFileVersionInfo>.Success(data: new ExeFileVersionInfo(fileVersion, gitSha, gitBranch));
			}
			return Result<IExeFileVersionInfo>.Success(new ExeFileVersionInfo(fileVersion, null, null));
		}
		catch (Exception ex)
		{
			return Result<IExeFileVersionInfo>.Fail(ex.Message);
		}

	}
}

internal class ExeFileVersionInfo  : IExeFileVersionInfo
{
	public ExeFileVersionInfo(string version, string? gitSha, string? gitBranch)
	{
		Version = version;
		GitSha = gitSha;
		GitBranch = gitBranch;
	}

	public string Version { get; }
	public string? GitSha { get; }
	public string? GitBranch { get;}
}