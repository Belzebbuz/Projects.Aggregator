using Application.Contracts.Services;
using Application.Contracts.Services.Events;
using Application.Contracts.Services.Identity;
using Domain.Base.Events.Identity;
using Infrastructure.Security;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using SharedLibrary.ApiMessages.Identity.Base;
using SharedLibrary.ApiMessages.Identity.M004;
using SharedLibrary.ApiMessages.Identity.M005;
using SharedLibrary.ApiMessages.Identity.M006;
using SharedLibrary.ApiMessages.Identity.M007;
using SharedLibrary.ApiMessages.Identity.M008;
using SharedLibrary.Authentication;
using SharedLibrary.Wrapper;

namespace Infrastructure.Identity.Services;

internal class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IStringLocalizer<UserService> _localizer;
    private readonly IEventPublisher _events;
    private readonly IPaginationService _paginationService;
    private readonly SecuritySettings _securitySettings;

    public UserService(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IStringLocalizer<UserService> localizer,
        IEventPublisher events,
        IOptions<SecuritySettings> securityOptions,
        IPaginationService paginationService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _localizer = localizer;
        _events = events;
        _paginationService = paginationService;
        _securitySettings = securityOptions.Value;
    }
    public async Task<IResult> AssignRolesAsync(M008Request request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
            return await Result<M007Response>.FailAsync(_localizer["identity.usernotfound"]);

        if (user.Email == _securitySettings.RootUserEmail
            && request.Roles.Any(a => !a.Enabled && a.RoleName == SHRoles.Admin))
        {
            await Result.FailAsync(_localizer["identity.failrootuserdisableadmin"]);
        }

        foreach (var userRole in request.Roles)
        {
            if (await _roleManager.FindByNameAsync(userRole.RoleName) is not null)
            {
                if (userRole.Enabled)
                {
                    if (!await _userManager.IsInRoleAsync(user, userRole.RoleName))
                    {
                        await _userManager.AddToRoleAsync(user, userRole.RoleName);
                    }
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, userRole.RoleName);
                }
            }
        }

        await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id, true));

        return await Result.SuccessAsync(_localizer["User Roles Updated Successfully."]);
    }

    public async Task<IResult> CreateAsync(M005Request request, string origin)
    {
        var user = AppUser.Create(request.Email, request.UserName, request.FirstName, request.LastName, request.PhoneNumber);
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return await Result.FailAsync(messages: result.GetErrors(_localizer));

        await _userManager.AddToRoleAsync(user, SHRoles.Basic);
        await _events.PublishAsync(new ApplicationUserCreatedEvent(user.Id));
        return await Result.SuccessAsync(user.Id);
    }

    public async Task<IResult<M007Response>> GetAsync(string id)
    {
        var user = await _userManager.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Id == id);

        if (user == null)
            return await Result<M007Response>.FailAsync(_localizer["identity.usernotfound"]);

        return await Result<M007Response>.SuccessAsync(data: user.Adapt<M007Response>());
    }

    public async Task<PaginatedResult<UserDto>> GetListAsync()
    {
        var count = await _userManager.Users.CountAsync();
        var pagination = _paginationService.Calculate(count);
        var users = await _userManager.Users
            .AsNoTracking()
            .OrderBy(x => x.Email)
            .Paginate(pagination)
            .ToListAsync();
        return PaginatedResult<UserDto>.Success(users.Adapt<List<UserDto>>(), count, pagination.Page, pagination.RecordsPerPage);
    }

    public async Task<IResult<M004Response>> GetRolesAsync(string id)
    {
        var userRoles = new List<UserRoleDto>();

        var user = await _userManager.FindByIdAsync(id);
        var roles = await _roleManager.Roles.AsNoTracking().ToListAsync();
        foreach (var role in roles)
        {
            userRoles.Add(new UserRoleDto
            {
                RoleId = role.Id,
                RoleName = role.Name,
                Enabled = await _userManager.IsInRoleAsync(user, role.Name)
            });
        }

        return await Result<M004Response>.SuccessAsync(data: new(userRoles));
    }

    public async Task<IResult> ToggleStatusAsync(M006Request request)
    {
        var user = await _userManager.Users
            .SingleOrDefaultAsync(u => u.Id == request.UserId);
        if (user == null)
            return await Result<M007Response>.FailAsync(_localizer["identity.usernotfound"]);
        bool isAdmin = await _userManager.IsInRoleAsync(user, SHRoles.Admin);
        if (isAdmin)
            return await Result.FailAsync(_localizer["identity.failtoggleadmin"]);

        user.ToogleUserStatus(request.IsActive);
        await _userManager.UpdateAsync(user);
        await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id));
        return await Result.SuccessAsync();
    }
}
