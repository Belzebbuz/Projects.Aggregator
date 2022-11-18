using Application.Contracts.DI;
using SharedLibrary.Wrapper;

namespace Application.Contracts.Services;

public interface IPaginationService : IScopedService
{
    public void SetRequestPaginate(int page, int itemsPerPage);
    public Pagination Calculate(int totalItemsCount);
    public Pagination Pagination { get; }
}
