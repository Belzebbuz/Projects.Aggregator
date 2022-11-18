using Microsoft.AspNetCore.Components;
using SharedLibrary.ApiMessages.Projects.Dto;

namespace Clients.MAUI.Pages.SharedForms;

public partial class EditProjectForm
{
	private FluentValidationValidator? _fluentValidationValidator;
	private bool _validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
	[Parameter] public CreateProjectDto ProjectDto { get; set; }
	[Parameter] public EventCallback OnSubmit { get; set; }
	[Parameter] public string SubmitButtonText { get; set; }

	private async Task SubmitAsync()
	{
		await OnSubmit.InvokeAsync();
	}
	public async Task<IEnumerable<TagDto>> SearchTagByNameAsync(string value)
	{
		var tagsResult = await _projectService.GetTagsByNameAsync(value);
		if (!string.IsNullOrEmpty(value))
		{
			if (tagsResult.Data != null)
			{
				if (!tagsResult.Data.Any(x => x.Value.ToLower() == value.ToLower()))
				{
					tagsResult.Data?.Insert(0, new() { Value = value });
				}
			}
			else
			{
				return new List<TagDto>() { new() { Value = value } };
			}
		}
		return tagsResult.Data;
	}

	public async Task OnSearchTagValueChangedAsync(TagDto tagDto)
	{
		if (ProjectDto.Tags == null)
			ProjectDto.Tags = new();


		if (tagDto != null)
		{
			if(tagDto.Value.Length >=15)
			{
				_snackBar.Add("Макисмум 15 символов", Severity.Warning);
				return;
			}
			if (ProjectDto.Tags.Any(x => x.Value == tagDto.Value))
			{
				_snackBar.Add("Уже добавлен!", Severity.Warning);
				return;
			}
			ProjectDto.Tags.Add(tagDto);
		}
	}
	public void Closed(MudChip chip)
	{
		ProjectDto.Tags.Remove((TagDto)chip.Value);
	}
}