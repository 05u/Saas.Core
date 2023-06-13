namespace Saas.Core.Infrastructure.Infrastructures
{
    /// <summary>
    /// 缓存数据
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IsInCache(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<string> GetAllKeys();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> GetAllValues<T>();
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetNotExpire<T>(string key, T value);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="span"></param>
        void SetSlidingExpire<T>(string key, T value, TimeSpan span);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="span"></param>
        void SetAbsoluteExpire<T>(string key, T value, TimeSpan span);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="slidingSpan"></param>
        /// <param name="absoluteSpan"></param>
        void SetSlidingAndAbsoluteExpire<T>(string key, T value, TimeSpan slidingSpan, TimeSpan absoluteSpan);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
    }
}
