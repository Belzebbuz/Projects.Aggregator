namespace App.Shared.ApiMessages.Projects.Dto;

public class ProjectShortDto : AuditDto
{
	public string Name { get; set; }
	public string Description { get; set; }
	public ICollection<TagDto> Tags { get; set; }
}
