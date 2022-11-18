using App.Shared.ApiMessages.Projects.P007;
using Domain.Aggregators.Project;
using SharedLibrary;
using SharedLibrary.ApiMessages.Projects.Dto;
using SharedLibrary.ApiMessages.Projects.P001;
using SharedLibrary.ApiMessages.Projects.P003;
using SharedLibrary.ApiMessages.Projects.P011;
using SharedLibrary.ApiMessages.Projects.P012;
using SharedLibrary.ApiMessages.Projects.P013;
using SharedLibrary.ApiMessages.Projects.P016;
using SharedLibrary.Routes;
using SharedLibrary.Wrapper;
using Shouldly;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Server.Tests.Host;

[ExcludeFromCodeCoverage]
public partial class ApiTests : IDisposable
{
    #region Init HostedApp
    private readonly HostTestApp _app;
    private readonly HttpClient _client;
    private readonly string _downloadPath = "Downloads";
    private readonly string _uploadPath = "Files";

    private readonly string _firstReleaseFilePath = "English-test-bot-1.zip";
    private readonly string _secondReleaseFilePath = "English-test-bot-2.zip";
    public ApiTests()
    {
        _app = new HostTestApp();
        _client = _app.CreateClient();
        if (!Directory.Exists(_downloadPath))
            Directory.CreateDirectory(_downloadPath);
        if (!Directory.Exists(_uploadPath))
            Directory.CreateDirectory(_uploadPath);
    }
    #endregion

    [Fact]
    public async void get_check_endpoint_should_return_ok()
    {
        await ShouldSuccessGetAccessTokenResult(_rootUserCredentials.Email, _rootUserCredentials.Password);
        var response = await _client.GetAsync(CheckEndpoints.Base);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var content = await response.ToResult();
        content.ShouldNotBeNull();
        content.Succeeded.ShouldBeTrue();
        content.Messages.ShouldNotBeNull();
    }

    [Fact]
    public async Task post_new_project_should_successfuly_return_get_response()
    {
        await ShouldSuccessGetAccessTokenResult(_rootUserCredentials.Email, _rootUserCredentials.Password);
        var tags = await GetDataBaseTags();
        var webTag = tags.Where(x => x.Value == "Android").ToList();
        var newAppId = await ShouldSuccessCreateNewProject("Trace-X web app", "Cool app", "Windows 10", "TXLineService.exe", webTag);

        var getAppResponse = await _client.GetAsync(ProjectsEndpoints.GetProjectRoute(Guid.Parse(newAppId)));
        getAppResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        var getAppResult = await getAppResponse.ToResult<P001Response>();
        getAppResult.Succeeded.ShouldBeTrue();
        getAppResult.Data.Name.ShouldBe("Trace-X web app");
        getAppResult.Data.CreatedByEmail.ShouldBe(_rootUserCredentials.Email);
    }

    [Fact]
    public async Task create_two_app_with_the_same_name_should_be_failed()
    {
        await ShouldSuccessGetAccessTokenResult(_rootUserCredentials.Email, _rootUserCredentials.Password);
        var tags = await GetDataBaseTags();
        var webTag = tags.Where(x => x.Value == "Web").ToList();
        var appId = await ShouldSuccessCreateNewProject("Trace-X web app", "Cool app",
            "Windows 10", "TXService.exe", webTag);
        var request = new P003Request("Trace-X web app", "Cool app", "Windows 10", "TXService.exe", webTag);
        var response = await _client.PostAsJsonAsync(ProjectsEndpoints.Base, request);
        var content = await response.ToResult();
        content.Succeeded.ShouldBeFalse();
    }

    [Fact]
    public async Task create_two_app_with_the_same_name_should_be_bad()
    {
        await ShouldSuccessGetAccessTokenResult(_rootUserCredentials.Email, _rootUserCredentials.Password);
        var tags = await GetDataBaseTags();
        var tagsIds = tags.Select(x => x.Id).ToList();
        await ShouldSuccessCreateNewProject("Trace-X web app", "Cool app", "Windows 10", "TXService.exe", tags);
        var request = new P003Request("Trace-X web app", "Cool app", "Windows 10", "TXService.exe", tags.ToList());
        var response = await _client.PostAsJsonAsync(ProjectsEndpoints.Base, request);
        var content = await response.ToResult();
        content.Succeeded.ShouldBeFalse();
        content.Messages.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task create_project_and_add_release_download_that_release_should_be_ok()
    {
        //Create
        await ShouldSuccessGetAccessTokenResult(_rootUserCredentials.Email, _rootUserCredentials.Password);
        var tags = await GetDataBaseTags();
        var webTag = tags.Where(x => x.Value == "Test").ToList();
        var appId = await ShouldSuccessCreateNewProject("English test app", "Telegram bot web api app", "Windows 10", "Client.exe", webTag);

        //Upload release
        var response = await UploadRelease(appId, _firstReleaseFilePath);

        var content = await response.ToResult();
        content.Succeeded.ShouldBeTrue();
        content.Messages.FirstOrDefault().ShouldNotBeNull();
        var releaseId = content.Messages.FirstOrDefault();

        //Get project and find created release
        var projectRequest = await _client.GetAsync(ProjectsEndpoints.GetProjectRoute(Guid.Parse(appId)));
        var projectResult = await projectRequest.ToResult<P001Response>();
        projectResult.Succeeded.ShouldBeTrue();
        projectResult.Data.ShouldNotBeNull();
        var uploadedRelease = projectResult.Data.Releases.SingleOrDefault(x => x.Id == Guid.Parse(releaseId));
        uploadedRelease.ShouldNotBeNull();
        uploadedRelease.CreatedByEmail.ShouldBe(_rootUserCredentials.Email);

        //Download uploaded file
        var downloadRequest = await _client.GetStreamAsync(ProjectsEndpoints.GetSingleReleaseRoute(Guid.Parse(appId), uploadedRelease.Id));
        if (!Directory.Exists(_downloadPath))
            Directory.CreateDirectory(_downloadPath);
        var downloadFileName = Path.Combine(_downloadPath, $"{projectResult.Data.Name}.zip");
        await using (var fs = File.OpenWrite(downloadFileName))
        {
            await downloadRequest.CopyToAsync(fs);
        }
        File.Exists(downloadFileName).ShouldBeTrue();
        File.Delete(downloadFileName);
    }

    [Fact]
    public async Task after_create_two_release_should_get_single_project_should_return_project_with_two_releases()
    {
        await ShouldSuccessGetAccessTokenResult(_rootUserCredentials.Email, _rootUserCredentials.Password);
        var tags = await GetDataBaseTags();
        var testTag = tags.Where(x => x.Value == "Test").ToList();
        var appId = await ShouldSuccessCreateNewProject("English test app", "Telegram bot web api app", "Windows 10", "Client.exe", testTag);

        var firstRelease = await UploadRelease(appId, _firstReleaseFilePath);
        var secondRelease = await UploadRelease(appId, _firstReleaseFilePath);

        var projectRequest = await _client.GetAsync(ProjectsEndpoints.GetProjectRoute(Guid.Parse(appId)));
        var projectResult = await projectRequest.ToResult<P001Response>();
        projectResult.Succeeded.ShouldBeTrue();
        projectResult.Data.ShouldNotBeNull();
        projectResult.Data.Releases.Count.ShouldBe(2);
    }

    [Fact]
    public async Task after_add_four_releases_should_get_project_should_return_only_three_last_releases()
    {
        await ShouldSuccessGetAccessTokenResult(_rootUserCredentials.Email, _rootUserCredentials.Password);
        var tags = await GetDataBaseTags();
        var testTag = tags.Where(x => x.Value == "Test").ToList();
        var appId = await ShouldSuccessCreateNewProject("English test app", "Telegram bot web api app", "Windows 10", "Client.exe", testTag);

        var firstRelease = await UploadRelease(appId, _firstReleaseFilePath);
        var firstReleaseResult = await firstRelease.ToResult();
        var secondRelease = await UploadRelease(appId, _firstReleaseFilePath);
        var thirdRelease = await UploadRelease(appId, _secondReleaseFilePath);
        var fourthRelease = await UploadRelease(appId, _secondReleaseFilePath);

        var projectRequest = await _client.GetAsync(ProjectsEndpoints.GetProjectRoute(Guid.Parse(appId)));
        var projectResult = await projectRequest.ToResult<P001Response>();
        projectResult.Succeeded.ShouldBeTrue();
        projectResult.Data.ShouldNotBeNull();
        projectResult.Data.Releases.Count.ShouldBe(3);
        projectResult.Data.Releases.SingleOrDefault(x => x.Id == Guid.Parse(firstReleaseResult.Messages.First())).ShouldBeNull();
    }

    [Fact]
    public async Task add_five_projects_and_filter_by_name_any_case_should_be_ok()
    {
        await ShouldSuccessGetAccessTokenResult(_rootUserCredentials.Email, _rootUserCredentials.Password);
        var tags = await GetDataBaseTags();
        var testTag = tags.Where(x => x.Value == "Test").ToList();
        var webTag = tags.Where(x => x.Value == "Web").ToList();
        await ShouldSuccessCreateNewProject("English app", "Help study english service", "Linux/Windows 10",
            "Client.exe", testTag);
        
        await ShouldSuccessCreateNewProject("Line tsd app", "Work as line", "Android",
            "Client.exe", webTag);

		await ShouldSuccessCreateNewProject("English app mobile", "Help study english mobile app", "Android",
			"Client.exe", testTag);
		var filterRequest = new P012Request("Engl");
        var filterResponse = await _client.PostAsJsonAsync(ProjectsEndpoints.GetProjectFilterRoute(), filterRequest);
        var filterResult = await filterResponse.ToPaginatedResult<ProjectShortDto>();
        filterResult.Succeeded.ShouldBeTrue();
        filterResult.Data.ShouldNotBeNull();
        filterResult.Data.Count.ShouldBe(2);
        filterResult.Data[0].Tags.ShouldNotBeEmpty();


        var filterMobileRequest = new P012Request("English app m");
        var filterMobileResponse = await _client.PostAsJsonAsync(ProjectsEndpoints.GetProjectFilterRoute(), filterMobileRequest);
        var filterMobileResult = await filterMobileResponse.ToPaginatedResult<ProjectShortDto>();
        filterMobileResult.Succeeded.ShouldBeTrue();
        filterMobileResult.Data.ShouldNotBeNull();
        filterMobileResult.Data.Count.ShouldBe(1);

        var filterTsdRequest = new P012Request("line");
        var filterTsdResponse = await _client.PostAsJsonAsync(ProjectsEndpoints.GetProjectFilterRoute(), filterTsdRequest);
        var filterTsdResult = await filterTsdResponse.ToPaginatedResult<ProjectShortDto>();
        filterTsdResult.Succeeded.ShouldBeTrue();
        filterTsdResult.Data.ShouldNotBeNull();
        filterTsdResult.Data.Count.ShouldBe(1);

    }

    [Fact]
    public async Task add_nine_projects_paginated_request_by_five_should_return_list_of_five_items_next_page_should_be_true_count_should_be_10()
    {
        await ShouldSuccessGetAccessTokenResult(_rootUserCredentials.Email, _rootUserCredentials.Password);
        var tags = await GetDataBaseTags();
        var testTag = tags.Where(x => x.Value == "Test").ToList();
        for (int i = 0; i < 9; i++)
        {
            await ShouldSuccessCreateNewProject($"English app #{i}", "Help study english service", "Linux/Windows 10",
            "Client.exe", testTag);
        }

        _client.DefaultRequestHeaders.Add(Headers.Page, "1");
        _client.DefaultRequestHeaders.Add(Headers.ItemsPerPage, "5");

        var projectsResponse = await _client.GetAsync(ProjectsEndpoints.Base);
        var paginatedResult = await projectsResponse.ToPaginatedResult<ProjectShortDto>();
        paginatedResult.Succeeded.ShouldBeTrue();
        paginatedResult.TotalCount.ShouldBe(9);
        paginatedResult.Data.Count.ShouldBe(5);
        paginatedResult.Data[0].Name.ShouldBe("English app #8");
        paginatedResult.HasNextPage.ShouldBeTrue();
        paginatedResult.HasPreviousPage.ShouldBeFalse();

        _client.DefaultRequestHeaders.Remove(Headers.Page);
        _client.DefaultRequestHeaders.Add(Headers.Page, "2");

        var projectsNextPageResponse = await _client.GetAsync(ProjectsEndpoints.Base);
        var paginatedNextPageResult = await projectsNextPageResponse.ToPaginatedResult<ProjectShortDto>();
        paginatedNextPageResult.Succeeded.ShouldBeTrue();
        paginatedNextPageResult.TotalCount.ShouldBe(9);
        paginatedNextPageResult.Data.Count.ShouldBe(4);
        paginatedNextPageResult.Data[0].Name.ShouldBe("English app #3");
        paginatedNextPageResult.HasNextPage.ShouldBeFalse();
        paginatedNextPageResult.HasPreviousPage.ShouldBeTrue();
    }

    [Fact]
    public async Task add_tag_to_project_should_succesfuly_shown_in_get_project_request()
    {
        await ShouldSuccessGetAccessTokenResult(_rootUserCredentials.Email, _rootUserCredentials.Password);
        var tags = await GetDataBaseTags();
        var webTag = tags.Where(x => x.Value == "Web").ToList();
        var testTagId = tags.FirstOrDefault(x => x.Value == "Test").Id;
        var appId = await ShouldSuccessCreateNewProject("English app       ", "Help study english service", "Linux/Windows 10",
            "Client.exe", webTag);

        var response = await _client.PutAsJsonAsync(ProjectsEndpoints.GetTagsRoute(), new P007Request(Guid.Parse(appId), testTagId));
        var result = await response.ToResult();
        result.Succeeded.ShouldBeTrue();

        var projResponse = await _client.GetAsync(ProjectsEndpoints.GetProjectRoute(Guid.Parse(appId)));
        var projResult = await projResponse.ToResult<ProjectDto>();
        projResult.Data.Tags.Count.ShouldBe(2);
        projResult.Data.Tags.ShouldContain(x => x.Id == testTagId);
        projResult.Data.Tags.ShouldContain(x => x.Id == webTag.First().Id);

        var deleteResponse = await _client.DeleteAsync(ProjectsEndpoints.GetSingleProjectTagRoute(projResult.Data.Id, projResult.Data.Tags.First().Id));
        var deleteResult = await deleteResponse.ToResult();
        deleteResult.Succeeded.ShouldBeTrue();
    }

    [Fact]
    public async Task add_tag_with_exists_name_should_be_onseccess()
    {
		await ShouldSuccessGetAccessTokenResult(_rootUserCredentials.Email, _rootUserCredentials.Password);
		var tags = await GetDataBaseTags();
		var response = await _client.PostAsJsonAsync(ProjectsEndpoints.GetTagsRoute(), new P016Request(tags.First().Value));
		var result = await response.ToResult();
		result.Succeeded.ShouldBeFalse();
	}

	[Fact]
	public async Task add_tag_should_be_success()
	{
		await ShouldSuccessGetAccessTokenResult(_rootUserCredentials.Email, _rootUserCredentials.Password);
		var response = await _client.PostAsJsonAsync(ProjectsEndpoints.GetTagsRoute(), new P016Request(new Random().Next(2000, 5000).ToString()));
		var result = await response.ToResult<Guid>();
		result.Succeeded.ShouldBeTrue();
        result.Data.ShouldNotBe(default);
	}

	[Fact]
    public async Task create_three_app_with_tags_then_filter_by_single_or_more_tags_should_return_tagged_projects()
    {
        await ShouldSuccessGetAccessTokenResult(_rootUserCredentials.Email, _rootUserCredentials.Password);

        var tags = await GetDataBaseTags();
        var webTag = tags.Where(x => x.Value == "Web").ToList();
        var testTag = tags.Where(x => x.Value == "Test").ToList();
        var desktopTag = tags.Where(x => x.Value == "Desktop").ToList();
        var androidTag = tags.Where(x => x.Value == "Android").ToList();

        await ShouldSuccessCreateNewProject("English app       ", "Help study english service", "Linux/Windows 10",
            "Client.exe", testTag);

        await ShouldSuccessCreateNewProject("English app mobile", "Help study english mobile app", "Android",
            "Client.exe", testTag.Concat(webTag).ToList());

        var projectId = await ShouldSuccessCreateNewProject("Line tsd app", "Work as line", "Android",
            "Client.exe", testTag.Concat(webTag).Concat(desktopTag).ToList());

        await ShouldSuccessCreateNewProject("Mes tsd app", "Work as line", "Android",
            "Client.exe", tags);

        var tagsResponse = await _client.GetAsync(ProjectsEndpoints.GetTagsRoute());
        var tagsResult = await tagsResponse.ToResult<P013Response>();
        tagsResult.Succeeded.ShouldBeTrue();
        tagsResult.Data.Tags.Count.ShouldBe(4);

        var filterByMobileTagRequest = new P012Request(string.Empty, webTag.Select(x => x.Id).ToList());
        var filterByMobileTagResponse = await _client.PostAsJsonAsync(ProjectsEndpoints.GetProjectFilterRoute(), filterByMobileTagRequest);
        var filterByMobileResult = await filterByMobileTagResponse.ToPaginatedResult<ProjectShortDto>();
        filterByMobileResult.Succeeded.ShouldBeTrue();
        filterByMobileResult.Data.Count.ShouldBe(3);
        filterByMobileResult.Data.First().Name.ShouldBe("Mes tsd app");
        filterByMobileResult.Data[1].Name.ShouldBe("Line tsd app");
        filterByMobileResult.Data.Last().Name.ShouldBe("English app mobile");

        var filterByHelpMobileTagRequest = new P012Request(string.Empty, desktopTag.Concat(webTag).Select(x => x.Id).ToList());
        var filterByHelpMobileTagResponse = await _client.PostAsJsonAsync(ProjectsEndpoints.GetProjectFilterRoute(), filterByHelpMobileTagRequest);
        var filterByHelpMobileResult = await filterByHelpMobileTagResponse.ToPaginatedResult<ProjectShortDto>();
        filterByHelpMobileResult.Succeeded.ShouldBeTrue();
        filterByHelpMobileResult.Data.Count.ShouldBe(2);
        filterByHelpMobileResult.Data.First().Name.ShouldBe("Mes tsd app");
        filterByHelpMobileResult.Data.Last().Name.ShouldBe("Line tsd app");

        var filterByTopTagRequest = new P012Request(string.Empty, androidTag.Select(x => x.Id).ToList());
        var filterByTopTagResponse = await _client.PostAsJsonAsync(ProjectsEndpoints.GetProjectFilterRoute(), filterByTopTagRequest);
        var filterByTopTagResult = await filterByTopTagResponse.ToPaginatedResult<ProjectShortDto>();
        filterByTopTagResult.Succeeded.ShouldBeTrue();
        filterByTopTagResult.Data.Count.ShouldBe(1);

        var filterByTestTagRequest = new P012Request(string.Empty, testTag.Select(x => x.Id).ToList());
        var filterByTestTagResponse = await _client.PostAsJsonAsync(ProjectsEndpoints.GetProjectFilterRoute(), filterByTestTagRequest);
        var filterByTestTagResult = await filterByTestTagResponse.ToPaginatedResult<ProjectShortDto>();
        filterByTestTagResult.Succeeded.ShouldBeTrue();
        filterByTestTagResult.Data.Count.ShouldBe(4);

        var filterByHelpMobileTagTsdNameRequest = new P012Request("tsd", desktopTag.Concat(webTag).Select(x => x.Id).ToList());
        var filterByHelpMobileTagTsdNameResponse = await _client.PostAsJsonAsync(ProjectsEndpoints.GetProjectFilterRoute(), filterByHelpMobileTagTsdNameRequest);
        var filterByHelpMobileTagTsdNameResult = await filterByHelpMobileTagTsdNameResponse.ToPaginatedResult<ProjectShortDto>();
        filterByHelpMobileTagTsdNameResult.Succeeded.ShouldBeTrue();
        filterByHelpMobileTagTsdNameResult.Data.Count.ShouldBe(2);
    }

    [Fact]
    public async Task create_release_and_add_release_note_should_successfuly_return_in_get_project_response()
    {
        await ShouldSuccessGetAccessTokenResult(_rootUserCredentials.Email, _rootUserCredentials.Password);
        var tags = await GetDataBaseTags();
        var appId = await ShouldSuccessCreateNewProject("English test app", "Telegram bot web api app",
            "Windows 10", "Client.exe", tags);

        var response = await UploadRelease(appId, _firstReleaseFilePath);

        var content = await response.ToResult();
        content.Succeeded.ShouldBeTrue();
        content.Messages.FirstOrDefault().ShouldNotBeNull();
        var releaseId = content.Messages.FirstOrDefault();

        var releaseNoteRequest = new P011Request(Guid.Parse(appId), Guid.Parse(releaseId), "Fixed a lot problems");
        var releaseNoteResponse = await _client.PostAsJsonAsync(ProjectsEndpoints.GetReleaseNoteRoute(), releaseNoteRequest);
        var releaseNoteResult = await releaseNoteResponse.ToResult();
        releaseNoteResult.Succeeded.ShouldBeTrue();

        var appResponse = await _client.GetAsync(ProjectsEndpoints.GetProjectRoute(Guid.Parse(appId)));
        var appResult = await appResponse.ToResult<ProjectDto>();
        appResult.Succeeded.ShouldBeTrue();
        appResult.Data.ShouldNotBeNull();
        appResult.Data.Tags.ShouldNotBeNull().ShouldNotBeEmpty();
        appResult.Data.Releases.ShouldNotBeNull().ShouldNotBeEmpty();
        appResult.Data.Releases.First().ShouldNotBeNull().ReleaseNote.ShouldBe("Fixed a lot problems");

    }

    [Fact]
    public async Task create_prject_with_old_tags_with_one_new_tag_should_succefuly_showed_in_get_project_response()
    {
		await ShouldSuccessGetAccessTokenResult(_rootUserCredentials.Email, _rootUserCredentials.Password);
		var tags = await GetDataBaseTags();
        tags.Add(new() { Value = "New tag" });

	   var appId = await ShouldSuccessCreateNewProject("English test app", "Telegram bot web api app",
			"Windows 10", "Client.exe", tags);
		var appResponse = await _client.GetAsync(ProjectsEndpoints.GetProjectRoute(Guid.Parse(appId)));
		var appResult = await appResponse.ToResult<ProjectDto>();
		appResult.Succeeded.ShouldBeTrue();
		appResult.Data.ShouldNotBeNull();
		appResult.Data.Tags.ShouldNotBeNull().ShouldNotBeEmpty();
        appResult.Data.Tags.Count.ShouldBeGreaterThan(1);
        appResult.Data.Tags.ShouldContain(x => x.Value == "New tag");
	}

	[Fact]
	public async Task create_project_with_new_tags_should_succefuly_showed_in_get_project_response()
	{
		await ShouldSuccessGetAccessTokenResult(_rootUserCredentials.Email, _rootUserCredentials.Password);
        var newTags = new List<TagDto>()
        {
			new() { Value = "New tag 1" }, new() { Value = "New tag 2" }
		};

		var appId = await ShouldSuccessCreateNewProject("English test app", "Telegram bot web api app",
			 "Windows 10", "Client.exe", newTags);
		var appResponse = await _client.GetAsync(ProjectsEndpoints.GetProjectRoute(Guid.Parse(appId)));
		var appResult = await appResponse.ToResult<ProjectDto>();
		appResult.Succeeded.ShouldBeTrue();
		appResult.Data.ShouldNotBeNull();
		appResult.Data.Tags.ShouldNotBeNull().ShouldNotBeEmpty();
        appResult.Data.Tags.Count.ShouldBe(2);
	}

    [Fact]
	public async Task create_project_with_new_same_name_tags_should_not_be_succefuly()
	{
		await ShouldSuccessGetAccessTokenResult(_rootUserCredentials.Email, _rootUserCredentials.Password);
		var newTags = new List<TagDto>()
		{
			new() { Value = "New tag 1" }, new() { Value = "New tag 1" }
		};
		var request = new P003Request("English test app", "Telegram bot web api app", "Windows 10", "Client.exe", newTags);
		var response = await _client.PostAsJsonAsync(ProjectsEndpoints.Base, request);
		var content = await response.ToResult();
		content.Succeeded.ShouldBeFalse();
	}

	private async Task<HttpResponseMessage> UploadRelease(string appId, string filePath)
    {
        using var multipartFormContent = new MultipartFormDataContent();
        var fileStreamContent = new StreamContent(File.OpenRead(filePath));
        fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("application/zip");
        multipartFormContent.Add(fileStreamContent, name: "file", fileName: "English-test-bot-1.zip");
        return await _client.PostAsync(ProjectsEndpoints.GetReleasesRoute(Guid.Parse(appId)), multipartFormContent);
    }

    private async Task<string> ShouldSuccessCreateNewProject(string appName, string description, string systemRequirements,
        string exeFileName, List<TagDto> tags)
    {
        var request = new P003Request(appName, description, systemRequirements, exeFileName, tags);
        var response = await _client.PostAsJsonAsync(ProjectsEndpoints.Base, request);
        var content = await response.ToResult();
        content.Succeeded.ShouldBeTrue();
        content.Messages.ShouldNotBeEmpty();
        content.Messages.FirstOrDefault().ShouldNotBeNull();
        return content.Messages.FirstOrDefault();
    }
    private async Task<List<TagDto>> GetDataBaseTags()
    {
        var tagsResponse = await _client.GetAsync(ProjectsEndpoints.GetTagsRoute());
        var tagsResult = await tagsResponse.ToResult<P013Response>();
        tagsResult.Succeeded.ShouldBeTrue();
        tagsResult.Data.ShouldNotBeNull().Tags.ShouldNotBeNull().ShouldNotBeEmpty();
        return tagsResult.Data.Tags.ToList();
    }
    public void Dispose()
    {
        Directory.Delete(_downloadPath, true);
        Directory.Delete("Files", true);
    }
}
