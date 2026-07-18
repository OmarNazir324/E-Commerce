using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace InfraStructure.Caching
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly IConfiguration _configuration;

        public CacheService(IDistributedCache cache,IConfiguration configuration)
        {
            _cache = cache;
            _configuration = configuration;
        }

        public async Task SetAsync<T>(
            string key,
            T value,
            int minutes)
        {
            var json = JsonSerializer.Serialize(value);

            await _cache.SetStringAsync(
                key,
                json,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromMinutes(minutes)
                });
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var json = await _cache.GetStringAsync(key);

            return json == null
                ? default
                : JsonSerializer.Deserialize<T>(json);
        }
        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }

        //public async Task ClearAsync()
        //{
        //    var connection = await ConnectionMultiplexer.ConnectAsync(_configuration["Red"]);
        //    var server = connection.GetServer("localhost", 6379);
        //    await server.FlushDatabaseAsync();
        //}
    }
}
