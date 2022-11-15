using Application.Contracts.DI;

namespace Application.Contracts.Services.Caching;

public interface ICacheKeyService : IScopedService
{
    public string GetCacheKey(string name, object id);
}