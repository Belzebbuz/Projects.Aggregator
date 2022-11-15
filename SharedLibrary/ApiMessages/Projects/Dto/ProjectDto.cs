namespace SharedLibrary.ApiMessages.Projects.Dto;

public class ProjectDto : AuditDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string SystemRequirements { get; set; }
    public string ExeFileName { get; set; }
    public ICollection<ReleaseDto> Releases { get; set; }
    public ICollection<TagDto> Tags { get; set; }
}
