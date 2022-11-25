using Application.Contracts.DI;
using Microsoft.AspNetCore.Http;
using SharedLibrary.Wrapper;
using System.Security;

namespace Application.Contracts.Services;

public interface IFileStorageService : IScopedService
{
    void DeleteFiles(IEnumerable<string> filePaths);
    void DeleteSingleFile(string url);
    FileStream DownloadAsync(string url);

    /// <summary>
    /// Return file path of saved file
    /// </summary>
    /// <param name="folder"></param>
    /// <returns></returns>
    Task<IResult<string>> SaveFileStreamingAsync(string folder);

    Task<IResult<IExeFileVersionInfo>> GetExeFileVersionAsync(string zipFilePath, string exeFileName);
}

public interface IExeFileVersionInfo 
{
	public string Version { get; }
	public string? GitSha { get; }
	public string? GitBranch { get; }
}

