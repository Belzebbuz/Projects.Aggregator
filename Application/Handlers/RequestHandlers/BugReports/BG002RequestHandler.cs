using Application.Contracts.Repository;
using Application.Contracts.Services;
using Ardalis.Specification;
using Domain.Aggregators.BugAggregate;
using Mapster;
using SharedLibrary.ApiMessages.BugReports.BG002;
using SharedLibrary.ApiMessages.BugReports.Dto;
using SharedLibrary.Wrapper;
using System.Drawing;

namespace Application.Handlers.RequestHandlers.BugReports;

public class BG002RequestHandler : IRequestHandler<BG002Request, PaginatedResult<BugReportDto>>
{
	private readonly IReadRepository<BugReport> _readRepository;
	private readonly IPaginationService _paginationService;

	public BG002RequestHandler(IReadRepository<BugReport> readRepository, IPaginationService paginationService)
	{
		_readRepository = readRepository;
		_paginationService = paginationService;
	}
	public async Task<PaginatedResult<BugReportDto>> Handle(BG002Request request, CancellationToken cancellationToken)
	{
		var count = await _readRepository.CountAsync();
		var pagination = _paginationService.Calculate(count);
		var bugReports = await _readRepository.ListAsync(new GetBugReports(pagination));
		return PaginatedResult<BugReportDto>.Success(bugReports, count, pagination.Page, pagination.RecordsPerPage);
	}

	public class GetBugReports : Specification<BugReport, BugReportDto>
	{
		public GetBugReports(Pagination pagination)
		{
			Query
				.OrderByDescending(x => x.LastModifiedOn)
				.Skip((pagination.Page - 1) * pagination.RecordsPerPage)
				.Take(pagination.RecordsPerPage)
				.Adapt<BugReportDto>();
		}
	}
}
