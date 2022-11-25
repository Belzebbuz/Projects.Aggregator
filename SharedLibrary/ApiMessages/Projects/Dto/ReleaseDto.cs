using SharedLibrary.ApiMessages.CommonDTO;

namespace SharedLibrary.ApiMessages.Projects.Dto;

public class ReleaseDto : AuditDto
{
    public string Version { get; set; }
    public string GitSha { get; set; }
    public string GitBranch { get; set; }
    public uint DownloadCount { get; set; }
    public string ReleaseNote { get; set; }
}
