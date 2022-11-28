using Domain.Base;
using SharedLibrary.Helpers;
using SharedLibrary.Wrapper;
using System.ComponentModel.DataAnnotations;

namespace Domain.Aggregators.ProjectAggregate;

public sealed class Release : AuditableEntity
{
    [MaxLength(10)]
    public string Version { get; private set; }

    [MaxLength(40)]
    public string? GitSha { get; private set; }

    [MaxLength(40)]
    public string? GitBranch { get; private set; }

    [MaxLength(100)]
    public string Url { get; private set; }
    public uint DownloadCount { get; private set; }

    [MaxLength(400)]
    public string? ReleaseNote { get; private set; }

    private Release()
    {
    }

    internal static Release Create(string version, string url, string? gitSha = null, string? gitBranch = null)
    {
        return new()
        {
            Version = version ?? throw new ArgumentNullException(nameof(version)),
            GitSha = gitSha,
            GitBranch = gitBranch,
            Url = url ?? throw new ArgumentNullException(nameof(url)),
        };
    }

    internal IResult SetReleaseNote(string? text)
    {
        ReleaseNote = text;
        return Result.Success();
    }

    internal IResult UpdateReleaseNote(string text)
    {
        ThrowHelper.StringNotEmpty(text);
        ReleaseNote = text;
        return Result.Success();
    }

    internal IResult IncrementDownloadCount()
    {
        DownloadCount++;
        return Result.Success();
    }
}
