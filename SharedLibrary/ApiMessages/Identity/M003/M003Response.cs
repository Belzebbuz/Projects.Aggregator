using SharedLibrary.ApiMessages.Identity.Base;

namespace SharedLibrary.ApiMessages.Identity.M003;
/// <summary>
/// List of user dto
/// </summary>
public record M003Response(List<UserDto> Users);

