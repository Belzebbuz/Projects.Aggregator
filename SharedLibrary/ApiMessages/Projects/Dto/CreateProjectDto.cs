namespace SharedLibrary.ApiMessages.Projects.Dto;

public class CreateProjectDto
{
	public string Name { get; set; }
	public string Description { get; set; }
	public string SystemRequirements { get; set; }
	public string ExeFileName { get; set; }
	public List<TagDto> Tags { get; set; } = new();
}
