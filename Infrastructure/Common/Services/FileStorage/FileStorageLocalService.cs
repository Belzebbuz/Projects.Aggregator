using Application.Contracts.Services;
using SharedLibrary.Helpers;
using SharedLibrary.Wrapper;
using System.Diagnostics;
using System.IO.Compression;

namespace Infrastructure.Common.Services.FileStorage;

internal class FileStorageLocalService : IFileStorageService
{
    private const string RootProjectsFolder = "Files\\Projects";
    private readonly string _tempFolder = $"Files\\Temp\\{Guid.NewGuid()}";

    public void DeleteFiles(IEnumerable<string> filePaths)
    {
        foreach (var url in filePaths)
        {
            if (File.Exists(url))
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

    public async Task<IResult<IZipUploadData>> UploadZipProjectAsync(Guid releaseId, string projectName, string fileName, string exeFile, Stream fileStream)
    {
        try
        {
            if (Path.GetExtension(fileName) != ".zip")
                return Result<IZipUploadData>.Fail("Archive must be .zip!");

            string projectFolder = Path.Combine(RootProjectsFolder, releaseId.ToString());

            if (!Directory.Exists(projectFolder))
                Directory.CreateDirectory(projectFolder);

            if (!Directory.Exists(_tempFolder))
                Directory.CreateDirectory(_tempFolder);

            using var zipArchive = new ZipArchive(fileStream);
            var exefile = zipArchive.Entries.SingleOrDefault(x => x.Name.Contains(exeFile));
            if (exefile == null)
                return await Result<IZipUploadData>.FailAsync($"Exe file：{exeFile} not found!");

            var tempExeFile = Path.Combine(_tempFolder, Path.GetFileName(exefile.Name));
            exefile.ExtractToFile(tempExeFile);
            var fileVersion = FileVersionInfo.GetVersionInfo(tempExeFile).FileVersion;
            var productVersion = FileVersionInfo.GetVersionInfo(tempExeFile).ProductVersion;
            var gitBranch = productVersion.Contains("Branch")
                ? productVersion.Split("Branch")[1].Split("Sha")[0].Replace(".", "")
                : String.Empty;
            var gitSha = productVersion.Contains("Sha")
                ? productVersion.Split("Sha")[1].Replace(".", "")
                : String.Empty;
            Directory.Delete(_tempFolder, true);

            string filePath = Path.Combine(projectFolder, $"{Guid.NewGuid().ToString()}{Path.GetExtension(fileName)}");
            await using var fs = File.Create(filePath);

            await fileStream.CopyToAsync(fs);
            await fs.FlushAsync();
            return Result<IZipUploadData>.Success(new ZipUploadDataResult(fileVersion, gitSha, gitBranch, filePath));
        }
        finally
        {
            fileStream?.Dispose();
        }

    }
}
