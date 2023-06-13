/*******************************************************************************
* Copyright (C) Oxygen
* 
* Author: dj.wong
* Create Date: 09/04/2015 11:47:14
* Description: Automated building by service@Oxygen.com 
* 
* Revision History:
* Date         Author               Description
*
*********************************************************************************/

using System.Reflection;
using static System.Enum;
using SystemEnum = System.Enum;


namespace Saas.Core.Infrastructure.Extentions
{
    /// <summary>
    ///     枚举扩展方法类
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取描述
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescriptionForEnum(this object value)
        {
            try
            {
                if (value == null) return string.Empty;
                var type = value.GetType();
                var field = type.GetField(Enum.GetName(type, value));

                if (field == null) return value.ToString();

                var des = CustomAttributeData.GetCustomAttributes(type.GetMember(field.Name)[0]);

                return des.AnyOne() && des[0].ConstructorArguments.AnyOne()
                    ? des[0].ConstructorArguments[0].Value.ToString()
                    : value.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fromSingleEnum"></param>
        /// <returns></returns>
        public static IEnumerable<T> ToFlagsList<T>(this T fromSingleEnum) where T : struct
        {
            return fromSingleEnum.ToString()
                .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(
                    strenum =>
                    {
                        T outenum = default;
                        SystemEnum.TryParse(strenum, true, out outenum);
                        return outenum;
                    });

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fromSingleEnum"></param>
        /// <returns></returns>
        public static IEnumerable<T> ToFlagsList<T>(this T? fromSingleEnum) where T : struct
        {
            if (!fromSingleEnum.HasValue) return null;

            return fromSingleEnum.ToString()
                .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(
                    strenum =>
                    {
                        T outenum = default;
                        SystemEnum.TryParse(strenum, true, out outenum);
                        return outenum;
                    });

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="i"></param>
        /// <returns></returns>
        public static T GetEnum<T>(this int i)
        {
            try
            {
                var enumType = typeof(T);
                TryParse(enumType, i.ToString(), out var result);
                return (T)result;
            }
            catch (Exception)
            {
                return default;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="i"></param>
        /// <returns></returns>
        public static T GetEnum<T>(this short i)
        {
            try
            {
                var enumType = typeof(T);
                TryParse(enumType, i.ToString(), out var result);
                return (T)result;
            }
            catch (Exception ex)
            {
                return default;
            }

        }
        /// <summary>
        /// 用于获取枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T GetEnum<T>(this string s)
        {

            try
            {
                var enumType = typeof(T);
                var result = (T)Enum.Parse(enumType, s);
                //var result = Convert.ToInt32(enumModel);
                return result;
            }
            catch
            {
                return default(T);
            }
        }
    }
}