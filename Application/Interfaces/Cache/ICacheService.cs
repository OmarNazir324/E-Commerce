namespace Application.Interfaces.Cache;

public interface ICacheService
{
    Task SetAsync<T>(string key,T value,int minutes);
    Task<T?> GetAsync<T>(string key);
    Task RemoveAsync(string key);
}