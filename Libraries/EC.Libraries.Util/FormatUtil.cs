using System;
using System.Threading;

namespace EC.Libraries.Util
{
    /// <summary>
    /// 字符格式化类
    /// </summary>
    public static class FormatUtil
    {
        /// <summary>
        /// 将数字转换为当前语言货币格式
        /// </summary>
        /// <param name="currency">数字</param>
        /// <param name="precision">精度,默认两位</param>
        /// <returns></returns>
        public static string ToCurrentCurrency(this decimal currency, int precision=2)
        {
            return currency.ToString("C"+precision, Thread.CurrentThread.CurrentUICulture);
        }

        /// <summary>
        /// 将数字转换为当前语言货币格式
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="precision">精度,默认两位</param>
        public static string ToCurrentCurrency(this int currency, int precision = 2)
        {
            return currency.ToString("C" + precision, Thread.CurrentThread.CurrentUICulture);
        }

        /// <summary>
        /// 将数字转换为当前语言货币格式
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="precision">精度,默认两位</param>
        public static string ToCurrentCurrency(this double currency, int precision = 2)
        {
            return currency.ToString("C" + precision, Thread.CurrentThread.CurrentUICulture);
        }

        /// <summary>
        /// 将时间转换为当前语言时间格式
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format">标准或自定义日期和时间格式的字符串</param>
        public static string ToCurrentDate(this DateTime date,string format="f")
        {
            return date.ToString(format,Thread.CurrentThread.CurrentUICulture);
        }
    }
}
