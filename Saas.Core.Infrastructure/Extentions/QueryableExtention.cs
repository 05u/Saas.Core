using System.Linq.Expressions;
using System.Reflection;

namespace Saas.Core.Infrastructure.Extentions
{
    /// <summary>
    /// Queryable扩展
    /// </summary>
    public static class QueryableExtention
    {
        public static IQueryable<TSource> ConcatIf<TSource>(this IQueryable<TSource> first, bool condition, IQueryable<TSource> second)
           => condition ? first.Concat(second) : first;

        public static IQueryable<TSource> DistinctIf<TSource>(this IQueryable<TSource> source, bool condition)
            => condition ? source.Distinct() : source;

        public static IQueryable<TSource> DistinctIf<TSource>(this IQueryable<TSource> source, bool condition, IEqualityComparer<TSource> comparer)
            => condition ? source.Distinct(comparer) : source;

        public static IQueryable<TSource> ExceptIf<TSource>(this IQueryable<TSource> first, bool condition, IQueryable<TSource> second)
            => condition ? first.Except(second) : first;

        public static IQueryable<TSource> ExceptIf<TSource>(this IQueryable<TSource> first, bool condition, IQueryable<TSource> second, IEqualityComparer<TSource> comparer)
            => condition ? first.Except(second, comparer) : first;

        public static IQueryable<TSource> IntersectIf<TSource>(this IQueryable<TSource> first, bool condition, IQueryable<TSource> second)
            => condition ? first.Intersect(second) : first;

        public static IQueryable<TSource> IntersectIf<TSource>(this IQueryable<TSource> first, bool condition, IQueryable<TSource> second, IEqualityComparer<TSource> comparer)
            => condition ? first.Intersect(second, comparer) : first;

        public static IOrderedQueryable<TSource> OrderByIf<TSource, TKey>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer)
            => condition ? source.OrderBy(keySelector, comparer) : source.OrderBy(e => true);

        public static IOrderedQueryable<TSource> OrderByIf<TSource, TKey>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, TKey>> keySelector)
            => condition ? source.OrderBy(keySelector) : source.OrderBy(e => true);

        public static IOrderedQueryable<TSource> OrderByDescendingIf<TSource, TKey>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, TKey>> keySelector)
            => condition ? source.OrderByDescending(keySelector) : source.OrderByDescending(e => true);

        public static IOrderedQueryable<TSource> OrderByDescendingIf<TSource, TKey>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer)
            => condition ? source.OrderByDescending(keySelector, comparer) : source.OrderByDescending(e => true);

        public static IQueryable<TSource> ReverseIf<TSource>(this IQueryable<TSource> source, bool condition)
            => condition ? source.Reverse() : source;

        public static IQueryable<TSource> SkipIf<TSource>(this IQueryable<TSource> source, bool condition, int count)
            => condition ? source.Skip(count) : source;

        public static IQueryable<TSource> SkipWhileIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, int, bool>> predicate)
            => condition ? source.SkipWhile(predicate) : source;

        public static IQueryable<TSource> SkipWhileIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
            => condition ? source.SkipWhile(predicate) : source;

        public static IQueryable<TSource> TakeIf<TSource>(this IQueryable<TSource> source, bool condition, int? count)
            => condition ? source.Take(count ?? 100) : source;

        public static IQueryable<TSource> TakeWhileIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, int, bool>> predicate)
            => condition ? source.TakeWhile(predicate) : source;

        public static IQueryable<TSource> TakeWhileIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
            => condition ? source.TakeWhile(predicate) : source;

        public static IOrderedQueryable<TSource> ThenByIf<TSource, TKey>(this IOrderedQueryable<TSource> source, bool condition, Expression<Func<TSource, TKey>> keySelector)
            => condition ? source.ThenBy(keySelector) : source;

        public static IOrderedQueryable<TSource> ThenByIf<TSource, TKey>(this IOrderedQueryable<TSource> source, bool condition, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer)
            => condition ? source.ThenBy(keySelector, comparer) : source;

        public static IOrderedQueryable<TSource> ThenByDescendingIf<TSource, TKey>(this IOrderedQueryable<TSource> source, bool condition, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer)
            => condition ? source.ThenByDescending(keySelector, comparer) : source;

        public static IOrderedQueryable<TSource> ThenByDescendingIf<TSource, TKey>(this IOrderedQueryable<TSource> source, bool condition, Expression<Func<TSource, TKey>> keySelector)
            => condition ? source.ThenByDescending(keySelector) : source;

        public static IQueryable<TSource> UnionIf<TSource>(this IQueryable<TSource> first, bool condition, IQueryable<TSource> second)
            => condition ? first.Union(second) : first;

        public static IQueryable<TSource> UnionIf<TSource>(this IQueryable<TSource> first, bool condition, IQueryable<TSource> second, IEqualityComparer<TSource> comparer)
            => condition ? first.Union(second, comparer) : first;

        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, int, bool>> predicate)
            => condition ? source.Where(predicate) : source;


        /// <summary>
        /// WhereIf[在condition为true的情况下应用Where表达式]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="condition"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> expression)
        {
            return condition ? source.Where(expression) : source;
        }

        ///// <summary>
        ///// lambda表达式Or
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="expression1">The expression1.</param>
        ///// <param name="expression2">The expression2.</param>
        ///// <returns></returns>
        ///// 创建者：王宇
        ///// 创建日期：9/1/2014 10:32 AM
        ///// 修改者：
        ///// 修改时间：
        ///// ------------------------------------
        //public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        //{
        //    var invokedExpression = Expression.Invoke(expression2, expression1.Parameters);

        //    return Expression.Lambda<Func<T, bool>>(Expression.Or(expression1.Body, invokedExpression), expression1.Parameters);
        //}


        ///// <summary>
        ///// 分页扩展
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="query"></param>
        ///// <returns></returns>
        //public static async Task<PagedResult<T>> QueryPagesAsync<T>(this IQueryable<T> source, BaseCondition filter)
        //{
        //    var count = await source.CountAsync();
        //    var rows = new List<T>();
        //    if (count > 0)
        //    {

        //        var pagedQuery = source.OrderBy(filter.SortField, filter.SortType)
        //            .Skip((filter.PageIndex - 1) * filter.PageSize)
        //            .Take(filter.PageSize);
        //        rows = await pagedQuery.ToListAsync();
        //    }

        //    return new PagedResult<T>
        //    {
        //        Datas = rows,
        //        Records = count,
        //        PageIndex = filter.PageIndex,
        //        PageSize = filter.PageSize
        //    };
        //}




        /// <summary>
        /// 自定义排序
        /// </summary>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string field, string orderby = "desc")
        {
            field = field.IsBlank() ? "CreateTime" : field;
            ParameterExpression p = Expression.Parameter(typeof(T));
            Expression key = Expression.Property(p, field);

            var propInfo = GetPropertyInfo(typeof(T), field);
            var expr = GetOrderExpression(typeof(T), propInfo);

            if (orderby.IsNotBlank() && orderby.Contains("asc"))
            {
                var method = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "OrderBy" && m.GetParameters().Length == 2);
                var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
                return (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, expr });
            }
            else
            {
                var method = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "OrderByDescending" && m.GetParameters().Length == 2);
                var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
                return (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, expr });
            }
        }


        /// <summary>
        /// 获取反射
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static PropertyInfo GetPropertyInfo(Type objType, string name)
        {
            var properties = objType.GetProperties();
            var matchedProperty = properties.FirstOrDefault(p => p.Name.IsEqual(name));
            if (matchedProperty == null)
                throw new ArgumentException("name");

            return matchedProperty;
        }
        /// <summary>
        /// 获取生成表达式
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="pi"></param>
        /// <returns></returns>
        private static LambdaExpression GetOrderExpression(Type objType, PropertyInfo pi)
        {
            var paramExpr = Expression.Parameter(objType);
            var propAccess = Expression.PropertyOrField(paramExpr, pi.Name);
            var expr = Expression.Lambda(propAccess, paramExpr);
            return expr;
        }

        /// <summary>
        /// 传入条件之间为OR查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicates"></param>
        /// <returns></returns>
        public static IQueryable<T> WhereOR<T>(this IQueryable<T> source, params Expression<Func<T, bool>>[] predicates)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (predicates == null) throw new ArgumentNullException("predicates");
            if (predicates.Length == 0) return source.Where(x => false); // no matches!
            if (predicates.Length == 1) return source.Where(predicates[0]); // simple

            var param = Expression.Parameter(typeof(T), "x");
            Expression body = Expression.Invoke(predicates[0], param);
            for (int i = 1; i < predicates.Length; i++)
            {
                body = Expression.OrElse(body, Expression.Invoke(predicates[i], param));
            }
            var lambda = Expression.Lambda<Func<T, bool>>(body, param);
            return source.Where(lambda);
        }


    }
}
