using Saas.Core.Data.Entities;
using System.Linq.Expressions;

namespace Saas.Core.Data.Respository
{
    public interface IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        Task<string> InsertAsync(T entity);


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        string Insert(T entity);

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list">实体集合</param>
        Task BatchInsertAsync(List<T> list);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="notUpdateProperties"></param>
        /// <param name="properties">需要更新字段的表达式(可以不指定，默认全部更新)</param>
        /// <returns></returns>
        Task UpdateAsync(T entity, List<Expression<Func<T, object>>> notUpdateProperties = null, params Expression<Func<T, object>>[] properties);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="properties">需要更新字段的表达式(可以不指定，默认全部更新)</param>
        /// <returns></returns>
        void Update(T entity, params Expression<Func<T, object>>[] properties);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entities">实体</param>
        /// <returns></returns>
        Task BatchUpdateAsync(List<T> entities);

        /// <summary>
        /// 删除指定主键的数据(逻辑删除)
        /// </summary>
        /// <param name="keyId">主键Id</param>
        /// <returns></returns>
        Task DeleteAsync(string keyId);

        /// <summary>
        /// 批量删除(逻辑删除)
        /// </summary>
        /// <param name="keyIds">主键Id集合</param>
        /// <returns></returns>
        Task DeleteAsync(IList<string> keyIds);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<bool> SoftDeleteAsync(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 根据条件删除实体(物理删除)
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 根据主键查询实体，如果不存在，则抛出异常
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        Task<T> FindAsync(object keyValue);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<T> SingleAsync(Expression<Func<T, bool>> predicate);


        /// <summary>
        /// 查询不跟踪实体
        /// </summary>
        /// <param name="predicate"></param>

        /// <returns></returns>
        Task<T> FindAsNoTrackingAsync(string id);

        /// <summary>
        /// 根据条件查询实体，不存在返回null
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 查询不跟踪实体
        /// </summary>
        /// <param name="predicate"></param>

        /// <returns></returns>
        Task<T> FindAsNoTrackingAsync(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 根据条件判断是否存在相同的数据
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);


        /// <summary>
        /// 根据条件判断是否存在相同的数据
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        bool Exists(Expression<Func<T, bool>> predicate);


        /// <summary>
        /// Queryable(自动加上CompanyId过滤参数,默认为true)
        /// </summary>
        /// <returns></returns>
        IQueryable<T> Queryable();



        /// <summary>
        /// 根据条件查询实体，不存在返回null
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        Task<List<T>> FindAllAsync(Expression<Func<T, bool>> predicate);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>        
        /// <returns></returns>
        IQueryable<T> QueryableAll(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="updateExpression"></param>
        /// <returns></returns>
        Task<int> UpdateBatchAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> updateExpression);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="updateExpression"></param>
        /// <returns></returns>
        int UpdateBatch(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> updateExpression);

    }
}
