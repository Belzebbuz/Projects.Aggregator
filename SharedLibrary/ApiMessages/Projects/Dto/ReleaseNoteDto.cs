using SharedLibrary.ApiMessages.CommonDTO;

namespace SharedLibrary.ApiMessages.Projects.Dto;

public class ReleaseNoteDto : AuditDto
{
    public string Text { get; set; }
}
