using Microsoft.EntityFrameworkCore;
using Saas.Core.Infrastructure.Attributes;
using Saas.Core.Infrastructure.Dtos;
using Saas.Core.Infrastructure.Enums;
using Saas.Core.Infrastructure.Infrastructures;
using System.ComponentModel;
using System.Reflection;
using SystemEnum = System.Enum;

namespace Saas.Core.Infrastructure.Extentions
{
    /// <summary>
    /// 扩展集合功能
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// 将字符串加在一起
        /// </summary>
        /// <param name="source"></param>
        /// <param name="splitor">分隔符</param>
        /// <returns></returns>
        public static string Join(this IEnumerable<string> source, string splitor)
        {
            if (source == null) return string.Empty;

            return string.Join(splitor, source);
        }

        /// <summary>
        /// 复制序列中的数据
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="iEnumberable">原数据</param>
        /// <param name="startIndex">原数据开始复制的起始位置</param>
        /// <param name="length">需要复制的数据长度</param>
        /// <returns></returns>
        public static IEnumerable<T> Copy<T>(this IEnumerable<T> iEnumberable, int startIndex, int length)
        {
            var sourceArray = iEnumberable.ToArray();
            T[] newArray = new T[length];
            Array.Copy(sourceArray, startIndex, newArray, 0, length);

            return newArray;
        }

        /// <summary>
        /// 将数据源映射成字符串数据源后加在一起
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="splitor">分隔符</param>
        /// <param name="selector">映射器</param>
        /// <returns></returns>
        public static string Join<T>(this IEnumerable<T> source, string splitor, Func<T, string> selector)
        {
            if (source == null) return string.Empty;

            return source.Select(selector).Join(splitor);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="splitor"></param>
        /// <returns></returns>
        public static string Join<T>(this IEnumerable<T> source, string splitor)
        {
            if (source == null) return string.Empty;

            return source.Select(x => x.ToString()).Join(splitor);
        }
        /// <summary>
        /// 为集合添加元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="add"></param>
        /// <returns></returns>
        public static IEnumerable<T> Plus<T>(this IEnumerable<T> source, T add)
        {
            foreach (var element in source)
            {
                yield return element;
            }
            yield return add;
        }
        /// <summary>
        /// 将元素添加到集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="add"></param>
        /// <returns></returns>
        public static IEnumerable<T> Plus<T>(this T source, IEnumerable<T> add)
        {
            yield return source;
            foreach (var element in add)
            {
                yield return element;
            }
        }

        ///// <summary>
        ///// 对source的每一个元素执行指定的动作action
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="source"></param>
        ///// <param name="action"></param>
        //public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        //{
        //    if (source == null || action == null)
        //        return;

        //    foreach (var element in source)
        //    {
        //        action(element);
        //    }
        //}

        /// <summary>
        /// 判断序列是否包含任何元素，如果序列为空，则返回False
        /// </summary>
        /// <typeparam name="T">序列类型</typeparam>
        /// <param name="source">序列</param>
        /// <returns></returns>
        public static bool AnyOne<T>(this IEnumerable<T> source)
        {
            return source != null ? source.Any() : false;
        }

        /// <summary>
        /// WhereIf[在condition为true的情况下应用Where表达式]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="condition"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> expression)
        {
            return condition ? source.Where(expression) : source;
        }


        /// <summary>
        /// 获取枚举类的名称
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription<TEnum>(this TEnum value)
        {
            if (value == null) return string.Empty;
            var fi = value.GetType().GetField(value.ToString());

            if (fi != null)
            {
                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes.Length > 0)
                {
                    return attributes[0].Description;
                }
            }

            return value.ToString();
        }


        ///// <summary>
        ///// 获取枚举类的名称
        ///// </summary>
        ///// <typeparam name="TEnum"></typeparam>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //public static string GetReactComponentPath<TEnum>(this TEnum value)
        //{
        //    if (value == null) return string.Empty;
        //    var fi = value.GetType().GetField(value.ToString());

        //    if (fi != null)
        //    {
        //        var attributes = (ReactComponentPathAttribute[])fi.GetCustomAttributes(typeof(ReactComponentPathAttribute), false);

        //        if (attributes.Length > 0)
        //        {
        //            return attributes[0].Description;
        //        }
        //    }

        //    return value.ToString();
        //}



        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetDescription(this SystemEnum enumValue)
        {
            FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);
            return attributes.Length > 0 ? attributes[0].Description : enumValue.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetToolTips(this SystemEnum enumValue)
        {
            FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

            ToolTipsAttribute[] attributes =
                (ToolTipsAttribute[])fi.GetCustomAttributes(
                typeof(ToolTipsAttribute),
                false);
            return attributes.Length > 0 ? attributes[0].Text : enumValue.ToString();
        }






        ///// <summary>
        ///// 获取枚举列表
        ///// </summary>
        //public static List<SelectEnumItem> SelectList<T>() where T : struct
        //{

        //    Type t = typeof(T);
        //    return t.SelectList();
        //}



        /// <summary>
        /// 获取枚举列表
        /// </summary>
        public static List<SelectEnumItem> SelectList(this Type t)
        {
            return t.IsEnum ? SystemEnum.GetValues(t)
                .Cast<SystemEnum>()
                .ToList()
                .ConvertAll(c => new SelectEnumItem { /*ToolTips = c.GetToolTips(),*/ Text = c.GetDescription(), Code = Convert.ToString(c), Id = Convert.ToInt32(c) }) : null;
        }

        #region 分页

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public static PagedResult<T> Paging<T>(this IQueryable<T> source, int pageIndex, int pageSize)
        {

            if (pageIndex <= 0)
                throw new ArgumentException("Index of current page can not less than 0 !", "pageIndex");
            if (pageSize <= 0)
                throw new ArgumentException("Size of page can not less than 0 !", "pageSize");
            var pagedQuery = source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);
            return new PagedResult<T>
            {
                Datas = pagedQuery.ToList(),
                Records = source.Count(),
                PageIndex = pageIndex,
                PageSize = pageSize


            };
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public static async Task<PagedResult<T>> PagingAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize)
        {
            if (pageIndex <= 0)
                throw new ArgumentException("Index of current page can not less than 0 !", nameof(pageIndex));
            if (pageSize <= 1)
                throw new ArgumentException("Size of page can not less than 1 !", nameof(pageSize));
            var pagedQuery = source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);
            return new PagedResult<T>
            {
                Datas = await pagedQuery.ToListAsync(),
                Records = await source.CountAsync(),
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public static PagedResult<T> Paging<T>(this IQueryable<T> source, int pageIndex, int pageSize, string sortField, string sortType)
        {

            if (pageIndex <= 0)
                throw new ArgumentException("Index of current page can not less than 0 !", "pageIndex");
            if (pageSize <= 0)
                throw new ArgumentException("Size of page can not less than 0 !", "pageSize");
            IQueryable<T> pagedQuery = null;
            if (sortField.IsNotBlank() && sortType.IsNotBlank())
                pagedQuery = source.OrderBy(sortField, sortType)
                            .Skip((pageIndex - 1) * pageSize)
                            .Take(pageSize);
            else
                pagedQuery = source
                           .Skip((pageIndex - 1) * pageSize)
                           .Take(pageSize);
            return new PagedResult<T>
            {
                Datas = pagedQuery.ToList(),
                Records = source.Count(),
                PageIndex = pageIndex,
                PageSize = pageSize


            };
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public static async Task<PagedResult<T>> PagingAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize, string sortField, string sortType)
        {
            if (pageIndex <= 0)
                throw new ArgumentException("Index of current page can not less than 0 !", nameof(pageIndex));
            if (pageSize <= 1)
                throw new ArgumentException("Size of page can not less than 1 !", nameof(pageSize));
            var count = await source.CountAsync()/*.ConfigureAwait(false).GetAwaiter().GetResult()*/;
            IQueryable<T> pagedQuery = null;
            if (sortField.IsNotBlank() && sortType.IsNotBlank())
                pagedQuery = source.OrderBy(sortField, sortType)
                            .Skip((pageIndex - 1) * pageSize)
                            .Take(pageSize);
            else
                pagedQuery = source
                           .Skip((pageIndex - 1) * pageSize)
                           .Take(pageSize);
            return new PagedResult<T>
            {
                Datas = await pagedQuery.ToListAsync(),
                Records = count,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public static async Task<NormalResult<PagedResult<T>>> PagingAPIResultAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize)
        {
            return new NormalResult<PagedResult<T>>()
            {
                Data = await PagingAsync(source, pageIndex, pageSize)
            };
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public static async Task<PagedResult<T>> PagingResultAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize, string sortField, string sortType)
        {
            return await PagingAsync(source, pageIndex, pageSize, sortField, sortType);
        }


        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public static NormalResult<PagedResult<T>> PagingAPIResult<T>(this IQueryable<T> source, int pageIndex, int pageSize)
        {
            return new NormalResult<PagedResult<T>>()
            {
                Data = Paging(source, pageIndex, pageSize)
            };
        }


        public static NormalResult<PagedResult<T>> PagingAPIResult<T>(this IQueryable<T> source, int pageIndex, int pageSize, string sortField, string sortType)
        {
            return new NormalResult<PagedResult<T>>()
            {
                Data = Paging(source, pageIndex, pageSize, sortField, sortType)
            };
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public static async Task<NormalResult<PagedResult<T>>> PagingAPIResultAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize, string sortField, string sortType)
        {
            return new NormalResult<PagedResult<T>>()
            {
                Data = await PagingAsync(source, pageIndex, pageSize, sortField, sortType)
            };
        }
        #endregion
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public static NormalResult<PagedResult<T>> PagingAPIResult<T>(this IEnumerable<T> source, int pageIndex, int pageSize)
        {
            return new NormalResult<PagedResult<T>>()
            {
                Data = Paging(source, pageIndex, pageSize)
            };
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public static PagedResult<T> Paging<T>(this IEnumerable<T> source, int pageIndex, int pageSize)
        {
            if (pageIndex <= 0)
                throw new ArgumentException("Index of current page can not less than 0 !", "pageIndex");
            if (pageSize <= 0)
                throw new ArgumentException("Size of page can not less than 0 !", "pageSize");
            var pagedQuery = source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);
            return new PagedResult<T>
            {
                Datas = pagedQuery.ToList(),
                Records = source.Count(),
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source is null || source == default)
            {
                yield break;

            }
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
