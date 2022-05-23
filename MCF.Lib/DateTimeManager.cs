using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCF.Lib
{
    public static class DateTimeManager
    {
        /// <summary>
        /// 将字符串转换为时间格式,转换错误返回默认时间
        /// </summary>
        public static DateTime GetDateTime(this string str, DateTime defaultReturn)
        {
            if (str.IsDateTime())
            {
                return DateTime.Parse(str.ToString().Trim());
            }
            return defaultReturn;
        }

        /// <summary>
        /// 将字符串转换为时间格式,转换错误返回当前时间
        /// </summary>
        public static DateTime GetDateTime(this string str)
        {
            return str.GetDateTime(DateTime.Now);
        }

        /// <summary>
        /// 将字符串转换为时间格式,转换错误返回null
        /// </summary>
        public static DateTime? GetDateTimeNullable(this string str)
        {
            if (str.IsDateTime())
            {
                return new DateTime?(str.GetDateTime(DateTime.Now));
            }
            return null;
        }

        public static string Format(this DateTime? datetime, string format = "yyyy-MM-dd")
        {
            if (!datetime.HasValue)
            {
                return "";
            }
            return datetime.Value.Format(format);
        }

        public static string Format(this DateTime datetime, string format = "yyyy-MM-dd")
        {
            return datetime.ToString(format);
        }

        public static DateTime GetMinTime(this DateTime datetime)
        {
            return DateTime.Parse(datetime.ToString("yyyy-MM-dd 00:00:00"));
        }

        public static DateTime GetMaxTime(this DateTime datetime)
        {
            return DateTime.Parse(datetime.ToString("yyyy-MM-dd 23:59:59"));
        }

        /// <summary>
        /// 判断字符串是否为日期格式
        /// </summary>
        public static bool IsDateTime(this string obj)
        {
            DateTime dateTime;
            return obj != null && !string.IsNullOrEmpty(obj.ToString()) && DateTime.TryParse(obj.ToString().Trim(), out dateTime);
        }
    }
}
