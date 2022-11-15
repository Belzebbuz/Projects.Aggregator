using SharedLibrary.ApiMessages.Projects.Dto;

namespace SharedLibrary.ApiMessages.Projects.M021;

/// <summary>
/// Reponse with list of tags
/// </summary>
/// <param name="Tags"></param>
public record M021Response(ICollection<TagDto> Tags);