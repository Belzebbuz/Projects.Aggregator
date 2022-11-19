using Domain.Base;
using SharedLibrary.Helpers;
using SharedLibrary.Wrapper;
using System.ComponentModel.DataAnnotations;

namespace Domain.Aggregators.Project;

public sealed class Project : AuditableEntity, IAggregateRoot
{
    [MaxLength(30)]
    public string Name { get; private set; }

    [MaxLength(400)]
    public string Description { get; private set; }

    [MaxLength(400)]
    public string SystemRequirements { get; private set; }

    [MaxLength(20)]
    public string ExeFileName { get; private set; }

    private ICollection<PatchNote> _patchNotes;
    public IReadOnlyCollection<PatchNote> PatchNotes => _patchNotes.ToList();

    private HashSet<Tag> _tags;
    public IReadOnlyCollection<Tag> Tags => _tags;

    private ICollection<Release> _releases;
    public IReadOnlyCollection<Release> Releases => _releases.ToList();
    private Project()
    {
    }

    public static Project Create(string name, string description, string exeFileName,
        string systemRequirements, ICollection<Tag> tags)
    {
        return new()
        {
            Name = name.Trim() ?? throw new ArgumentNullException(nameof(name)),
            Description = description.Trim() ?? throw new ArgumentNullException(nameof(description)),
            ExeFileName = exeFileName.Trim() ?? throw new ArgumentNullException(nameof(exeFileName)),
            SystemRequirements = systemRequirements.Trim() ?? throw new ArgumentNullException(nameof(systemRequirements)),
            _tags = tags.ToHashSet()
        };
    }

    public IResult Update(string name, string description, string exeFileName, string systemRequirements)
    {
        Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;
        Description = string.IsNullOrEmpty(description) ? throw new ArgumentNullException(nameof(description)) : description;
        ExeFileName = string.IsNullOrEmpty(exeFileName) ? throw new ArgumentNullException(nameof(exeFileName)) : exeFileName;
        SystemRequirements = string.IsNullOrEmpty(systemRequirements) ? throw new ArgumentNullException(nameof(systemRequirements)) : systemRequirements;
        return Result.Success();
    }

    public IResult<Release> AddRelease(string version, string url, string? gitSha = null, string? gitBranch = null, bool forceAdd = true)
    {
        ThrowHelper.NotLoadedProperty(_releases, nameof(_releases), nameof(Project), Id.ToString());

        var existRelease = _releases.FirstOrDefault(x => x.Version == version);

        if (existRelease != null && !forceAdd)
            return Result<Release>.Fail($"{Name} already have release with version: {version}");

        var newRelease = Release.Create(version, url, gitSha, gitBranch);
        _releases.Add(newRelease);
        return Result<Release>.Success(newRelease);
    }

    public IResult SetReleaseNote(Guid releaseId, string? text)
    {
        ThrowHelper.NotLoadedProperty(_releases, nameof(_releases), nameof(Project), Id.ToString());
        var release = _releases.SingleOrDefault(x => x.Id == releaseId);
        ThrowHelper.NotFoundEntity(release, releaseId.ToString(), nameof(Release));

        return release.SetReleaseNote(text);
    }

    public IResult RemoveRelease(Guid releaseId)
    {
        ThrowHelper.NotLoadedProperty(_releases, nameof(_releases), nameof(Project), Id.ToString());
        var release = _releases.SingleOrDefault(x => x.Id == releaseId);
        ThrowHelper.NotFoundEntity(release, releaseId.ToString(), nameof(Release));

        _releases.Remove(release);
        return Result.Success();
    }

    public IEnumerable<Release> GetLastReleases(int saveLastReleases)
    {
        ThrowHelper.NotLoadedProperty(_releases, nameof(_releases), nameof(Project), Id.ToString());

        return _releases.OrderByDescending(x => x.LastModifiedOn).Skip(saveLastReleases);
    }

    public void RemoveReleases(IEnumerable<Release> releases)
    {
        ThrowHelper.NotLoadedProperty(_releases, nameof(_releases), nameof(Project), Id.ToString());

        foreach (var release in releases)
        {
            var existRelease = _releases.SingleOrDefault(x => x.Id == release.Id);
            ThrowHelper.NotFoundEntity(existRelease, release.Id.ToString(), nameof(Release));

            _releases.Remove(existRelease);
        }
    }

    public Release GetRelease(Guid releaseId)
    {
        ThrowHelper.NotLoadedProperty(_releases, nameof(_releases), nameof(Project), Id.ToString());
        var release = _releases.SingleOrDefault(x => x.Id == releaseId);
        ThrowHelper.NotFoundEntity(release, releaseId.ToString(), nameof(Release));
        return release;
    }

    public IResult AddTag(Tag tag)
    {
        ThrowHelper.NotLoadedProperty(_tags, nameof(_tags), nameof(Project), Id.ToString());
        ThrowHelper.NotNull(tag, nameof(Tag));
        if (_tags.Any(x => x.Id == tag.Id))
            return Result.Success("Already exists");

        _tags.Add(tag);
        return Result.Success();
    }

    public IResult RemoveTag(Guid tagId)
    {
        ThrowHelper.NotLoadedProperty(_tags, nameof(_tags), nameof(Project), Id.ToString());
        var tag = _tags.FirstOrDefault(x => x.Id == tagId);
        ThrowHelper.NotFoundEntity(tag, tagId.ToString(), nameof(Tag));
        _tags.Remove(tag);
        return Result.Success();
    }

    public void UpdateTags(List<Tag> newTags)
    {
        ThrowHelper.NotLoadedProperty(_tags, nameof(_tags), nameof(Project), Id.ToString());
        _tags.RemoveWhere(x => !newTags.Contains(x));
        newTags.ForEach(x => _tags.Add(x));
    }

    public void AddPatchNote(string text)
    {
        ThrowHelper.NotLoadedProperty(_patchNotes, nameof(_patchNotes), nameof(Project), Id.ToString());
        _patchNotes.Add(new(text));
    }

    public void UpdatePatchNote(Guid patchNoteId, string text)
    {
		ThrowHelper.NotLoadedProperty(_patchNotes, nameof(_patchNotes), nameof(Project), Id.ToString());
        var patchNote = _patchNotes.SingleOrDefault(x => x.Id == patchNoteId);
        ThrowHelper.NotFoundEntity(patchNote, patchNoteId.ToString(), nameof(PatchNote));
        patchNote.Update(text);
	}

	public void RemovePatchNote(Guid patchNoteId)
	{
		ThrowHelper.NotLoadedProperty(_patchNotes, nameof(_patchNotes), nameof(Project), Id.ToString());
		var patchNote = _patchNotes.SingleOrDefault(x => x.Id == patchNoteId);
		ThrowHelper.NotFoundEntity(patchNote, patchNoteId.ToString(), nameof(PatchNote));
		_patchNotes.Remove(patchNote);
	}
}
