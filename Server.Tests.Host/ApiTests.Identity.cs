using SharedLibrary;
using SharedLibrary.ApiMessages.Identity.Base;
using SharedLibrary.ApiMessages.Identity.ID001;
using SharedLibrary.ApiMessages.Identity.ID004;
using SharedLibrary.ApiMessages.Identity.ID005;
using SharedLibrary.ApiMessages.Identity.ID006;
using SharedLibrary.ApiMessages.Identity.ID008;
using SharedLibrary.ApiMessages.Identity.ID009;
using SharedLibrary.ApiMessages.Identity.ID010;
using SharedLibrary.Authentication;
using SharedLibrary.Routes;
using SharedLibrary.Wrapper;
using Shouldly;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
namespace Server.Tests.Host;

public partial class ApiTests
{
	private readonly ID001Request _rootUserCredentials = new("belzebbuz@mail.ru", "qwert1234QW");
	private readonly ID005Request _newUser = new("Test", null, "User@user.net", null, "qwerty1234QW", "qwerty1234QW", null);
	private readonly ID005Request _newUser1 = new("TestUser1", null, "User1@user.net", null, "qwerty1234QW", "qwerty1234QW", null);
	private readonly ID005Request _newUser2 = new("TestTestUser2", null, "User2@user.net", null, "qwerty1234QW", "qwerty1234QW", null);

	[Fact]
	public async Task self_register_should_create_user_and_get_by_id_should_return_posted_user()
	{
		var response = await _client.PostAsJsonAsync(UsersEndpoints.GetSelfRegisterRoute(), _newUser);
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

		var getRolesRequest = await _client.GetAsync(UsersEndpoints.GetUserRolesRoute(newUserId));
		response.StatusCode.ShouldBe(HttpStatusCode.OK);
		var getRolesResult = await getRolesRequest.ToResult<ID004Response>();
		getRolesResult.Succeeded.ShouldBeTrue();
		getRolesResult.Data.Roles.ShouldNotBeEmpty();

		getRolesResult.Data.Roles.FirstOrDefault(x => x.RoleName == SHRoles.Admin).Enabled = true;
		var asignRolesRequest = new ID008Request(newUserId, getRolesResult.Data.Roles);
		var assignRolesResponse = await _client.PostAsJsonAsync(UsersEndpoints.GetRolesRoute(), asignRolesRequest);
		assignRolesResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
		var assignRolesResult = await assignRolesResponse.ToResult();
		assignRolesResult.Succeeded.ShouldBeTrue();

		await ShouldSuccessGetAccessTokenResult(_newUser.Email, _newUser.Password);
		var newListUsersRequest = await _client.GetAsync(UsersEndpoints.Base);
		var newListUsersResult = await newListUsersRequest.ToResult();
		newListUsersResult.Succeeded.ShouldBeTrue();
	}

	[Fact]
	public async Task change_root_user_password_chould_succfuly_get_token_with_new_password()
	{
		await ShouldSuccessGetAccessTokenResult(_rootUserCredentials.Email, _rootUserCredentials.Password);

		var changeTokenRequest = new ID009Request()
		{
			OldPassword = _rootUserCredentials.Password,
			NewPassword = "qwerty134QWER",
			ConfirmNewPassword = "qwerty134QWER"
		};

		var response = await _client.PostAsJsonAsync(UsersEndpoints.GetChangePasswordRoute(), changeTokenRequest);
		response.StatusCode.ShouldBe(HttpStatusCode.OK);
		var requestResult = await response.ToResult();
		requestResult.Succeeded.ShouldBeTrue();

		await ShouldSuccessGetAccessTokenResult(_rootUserCredentials.Email, "qwerty134QWER");
	}

	[Fact]
	public async Task create_two_users_login_as_admin_get_users_by_page_should_be_success()
	{
		var createUserResponse = await _client.PostAsJsonAsync(UsersEndpoints.GetSelfRegisterRoute(), _newUser);
		var createUserResult = await createUserResponse.ToResult();
		createUserResult.Succeeded.ShouldBeTrue();
		var createUser1Response = await _client.PostAsJsonAsync(UsersEndpoints.GetSelfRegisterRoute(), _newUser1);
		var createUser1Result = await createUser1Response.ToResult();
		createUser1Result.Succeeded.ShouldBeTrue();
		var createUser2Response = await _client.PostAsJsonAsync(UsersEndpoints.GetSelfRegisterRoute(), _newUser2);
		var createUser2Result = await createUser2Response.ToResult();
		createUser2Result.Succeeded.ShouldBeTrue();

		await ShouldSuccessGetAccessTokenResult(_rootUserCredentials.Email, _rootUserCredentials.Password);

		_client.DefaultRequestHeaders.Add(Headers.Page, "1");
		_client.DefaultRequestHeaders.Add(Headers.ItemsPerPage, "2");
		var filterBySingleUserResponse = await _client.PostAsJsonAsync(UsersEndpoints.GetUserByFilterRoute(), new ID010Request(_newUser.Email));
		filterBySingleUserResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
		var filterBySingleEmailResult = await filterBySingleUserResponse.ToPaginatedResult<UserDto>();
		filterBySingleEmailResult.Succeeded.ShouldBeTrue();
		filterBySingleEmailResult.Data.Count.ShouldBe(1);
		filterBySingleEmailResult.TotalPages.ShouldBe(1);
		filterBySingleEmailResult.HasNextPage.ShouldBeFalse();

		var filterByUserEmailResponse = await _client.PostAsJsonAsync(UsersEndpoints.GetUserByFilterRoute(), new ID010Request("user"));
		filterByUserEmailResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
		var filterByUserEmailResult = await filterByUserEmailResponse.ToPaginatedResult<UserDto>();
		filterByUserEmailResult.Succeeded.ShouldBeTrue();
		filterByUserEmailResult.Data.Count.ShouldBe(2);
		filterByUserEmailResult.TotalPages.ShouldBe(2);
		filterByUserEmailResult.TotalCount.ShouldBe(3);
		filterByUserEmailResult.HasNextPage.ShouldBeTrue();
		filterByUserEmailResult.HasPreviousPage.ShouldBeFalse();


		var filterEmptyNameResponse = await _client.PostAsJsonAsync(UsersEndpoints.GetUserByFilterRoute(), new ID010Request(""));
		filterEmptyNameResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
		var filterByEmptyResult = await filterEmptyNameResponse.ToPaginatedResult<UserDto>();
		filterByEmptyResult.Succeeded.ShouldBeTrue();
		filterByEmptyResult.Data.Count.ShouldBe(2);
		filterByEmptyResult.TotalPages.ShouldBe(2);
		filterByEmptyResult.TotalCount.ShouldBe(4);
		filterByEmptyResult.HasNextPage.ShouldBeTrue();
		filterByEmptyResult.HasPreviousPage.ShouldBeFalse();
	}

	[Fact]
	public async Task create_new_user_then_toggle_his_isActive_should_be_success()
	{
		var createUserResponse = await _client.PostAsJsonAsync(UsersEndpoints.GetSelfRegisterRoute(), _newUser);
		var createUserResult = await createUserResponse.ToResult();
		createUserResult.Succeeded.ShouldBeTrue();
		await ShouldSuccessGetAccessTokenResult(_rootUserCredentials.Email, _rootUserCredentials.Password);

		var getUsersResponse = await _client.GetAsync(UsersEndpoints.Base);
		var getUsersResult = await getUsersResponse.ToPaginatedResult<UserDto>();
		getUsersResult.Succeeded.ShouldBeTrue();
		var userId = getUsersResult.Data.FirstOrDefault(x => x.Email == _newUser.Email).Id;
		var toggleResponse = await _client.PostAsJsonAsync(UsersEndpoints.GetToggleUserStatusRoute(), new ID006Request(userId, false));
		var toggleResult = await toggleResponse.ToResult();
		toggleResult.Succeeded.ShouldBeTrue();
	}
	private async Task<IResult<ID001Response>> ShouldSuccessGetAccessTokenResult(string email, string password)
	{
		var tokenRequest = new ID001Request(email, password);
		var tokenResponse = await _client.PostAsJsonAsync(TokenEndpoints.Base, tokenRequest);
		tokenResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
		var tokenResult = await tokenResponse.ToResult<ID001Response>();
		tokenResult.Succeeded.ShouldBeTrue();
		tokenResult.Data.ShouldNotBeNull();
		tokenResult.Data.Token.ShouldNotBeNull().ShouldNotBeNullOrWhiteSpace();
		_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResult.Data.Token);
		return tokenResult;
	}
}

