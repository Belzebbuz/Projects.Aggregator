using SharedLibrary.ApiMessages.CommonDTO;

namespace SharedLibrary.ApiMessages.Projects.Dto
{
    public class PatchNoteDto : AuditDto
    {
        public string Text { get; set; }
    }
}