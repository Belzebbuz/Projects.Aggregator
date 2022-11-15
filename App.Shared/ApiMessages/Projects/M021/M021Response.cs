using App.Shared.ApiMessages.Projects.Dto;

namespace App.Shared.ApiMessages.Projects.M021;

/// <summary>
/// Reponse with list of tags
/// </summary>
/// <param name="Tags"></param>
public record M021Response(ICollection<TagDto> Tags);