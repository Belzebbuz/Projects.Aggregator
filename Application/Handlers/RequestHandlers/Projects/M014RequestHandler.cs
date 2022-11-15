﻿using Application.Contracts.Repository;
using Application.Contracts.Services;
using Ardalis.Specification;
using Domain.Aggregators.Project;
using SharedLibrary.ApiMessages.Projects.M014;
using SharedLibrary.Helpers;
using SharedLibrary.Wrapper;

namespace Application.Handlers.RequestHandlers.Projects;

public class M014RequestHandler : IRequestHandler<M014Request, IResult>
{
    private readonly IFileStorageService _storageService;
    private readonly IRepository<Project> _repository;

    public M014RequestHandler(IFileStorageService fileStorageService, IRepository<Project> repository)
    {
        _storageService = fileStorageService;
        _repository = repository;
    }
    public async Task<IResult> Handle(M014Request request, CancellationToken cancellationToken)
    {
        var project = await _repository.SingleOrDefaultAsync(new GetSingleProjectById(request.ProjectId));
        ThrowHelper.NotFoundEntity(project, request.ProjectId.ToString(), nameof(Project));

        var release = project.GetRelease(request.ReleaseId);
        _storageService.DeleteSingleFile(release.Url);
        project.RemoveRelease(release.Id);
        await _repository.UpdateAsync(project);
        return Result.Success();
    }

    private class GetSingleProjectById : Specification<Project>, ISingleResultSpecification<Project>
    {
        internal GetSingleProjectById(Guid Id)
            => Query
            .AsSplitQuery()
            .Include(x => x.Releases)
            .Where(x => x.Id == Id);
    }
}
