using Application.Contracts.DI;
using SharedLibrary.Wrapper;

namespace Application.Contracts.Services;

public interface IFileStorageService : ITransientService
{
    void DeleteFiles(IEnumerable<string> filePaths);
    void DeleteSingleFile(string url);
    FileStream DownloadAsync(string url);
    Task<IResult<IZipUploadData>> UploadZipProjectAsync(Guid releaseId, string projectName, string fileName, string exeFile, Stream fileStream);
}

public interface IZipUploadData
{
    public string Version { get; }
    public string GitSha { get; }
    public string GitBranch { get; }
    public string Url { get; }
}

