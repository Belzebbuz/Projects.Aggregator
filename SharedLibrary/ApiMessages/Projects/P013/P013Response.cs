using SharedLibrary.ApiMessages.Projects.Dto;

namespace SharedLibrary.ApiMessages.Projects.P013;

/// <summary>
/// Reponse with list of tags
/// </summary>
/// <param name="Tags"></param>
public record P013Response(ICollection<TagDto> Tags);