using Application.Contracts.Services;

namespace Infrastructure.Common.Services.FileStorage;

internal class ZipUploadDataResult : IZipUploadData
{
    public string Version { get; }

    public string GitSha { get; }

    public string GitBranch { get; }

    public string Url { get; }

    internal ZipUploadDataResult(string version, string gitSha, string gitBranch, string url)
    {
        Version = version;
        GitSha = gitSha;
        GitBranch = gitBranch;
        Url = url;
    }
}
