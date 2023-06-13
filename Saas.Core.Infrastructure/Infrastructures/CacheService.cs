using Microsoft.Extensions.Caching.Memory;
using System.Collections;
using System.Reflection;

namespace Saas.Core.Infrastructure.Infrastructures
{
    /// <summary>
    /// 
    /// </summary>
    public class CacheService : ICacheService
    {
        ///// <summary>
        ///// 
        ///// </summary>
        //public static CacheService Default = new CacheService();

        /// <summary>
        /// 
        /// </summary>
        private readonly IMemoryCache _cache;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cache"></param>
        public CacheService(IMemoryCache cache)
        {
            _cache = cache;

        }


        /// <summary>
        /// 判断是否在缓存中
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public bool IsInCache(string key)
        {
            var keys = GetAllKeys();
            return keys.Any(i => i == key);
        }

        /// <summary>
        /// 获取所有缓存键
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllKeys()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var entries = _cache.GetType().GetField("_entries", flags)?.GetValue(_cache);
            var cacheItems = entries as IDictionary;
            var keys = new List<string>();
            if (cacheItems == null) return keys;
            keys.AddRange(from DictionaryEntry cacheItem in cacheItems select cacheItem.Key.ToString());
            return keys;
        }

        /// <summary>
        /// 获取所有的缓存值
        /// </summary>
        /// <returns></returns>
        public List<T> GetAllValues<T>()
        {
            var cacheKeys = GetAllKeys();
            var values = new List<T>();
            cacheKeys.ForEach(i =>
            {
                if (_cache.TryGetValue<T>(i, out var t))
                {
                    values.Add(t);
                }
            });
            return values;
        }
        /// <summary>
        /// 取得缓存数据
        /// </summary>
        /// <typeparam name="T">类型值</typeparam>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            _cache.TryGetValue<T>(key, out var value);
            return value;
        }

        /// <summary>
        /// 设置缓存(永不过期)
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">缓存值</param>
        public void SetNotExpire<T>(string key, T value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            if (_cache.TryGetValue(key, out T _))
                _cache.Remove(key);
            _cache.Set(key, value);
        }

        /// <summary>
        /// 设置缓存(滑动过期:超过一段时间不访问就会过期,一直访问就一直不过期)
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">缓存值</param>
        /// <param name="span"></param>
        public void SetSlidingExpire<T>(string key, T value, TimeSpan span)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            if (_cache.TryGetValue(key, out T _))
                _cache.Remove(key);
            _cache.Set(key, value, new MemoryCacheEntryOptions()
            {
                SlidingExpiration = span
            });
        }

        /// <summary>
        /// 设置缓存(绝对时间过期:从缓存开始持续指定的时间段后就过期,无论有没有持续的访问)
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">缓存值</param>
        public void SetAbsoluteExpire<T>(string key, T value, TimeSpan span)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            if (_cache.TryGetValue(key, out T _))
                _cache.Remove(key);
            _cache.Set(key, value, span);
        }

        /// <summary>
        /// 设置缓存(绝对时间过期+滑动过期:比如滑动过期设置半小时,绝对过期时间设置2个小时，那么缓存开始后只要半小时内没有访问就会立马过期,如果半小时内有访问就会向后顺延半小时，但最多只能缓存2个小时)
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">缓存值</param>
        public void SetSlidingAndAbsoluteExpire<T>(string key, T value, TimeSpan slidingSpan, TimeSpan absoluteSpan)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            if (_cache.TryGetValue(key, out T _))
                _cache.Remove(key);
            _cache.Set(key, value, new MemoryCacheEntryOptions()
            {
                SlidingExpiration = slidingSpan,
                AbsoluteExpiration = DateTimeOffset.Now.AddMilliseconds(absoluteSpan.Milliseconds)
            });
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">关键字</param>
        public void Remove(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            _cache.Remove(key);
        }


    }
}
