using SharedLibrary.ApiMessages.Identity.Base;

namespace SharedLibrary.ApiMessages.Identity.ID003;
/// <summary>
/// List of user dto
/// </summary>
public record ID003Response(List<UserDto> Users);

