using Application.Contracts.Services;
using Microsoft.AspNetCore.Http;
using SharedLibrary;

namespace Infrastructure.Middleware;

public class PaginationMiddleware : IMiddleware
{
    private readonly IPaginationService _paginationService;

    public PaginationMiddleware(IPaginationService paginationService)
    {
        _paginationService = paginationService;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var page = context.Request.Headers[Headers.Page];
        var itemsPerPage = context.Request.Headers[Headers.ItemsPerPage];
        if (int.TryParse((string)page, out int currentPage))
        {
            if (int.TryParse((string)itemsPerPage, out int itemsCount))
            {
                _paginationService.SetRequestPaginate(currentPage, itemsCount);
            }
            else
            {
                _paginationService.SetRequestPaginate(currentPage, 10);
            }
        }
        else
        {
            _paginationService.SetRequestPaginate(1, 10);
        }
        await next(context);
    }
}
