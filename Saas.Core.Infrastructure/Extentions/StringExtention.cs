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

using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;


namespace Saas.Core.Infrastructure.Extentions
{
    /// <summary>
    /// 字符串扩展类
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 用于判断是否为空字符
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsBlank(this string s)
        {
            return s == null || (s.Trim().Length == 0);
        }

        /// <summary>
        /// 用于判断是否为空字符
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNotBlank(this string s)
        {
            return !s.IsBlank();
        }

        /// <summary>
        /// 将字符串转换成MD5加密字符串
        /// </summary>
        /// <param name="orgStr"></param>
        /// <returns></returns>
        public static string ToMd5(this string orgStr)
        {
            using (var md5 = MD5.Create())
            {
                var encoding = Encoding.UTF8;
                var encryptedBytes = md5.ComputeHash(encoding.GetBytes(orgStr));
                var sb = new StringBuilder(32);
                foreach (var bt in encryptedBytes)
                {
                    sb.Append(bt.ToString("x").PadLeft(2, '0'));
                }
                return sb.ToString();
            }
        }


        /// <summary>
        /// Base64Url编码
        /// </summary>
        /// <param name="text">待编码的文本字符串</param>
        /// <returns>编码的文本字符串</returns>
        public static string Base64UrlEncode(this string text)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(text);
            var base64 = Convert.ToBase64String(plainTextBytes).Replace('+', '-').Replace('/', '_').TrimEnd('=');

            return base64;
        }

        /// <summary>
        /// HMACSHA256算法
        /// </summary>
        /// <param name="text">内容</param>
        /// <param name="secret">密钥</param>
        /// <returns></returns>
        public static string ToHMACSHA256String(this string text, string secret)
        {
            secret ??= "";
            byte[] keyByte = Encoding.UTF8.GetBytes(secret);
            byte[] messageBytes = Encoding.UTF8.GetBytes(text);
            using var hmacsha256 = new HMACSHA256(keyByte);
            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
            return Convert.ToBase64String(hashmessage).Replace('+', '-').Replace('/', '_').TrimEnd('=');
        }
        /// <summary>
        /// 将字符串转换成MD5加密字符串
        /// </summary>
        /// <param name="orgStr">原始字符串</param>
        /// <param name="secretKey">固定的加密key</param>
        /// <returns></returns>
        public static string ToMd5(this string orgStr, string secretKey)
        {
            using (var md5 = MD5.Create())
            {
                var encoding = Encoding.UTF8;
                var key = $"{secretKey}{orgStr}";
                var encryptedBytes = md5.ComputeHash(encoding.GetBytes(key));
                var sb = new StringBuilder(32);
                foreach (var bt in encryptedBytes)
                {
                    sb.Append(bt.ToString("x").PadLeft(2, '0'));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// 获取扩展名
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GetExt(this string s)
        {
            var ret = string.Empty;
            if (!s.Contains('.')) return ret;
            var temp = s.Split('.');
            ret = temp[^1];

            return ret;
        }
        /// <summary>
        /// 验证QQ格式
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsQq(this string s)
        {
            return s.IsBlank() || Regex.IsMatch(s, @"^[1-9]\d{4,15}$");
        }

        /// <summary>
        /// 判断是否为有效的Email地址
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsEmail(this string s)
        {
            if (!s.IsBlank())
            {
                //return Regex.IsMatch(s,
                //         @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
                //         @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
                const string pattern = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
                return Regex.IsMatch(s, pattern);
            }
            return false;
        }

        /// <summary>
        /// 验证是否是合法的电话号码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsPhone(this string s)
        {
            if (!s.IsBlank())
            {
                return Regex.IsMatch(s, @"^\+?((\d{2,4}(-)?)|(\(\d{2,4}\)))*(\d{0,16})*$");
            }
            return true;
        }

        /// <summary>
        /// 验证是否是合法的手机号码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsMobile(this string s)
        {
            if (!s.IsBlank())
            {
                return Regex.IsMatch(s, @"^\+?\d{0,4}?[1][3-8]\d{9}$");
            }
            return false;
        }

        /// <summary>
        /// 手机号码脱敏
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string MobileDesensitization(this string s)
        {
            return s.IsMobile() ? Regex.Replace(s, "(\\d{3})\\d{4}(\\d{4})", "$1****$2") : s;
        }


        /// <summary>
        /// 验证是否是合法的邮编
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsZipCode(this string s)
        {
            if (!s.IsBlank())
            {
                return Regex.IsMatch(s, @"[1-9]\d{5}(?!\d)");
            }
            return true;
        }

        /// <summary>
        /// 验证是否是合法的传真
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsFax(this string s)
        {
            if (!s.IsBlank())
            {
                return Regex.IsMatch(s, @"(^[0-9]{3,4}\-[0-9]{7,8}$)|(^[0-9]{7,8}$)|(^\([0-9]{3,4}\)[0-9]{3,8}$)|(^0{0,1}13[0-9]{9}$)");
            }
            return true;
        }

        /// <summary>
        /// 检查字符串是否为有效的int数字
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsInt(this string val)
        {
            if (IsBlank(val))
                return false;
            return int.TryParse(val, out _);
        }

        /// <summary>
        /// 字符串转数字，未转换成功返回0
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int ToInt(this string val)
        {
            if (IsBlank(val))
                return 0;
            return int.TryParse(val, out int resultValue) ? resultValue : 0;
        }

        /// <summary>
        /// 检查字符串是否为有效的INT64数字
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsInt64(this string val)
        {
            if (IsBlank(val))
                return false;
            return long.TryParse(val, out _);
        }

        /// <summary>
        /// 检查字符串是否为有效的Decimal
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsDecimal(this string val)
        {
            if (IsBlank(val))
                return false;
            return decimal.TryParse(val, out _);
        }

        /// <summary>
        /// 获取中文字符串首字母
        /// </summary>
        /// <param name="source"></param>
        ///  <param name="toUpper">是否大写</param>
        /// <returns></returns>
        public static string GetChineseSpell(this string source, bool toUpper = true)
        {
            var len = source.Length;
            var myStr = new StringBuilder();
            for (var i = 0; i < len; i++)
            {
                myStr.Append(GetSpell(source.Substring(i, 1)));
            }
            return toUpper ? myStr.ToString().ToUpper() : myStr.ToString();
        }

        /// <summary>
        /// 比较两个字符串值是否相等
        /// </summary>
        public static bool IsEqual(this string source, string comapreValue, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            if (source.IsNotBlank() && comapreValue.IsNotBlank())
            {
                return source.Equals(comapreValue, comparison);
            }
            return source.IsBlank() && comapreValue.IsBlank();
        }

        /// <summary>  
        /// 获取单个中文的首字母  
        /// </summary>  
        /// <param name="cnChar"></param>  
        /// <returns></returns>  
        private static string GetSpell(string cnChar)
        {
            var arrCn = Encoding.Default.GetBytes(cnChar);
            if (arrCn.Length > 1)
            {
                var area = arrCn[0];
                var pos = arrCn[1];
                var code = (area << 8) + pos;
                var areacode = new[] { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };

                for (var i = 0; i < 26; i++)
                {
                    var max = 55290;
                    if (i != 25)
                    {
                        max = areacode[i + 1];
                    }
                    if (areacode[i] <= code && code < max)
                    {
                        return Encoding.Default.GetString(new[] { (byte)(97 + i) });
                    }
                }
                return "*";
            }
            return cnChar;
        }


        public static string GeNewLowtGuid(this Guid guid)
        {
            return guid.ToString("N");

        }


        public static string ToCamelCase(this string str)
        {
            Regex pattern = new Regex(@"[A-Z]{2,}(?=[A-Z][a-z]+[0-9]*|\b)|[A-Z]?[a-z]+[0-9]*|[A-Z]|[0-9]+");
            return new string(
              new CultureInfo("en-US", false)
                .TextInfo
                .ToTitleCase(
                  string.Join(" ", pattern.Matches(str)).ToLower()
                )
                .Replace(@" ", "")
                .Select((x, i) => i == 0 ? char.ToLower(x) : x)
                .ToArray()
            );
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="datetFormat"></param>
        /// <returns></returns>
        public static bool IsDatetFormat(this string datetFormatStr)
        {
            //验证正则
            // string pattern = @"^(?:Y[1-4]|Q[1-2]|M[1-7]|W[1-2]|D[1-7])+$";
            string pattern = "^(?!.*([YQMWD]).*\\1)(?:Y[1-4]|Q[1-2]|M[1-7]|W[1-2]|D[1-7])+";
            Regex regex = new(pattern);
            return regex.IsMatch(datetFormatStr.ToString());

        }

        /// <summary>
        /// 去除字符串中包含的空格
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string DeleteBlankSpace(this string s)
        {
            return s.Replace(" ", "");
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="datetFormat"></param>
        ///// <returns></returns>
        //public static string GetDatetimeCode(this string datetFormatStr) 
        //{
        //    if (datetFormatStr.IsDatetFormat()) 
        //    {
        //        string qDic = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        //        string qdicl = qDic.ToLower();
        //         var nf = new NumberFormater(qDic);
        //        var datetimeNow = DateTime.Now;
        //        var yearStr = datetimeNow.Year.ToString();
        //        var monthVal = datetimeNow.Month;
        //        #region y
        //        string y1 = yearStr[^1].ToString();
        //        string y2 = yearStr[2..];
        //        string y3 = yearStr;
        //        string y4 = nf.ToString(Convert.ToInt32(y2));
        //        #endregion
        //        #region q
        //        var q = (int)Math.Ceiling((double)monthVal / 3);
        //        string q1 = q.ToString();
        //        string q2 = qDic[(9 + q)].ToString();
        //        #endregion

        //        #region m
        //        string m1 = qdicl[monthVal].ToString();

        //        string m2 = m1.ToUpper();
        //        string m3 = qdicl[monthVal + 9].ToString();

        //        string m4 = m3.ToUpper();
        //        string m5 = monthVal.ToString().PadLeft(2, '0');
        //        string m6 = monthVal.GetEnum<MonthType>().ToString();
        //        string m7 = m6.ToUpper();


        //        #endregion
        //        #region w
        //        string w1 = datetimeNow.GetWeekOfYear( CalendarWeekRule.FirstDay ,DayOfWeek.Sunday).ToString().PadLeft(2,'0');
        //        string w2 = datetimeNow.GetWeekOfYear(CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString().PadLeft(2, '0');
        //        #endregion

        //        #region d

        //        string d1 = datetimeNow.Day.ToString().PadLeft(2, '0');
        //        string d2 = datetimeNow.Day.ToString().PadLeft(3, '0');
        //        string d3 = ((int)datetimeNow.DayOfWeek).ToString();
        //        string d4 = ((int)datetimeNow.DayOfWeek+1).ToString();
        //        string d5 = (datetimeNow.DayOfWeek).ToString()[..3].ToCamelCase();
        //        string d6 = (datetimeNow.DayOfWeek).ToString()[..2].ToCamelCase();
        //        string d7 = qDic[datetimeNow.Day].ToString();
        //        #endregion
        //        return datetFormatStr
        //             .Replace("Y1", y1)
        //             .Replace("Y2", y2)
        //             .Replace("Y3", y3)
        //             .Replace("Y4", y4)

        //             .Replace("Q1", q1)
        //             .Replace("Q2", q2)

        //             .Replace("M1", m1)
        //             .Replace("M2", m2)
        //             .Replace("M3", m3)
        //             .Replace("M4", m4)
        //             .Replace("M5", m5)
        //             .Replace("M6", m6)
        //             .Replace("M7", m7)

        //             .Replace("W1", w1)
        //             .Replace("W2", w2)

        //             .Replace("D1", d1)
        //             .Replace("D2", d2)
        //             .Replace("D3", d3)
        //             .Replace("D4", d4)
        //             .Replace("D5", d5)
        //             .Replace("D6", d6)
        //             .Replace("D7", d7);

        //    }
        //    return default;

        //}

        /// <summary>
        /// 用于判断是否为机器人管理员(仅限传入QQ号,wxid,小爱deviceId)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsRobotAdmin(this string s)
        {
            var list = new List<string>();
            #region QQ
            list.Add("923314333");
            list.Add("1716595288");
            #endregion

            #region 微信
            list.Add("wxid_9822978230012");
            list.Add("wxid_209bhgqvdezj22");
            list.Add("wxid_0152821528112");
            list.Add("wxid_l9kxn1qeu9yn22");
            #endregion

            #region 小爱
            //list.Add("45f3127f-6c66-4c74-840c-600237d656a9");//书房
            //list.Add("6ee6c496-be55-4d79-b5c6-615b37b25245");//主卧
            //list.Add("ddbeddb1-3bf0-40d1-919c-fdef83e858c6");//客厅
            list.Add("南京书房");
            list.Add("南京主卧");
            list.Add("南京客厅");
            #endregion

            return list.Contains(s);
        }

        /// <summary>
        /// 用于判断是否全部包含数组内的成员
        /// </summary>
        /// <param name="s"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsContainsAll(this string s, string[] input)
        {
            foreach (string inputItem in input)
            {
                if (!s.Contains(inputItem))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 用于判断是否包含数组内的任一成员
        /// </summary>
        /// <param name="s"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsContainsAny(this string s, string[] input)
        {
            foreach (string inputItem in input)
            {
                if (s.Contains(inputItem))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 根据指定分隔符将string转为List<string>
        /// </summary>
        /// <param name="s"></param>
        /// <param name="separator">分隔符,默认英文逗号</param>
        /// <returns></returns>
        public static List<string> ToStringList(this string s, string separator = ",")
        {
            return s.Split(separator, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        /// <summary>
        /// 将字符转换为dynamic,失败则返回0
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static dynamic ToDynamic(this string s)
        {
            decimal numDecimal;
            if (!decimal.TryParse(s, out numDecimal))
            {
                numDecimal = 0M;
            }
            return numDecimal;
        }

    }
}
