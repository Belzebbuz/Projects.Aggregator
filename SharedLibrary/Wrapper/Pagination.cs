namespace SharedLibrary.Wrapper;

public class Pagination
{
    public int Page { get; }
    public int RecordsPerPage { get; } = 5;
    public int NextPage { get; private set; }
    public int PreviousPage { get; private set; }
    public int TotalAmountOfPages { get; private set; }
    public int ItemsCount { get; private set; }
    public Pagination(int page)
    {
        Page = page;
    }

    public Pagination(int page, int recordsPerPage)
    {
        Page = page;
        RecordsPerPage = recordsPerPage;
    }
    public void Setup(int itemsCount)
    {
        TotalAmountOfPages = (itemsCount + RecordsPerPage - 1) / RecordsPerPage;
        NextPage = Page != TotalAmountOfPages && TotalAmountOfPages > 0 ? Page + 1 : Page;
        PreviousPage = Page != 1 ? Page - 1 : Page;
        ItemsCount = itemsCount;
    }
}
