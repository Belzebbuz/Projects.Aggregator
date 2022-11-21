using Application.Contracts.Identity;
using Application.Contracts.Services.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using SharedLibrary.ApiMessages.Identity.Base;
using SharedLibrary.ApiMessages.Identity.ID004;
using SharedLibrary.ApiMessages.Identity.ID005;
using SharedLibrary.ApiMessages.Identity.ID006;
using SharedLibrary.ApiMessages.Identity.ID007;
using SharedLibrary.ApiMessages.Identity.ID009;
using SharedLibrary.ApiMessages.Identity.ID008;
using SharedLibrary.Authentication;
using SharedLibrary.Routes;
using SharedLibrary.Wrapper;
using IResult = SharedLibrary.Wrapper.IResult;
using SharedLibrary.ApiMessages.Identity.ID010;
using Host.Controllers.Common;

namespace Host.Controllers.Identity
{
	[Route(UsersEndpoints.Base)]
	[ApiController]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class UsersController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly ICurrentUser _currentUser;

		public UsersController(IUserService userService, ICurrentUser currentUser)
		{
			_userService = userService;
			_currentUser = currentUser;
		}

		[HttpGet]
		[OpenApiOperation("Получить список пользователей","Постраничный вывод списка пользователей.\n" + 
			OpenApiDescriptonConstants.PerPageDescription)]
		[Authorize(Roles = SHRoles.Admin)]
		public async Task<PaginatedResult<UserDto>> GetListAsync()
			=> await _userService.GetListAsync();

		[HttpPost("search")]
		[Authorize(Roles = SHRoles.Admin)]
		[OpenApiOperation("Список пользователей по фильтру.", OpenApiDescriptonConstants.PerPageDescription)]
		public async Task<PaginatedResult<UserDto>> GetListAsync(ID010Request request)
			=> await _userService.GetListByFilterAsync(request);

		[HttpGet("{id}")]
		[OpenApiOperation("Подробное данные пользователя", "")]
		public async Task<IResult<ID007Response>> GetByIdAsync(string id)

			=> await _userService.GetAsync(id);

		[HttpDelete("{id}")]
		[OpenApiOperation("Удаление пользователя", "")]
		[Authorize(Roles = SHRoles.Admin)]
		public async Task<IResult> DeleteByIdAsync(string id)
			=> await _userService.DeleteAsync(id);

		[HttpGet("{id}/roles")]
		[OpenApiOperation("Получение ролей пользователя", "")]
		[Authorize(Roles = SHRoles.Admin)]
		public async Task<IResult<ID004Response>> GetRolesAsync(string id)
			=> await _userService.GetRolesAsync(id);

		[HttpPost]
		[OpenApiOperation("Создание пользователя администратором системы", "")]
		[Authorize(Roles = SHRoles.Admin)]
		public async Task<IResult> CreateAsync(ID005Request request)
		=> await _userService.CreateAsync(request, GetOriginFromRequest());

		[HttpPost("self-register")]
		[AllowAnonymous]
		[OpenApiOperation("Самостоятельная регистрация", "")]
		public async Task<IResult> SelfRegisterAsync(ID005Request request)
			=> await _userService.CreateAsync(request, GetOriginFromRequest());

		[HttpPost("toggle-status")]
		[Authorize(Roles = SHRoles.Admin)]
		[OpenApiOperation("Запретить пользователю проходить авторизацию", "")]
		public async Task<IResult> ToggleUserStatus(ID006Request request)
			=> await _userService.ToggleStatusAsync(request);

		[HttpPost("roles")]
		[OpenApiOperation("Изменить состояние ролей пользователя", "")]
		[Authorize(Roles = SHRoles.Admin)]
		public async Task<IResult> AssignRolesAsync(ID008Request request)
			=> await _userService.AssignRolesAsync(request);

		[HttpPost("change-password")]
		[OpenApiOperation("Изменить пароль", "")]
		public async Task<IResult> ChangePasswordAsync(ID009Request request)
			=> await _userService.ChangePasswordAsync(request, _currentUser.GetUserId().ToString());

		private string GetOriginFromRequest() => $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";
	}
}
