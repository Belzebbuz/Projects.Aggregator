using SharedLibrary.ApiMessages.Identity.Base;
using SharedLibrary.ApiMessages.Identity.M001;
using SharedLibrary.ApiMessages.Identity.M004;
using SharedLibrary.ApiMessages.Identity.M005;
using SharedLibrary.ApiMessages.Identity.M008;
using SharedLibrary.Authentication;
using SharedLibrary.Routes;
using SharedLibrary.Wrapper;
using Shouldly;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
namespace Server.Tests.Host;
/// <summary>
/// 1. Самостоятельная регистрация
/// 2. Получить токен пдминистратора и отправить запрос на ендпоинт с требованием к роли Администратор
/// 3. Создать с помощью админа нового пользователя и под новым пользователем отправить запрос на защищенный ендпоинт
/// 4. СОздать нового пользователя попытаться под ним попасть на эндпоинт с требованием к роли Администратор. 
/// Добавить ему роль администратора и попытаться еще раз
/// </summary>
public partial class ApiTests
{
    private readonly M001Request _rootUserCredentials = new("belzebbuz@mail.ru", "qwert1234QW");
    private readonly M005Request _newUser = new("Test", null, "User@user.net", null, "qwerty1234QW", "qwerty1234QW", null);

    [Fact]
    public async Task self_register_should_create_user_and_get_by_id_should_return_posted_user()
    {
        var response = await _client.PostAsJsonAsync(UsersEndpoints.SelfRegister(), _newUser);
        var result = await response.ToResult();
        result.Succeeded.ShouldBeTrue();
        result.Messages.ShouldNotBeNull().ShouldNotBeEmpty();

        var failResponse = await _client.GetAsync(CheckEndpoints.JwtAuth);
        var failResult = await failResponse.ToResult();
        failResult.Succeeded.ShouldBeFalse();

        var tokenResult = await ShouldSuccessGetAccessTokenResult(_newUser.Email, _newUser.Password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResult.Data.Token);
        var successResponse = await _client.GetAsync(CheckEndpoints.JwtAuth);
        var successResult = await successResponse.ToResult();
        successResult.Succeeded.ShouldBeTrue();
    }

    [Fact]
    public async Task login_as_root_user_and_get_list_of_user_should_be_success()
    {
        var rootUserToken = await ShouldSuccessGetAccessTokenResult(_rootUserCredentials.Email, _rootUserCredentials.Password);
        var response = await _client.GetAsync(UsersEndpoints.Base);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = await response.ToPaginatedResult<UserDto>();
        result.Succeeded.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data.ShouldContain(x => x.Email == _rootUserCredentials.Email);
    }

    [Fact]
    public async Task admin_user_should_create_user_and_new_user_should_succsessduly_get_token()
    {
        await ShouldSuccessGetAccessTokenResult(_rootUserCredentials.Email, _rootUserCredentials.Password);
        var response = await _client.PostAsJsonAsync(UsersEndpoints.Base, _newUser);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = await response.ToResult();
        result.Succeeded.ShouldBeTrue();

        await ShouldSuccessGetAccessTokenResult(_newUser.Email, _newUser.Password);
        var successResponse = await _client.GetAsync(CheckEndpoints.JwtAuth);
        var successResult = await successResponse.ToResult();
        successResult.Succeeded.ShouldBeTrue();
    }

    [Fact]
    public async Task create_user_and_get_request_should_not_be_success_then_grant_admin_role_should_fix_that()
    {
        await ShouldSuccessGetAccessTokenResult(_rootUserCredentials.Email, _rootUserCredentials.Password);
        var response = await _client.PostAsJsonAsync(UsersEndpoints.Base, _newUser);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = await response.ToResult();
        result.Succeeded.ShouldBeTrue();
        result.Messages.ShouldNotBeEmpty();
        var newUserId = result.Messages.First();

        await ShouldSuccessGetAccessTokenResult(_newUser.Email, _newUser.Password);
        var listUsersRequest = await _client.GetAsync(UsersEndpoints.Base);
        var listUsersResult = await listUsersRequest.ToResult();
        listUsersResult.Succeeded.ShouldBeFalse();



        await ShouldSuccessGetAccessTokenResult(_rootUserCredentials.Email, _rootUserCredentials.Password);

        var getRolesRequest = await _client.GetAsync(UsersEndpoints.GetUserRoles(newUserId));
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var getRolesResult = await getRolesRequest.ToResult<M004Response>();
        getRolesResult.Succeeded.ShouldBeTrue();
        getRolesResult.Data.Roles.ShouldNotBeEmpty();

        getRolesResult.Data.Roles.FirstOrDefault(x => x.RoleName == SHRoles.Admin).Enabled = true;
        var asignRolesRequest = new M008Request(newUserId, getRolesResult.Data.Roles);
        var assignRolesResponse = await _client.PostAsJsonAsync(UsersEndpoints.AssignRoles(), asignRolesRequest);
        assignRolesResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        var assignRolesResult = await assignRolesResponse.ToResult();
        assignRolesResult.Succeeded.ShouldBeTrue();

        await ShouldSuccessGetAccessTokenResult(_newUser.Email, _newUser.Password);
        var newListUsersRequest = await _client.GetAsync(UsersEndpoints.Base);
        var newListUsersResult = await newListUsersRequest.ToResult();
        newListUsersResult.Succeeded.ShouldBeTrue();
    }

    private async Task<IResult<M001Response>> ShouldSuccessGetAccessTokenResult(string email, string password)
    {
        var tokenRequest = new M001Request(email, password);
        var tokenResponse = await _client.PostAsJsonAsync(TokenEndpoints.Base, tokenRequest);
        tokenResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        var tokenResult = await tokenResponse.ToResult<M001Response>();
        tokenResult.Succeeded.ShouldBeTrue();
        tokenResult.Data.ShouldNotBeNull();
        tokenResult.Data.Token.ShouldNotBeNull().ShouldNotBeNullOrWhiteSpace();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResult.Data.Token);
        return tokenResult;
    }
}

