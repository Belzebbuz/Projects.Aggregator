using Application.Contracts.Repository;
using Application.Contracts.Services;
using Domain.Aggregators.BugAggregate;
using SharedLibrary.ApiMessages.BugReports.BG001;
using SharedLibrary.Wrapper;

namespace Application.Handlers.RequestHandlers.BugReports;

public class BG001RequestHandler : IRequestHandler<BG001Request, IResult>
{
	private readonly IRepository<BugReport> _repository;
	private readonly IFileStorageService _fileStorageService;

	public BG001RequestHandler(IRepository<BugReport> repository, IFileStorageService fileStorageService)
	{
		_repository = repository;
		_fileStorageService = fileStorageService;
	}
	public async Task<IResult> Handle(BG001Request request, CancellationToken cancellationToken)
	{
		var bugReport = BugReport.Create(request.Text);
		await _repository.AddAsync(bugReport);
		return Result.Success();
	}
}
