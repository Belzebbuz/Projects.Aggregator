using Microsoft.AspNetCore.Components;
using SharedLibrary.ApiMessages.Projects.Dto;

namespace Clients.MAUI.Pages.SharedForms.Dialogs;

public partial class SelectTagsDialog
{
    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; }

    [Parameter]
    public List<Guid> SelectedTags { get; set; }

    private List<SelectionTagRow> _allTags = new();
    private bool _isLoaded = false;
    protected override async Task OnInitializedAsync()
    {
        _isLoaded = false;
        await FindTagByName(String.Empty);
		_isLoaded = true;
    }


    void Submit()
    {
        MudDialog.Close(DialogResult.Ok(_allTags.Where(x => x.Selected).Select(x => x.Tag)));
    }

    private async Task FindTagByName(string text)
    {
        if(string.IsNullOrEmpty(text))
        {
			var allTagsResult = await _projectService.GetAllTagsAsync();
			if (allTagsResult.Data != null)
			{
                _allTags.Clear();
				allTagsResult.Data.ToList().ForEach(x => _allTags.Add(new(SelectedTags.Contains(x.Id), x)));
			}
            return;
		}
        var findTagsResult = await _projectService.GetTagsByNameAsync(text);
        if(findTagsResult.Data != null)
        {
            _allTags.Clear();
            _allTags = findTagsResult.Data.Select(x => new SelectionTagRow(SelectedTags.Contains(x.Id), x)).ToList();
		}
    }

    private class SelectionTagRow
    {
        public SelectionTagRow(bool selected, TagDto tag)
        {
            Selected = selected;
			Tag = tag;
        }

        public bool Selected { get; set; }
        public TagDto Tag { get; set; }
    }
}