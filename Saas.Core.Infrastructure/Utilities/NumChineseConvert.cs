using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Saas.Core.Infrastructure.Utilities
{
    public class NumChineseConvert
    {
        /// <summary>
        /// 阿拉伯数字转换成中文数字
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public string NumToChinese(string x)
        {
            string[] pArrayNum = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
            //为数字位数建立一个位数组
            string[] pArrayDigit = { "", "十", "百", "千" };
            //为数字单位建立一个单位数组
            string[] pArrayUnits = { "", "万", "亿", "万亿" };
            var pStrReturnValue = ""; //返回值
            var finger = 0; //字符位置指针
            var pIntM = x.Length % 4; //取模
            int pIntK;
            if (pIntM > 0)
                pIntK = x.Length / 4 + 1;
            else
                pIntK = x.Length / 4;
            //外层循环,四位一组,每组最后加上单位: ",万亿,",",亿,",",万,"
            for (var i = pIntK; i > 0; i--)
            {
                var pIntL = 4;
                if (i == pIntK && pIntM != 0)
                    pIntL = pIntM;
                //得到一组四位数
                var four = x.Substring(finger, pIntL);
                var P_int_l = four.Length;
                //内层循环在该组中的每一位数上循环
                for (int j = 0; j < P_int_l; j++)
                {
                    //处理组中的每一位数加上所在的位
                    int n = Convert.ToInt32(four.Substring(j, 1));
                    if (n == 0)
                    {
                        if (j < P_int_l - 1 && Convert.ToInt32(four.Substring(j + 1, 1)) > 0 && !pStrReturnValue.EndsWith(pArrayNum[n]))
                            pStrReturnValue += pArrayNum[n];
                    }
                    else
                    {
                        if (!(n == 1 && (pStrReturnValue.EndsWith(pArrayNum[0]) | pStrReturnValue.Length == 0) && j == P_int_l - 2))
                            pStrReturnValue += pArrayNum[n];
                        pStrReturnValue += pArrayDigit[P_int_l - j - 1];
                    }
                }
                finger += pIntL;
                //每组最后加上一个单位:",万,",",亿," 等
                if (i < pIntK) //如果不是最高位的一组
                {
                    if (Convert.ToInt32(four) != 0)
                        //如果所有4位不全是0则加上单位",万,",",亿,"等
                        pStrReturnValue += pArrayUnits[i - 1];
                }
                else
                {
                    //处理最高位的一组,最后必须加上单位
                    pStrReturnValue += pArrayUnits[i - 1];
                }
            }
            return pStrReturnValue;
        }


        /// <summary>
        /// 转换数字
        /// </summary>
        protected static long CharToNumber(char c)
        {
            switch (c)
            {
                case '一': return 1;
                case '二': return 2;
                case '三': return 3;
                case '四': return 4;
                case '五': return 5;
                case '六': return 6;
                case '七': return 7;
                case '八': return 8;
                case '九': return 9;
                case '零': return 0;
                default: return -1;
            }
        }

        /// <summary>
        /// 转换单位
        /// </summary>
        protected static long CharToUnit(char c)
        {
            switch (c)
            {
                case '十': return 10;
                case '百': return 100;
                case '千': return 1000;
                case '万': return 10000;
                case '亿': return 100000000;
                default: return 1;
            }
        }
        /// <summary>
        /// 将中文数字转换阿拉伯数字
        /// </summary>
        /// <param name="cnum">汉字数字</param>
        /// <returns>长整型阿拉伯数字</returns>
        public static long ParseCnToInt(string cnum)
        {
            cnum = Regex.Replace(cnum, "\\s+", "");
            long firstUnit = 1;//一级单位
            long secondUnit = 1;//二级单位
            long result = 0;//结果
            for (var i = cnum.Length - 1; i > -1; --i)//从低到高位依次处理
            {
                var tmpUnit = CharToUnit(cnum[i]);//临时单位变量
                if (tmpUnit > firstUnit)//判断此位是数字还是单位
                {
                    firstUnit = tmpUnit;//是的话就赋值,以备下次循环使用
                    secondUnit = 1;
                    if (i == 0)//处理如果是"十","十一"这样的开头的
                    {
                        result += firstUnit * secondUnit;
                    }
                    continue;//结束本次循环
                }
                if (tmpUnit > secondUnit)
                {
                    secondUnit = tmpUnit;
                    continue;
                }
                result += firstUnit * secondUnit * CharToNumber(cnum[i]);//如果是数字,则和单位想乘然后存到结果里
            }
            return result;
        }

    }
}
