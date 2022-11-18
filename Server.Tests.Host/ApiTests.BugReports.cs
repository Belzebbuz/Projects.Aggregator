using SharedLibrary.ApiMessages.BugReports.BG001;
using SharedLibrary.ApiMessages.BugReports.Dto;
using SharedLibrary.Routes;
using SharedLibrary.Wrapper;
using Shouldly;
using System.Net.Http.Json;

namespace Server.Tests.Host;

public partial class ApiTests
{
    [Fact]
    public async Task should_successfuly_create_bug_report()
    {
		await ShouldSuccessGetAccessTokenResult(_rootUserCredentials.Email, _rootUserCredentials.Password);

        var response = await _client.PostAsJsonAsync(BugEndpoints.Base, new BG001Request() { Text = "there is a lot trubbles" });
        var result = await response.ToResult();
        result.Succeeded.ShouldBeTrue();

        var getResponse = await _client.GetAsync(BugEndpoints.Base);
        var getResult = await getResponse.ToPaginatedResult<BugReportDto>();
		getResult.Succeeded.ShouldBeTrue();
        getResult.HasNextPage.ShouldBeFalse();
        getResult.TotalCount.ShouldBe(1);
        getResult.TotalPages.ShouldBe(1);
        getResult.Data.Count.ShouldBe(1);
        getResult.Data.First().Text.ShouldBe("there is a lot trubbles");
	}
}
