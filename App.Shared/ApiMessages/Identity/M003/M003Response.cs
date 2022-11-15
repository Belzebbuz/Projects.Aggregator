using App.Shared.ApiMessages.Identity.Base;

namespace App.Shared.ApiMessages.Identity.M003;
/// <summary>
/// List of user dto
/// </summary>
public record M003Response(List<UserDto> Users);

