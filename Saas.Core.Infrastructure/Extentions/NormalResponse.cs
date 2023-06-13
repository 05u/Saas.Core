using Saas.Core.Infrastructure.Infrastructures;

namespace Saas.Core.Infrastructure.Extentions
{
    public static class NormalResult
    {
        /// <summary>
        /// 通用返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static NormalResult<T> Response<T>(this T entity)
        {
            return new NormalResult<T> { Data = entity };
        }

        /// <summary>
        /// 通用返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async static Task<NormalResult<T>> Response<T>(this Task<T> entity)
        {
            return new NormalResult<T> { Data = await entity };
        }


        /// <summary>
        /// 通用返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static NormalResult<T> Response<T>(this T entity, Infrastructures.NormalResult result)
        {
            return new NormalResult<T> { Data = entity, Message = result.Message, Reason = result.Reason, Successful = result.Successful };
        }





        /// <summary>
        /// 通用返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static NormalResult<List<T>> Response<T>(this IQueryable<T> entity)
        {
            return new NormalResult<List<T>> { Data = entity.ToList() };
        }
    }
}
