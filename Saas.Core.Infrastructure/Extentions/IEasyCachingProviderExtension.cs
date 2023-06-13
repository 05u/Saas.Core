using EasyCaching.Core;

namespace Saas.Core.Infrastructure.Extentions
{

    public static class IEasyCachingProviderExtension
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static async Task UpdateAsync<T>(this IEasyCachingProvider easyCachingProvider, string cacheKey, T value, TimeSpan ts)
        {

            if (await easyCachingProvider.ExistsAsync(cacheKey))
            {
                await easyCachingProvider.RemoveAsync(cacheKey);
            }

            await easyCachingProvider.TrySetAsync(cacheKey, value, ts);

        }
    }
}
