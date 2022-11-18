using Clients.MAUI.Infrastructure.Constants;
using SharedLibrary.Wrapper;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Clients.MAUI.Infrastructure.Authentication;

public class AuthenticationHeaderHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (request.Headers.Authorization?.Scheme != "Bearer")
        {
            var savedToken = await SecureStorage.GetAsync(StorageConstants.AuthToken);

            if (!string.IsNullOrWhiteSpace(savedToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);
            }
        }
        try
        {
			return await base.SendAsync(request, cancellationToken);
		}
		catch (Exception ex)
        {
            var errorResult = JsonSerializer.Serialize(Result.Fail(ex.Message));
            return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
            {
                Content = new StringContent(errorResult)
            };
        }
    }
}
