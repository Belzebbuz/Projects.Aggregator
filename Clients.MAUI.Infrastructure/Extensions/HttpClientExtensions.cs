using SharedLibrary;

namespace Clients.MAUI.Infrastructure.Extensions;

public static class HttpClientExtensions
{
    public static void AddOrUpdatePaginationHeaders(this HttpClient client, int page, int itemsPerPage)
    {
        var pageHeader = client.DefaultRequestHeaders.FirstOrDefault(x => x.Key == Headers.Page);
        if (pageHeader.Key == null)
            client.DefaultRequestHeaders.Add(Headers.Page, page.ToString());
        if (pageHeader.Key != null)
        {
            client.DefaultRequestHeaders.Remove(Headers.Page);
            client.DefaultRequestHeaders.Add(Headers.Page, page.ToString());
        }

        var itemsPerPageHeader = client.DefaultRequestHeaders.FirstOrDefault(x => x.Key == Headers.ItemsPerPage);
        if (itemsPerPageHeader.Key == null)
            client.DefaultRequestHeaders.Add(Headers.ItemsPerPage, itemsPerPage.ToString());
        if (itemsPerPageHeader.Key != null)
        {
            client.DefaultRequestHeaders.Remove(Headers.ItemsPerPage);
            client.DefaultRequestHeaders.Add(Headers.ItemsPerPage, itemsPerPage.ToString());
        }
    }
}
