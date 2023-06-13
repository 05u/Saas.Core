
using StackExchange.Redis;

namespace Saas.Core.Infrastructure.Utilities
{
    public interface IRedisStackExchangeService
    {
        public IDatabase GetDatabase();

        List<long> GetSerialNumber(string key, int count = 1);

        List<long> GetStaticSerialNumber(string key, int count = 1);

        Task<List<int>> CacheCountCheck(string cacheKey, TimeSpan? ts, int count = 1);

        Task<bool> StringSetAsync(string key, string value, TimeSpan? expiry = default(TimeSpan?));

        Task<string> StringGetAsync(string key);

        Task<bool> StringSetAsync<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?));

        Task<T> StringGetAsync<T>(string key);
    }
}
