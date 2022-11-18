using Application.Contracts.Services;
using SharedLibrary.Wrapper;

namespace Infrastructure.Common.Services;

public class PaginationService : IPaginationService
{
    private Pagination _pagination;

    public Pagination Pagination => _pagination;

    public Pagination Calculate(int totalItemsCount)
    {
        _pagination.Setup(totalItemsCount);
        return _pagination;
    }

    public void SetRequestPaginate(int page, int itemsPerPage)
    {
        _pagination = new Pagination(page, itemsPerPage);
    }
}
