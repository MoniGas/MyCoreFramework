using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCF.Lib
{
    public static class ObjectManager
    {
        /// <summary>
        /// 将对象转换为String
        /// </summary>
        public static string GetString(this object obj)
        {
            if (obj.IsEmpty())
            {
                return "";
            }
            return obj.ToString();
        }

        /// <summary>
        /// 将对象转换为String  并trim
        /// </summary>
        public static string GetTrimString(this object obj, params char[] trimChars)
        {
            return obj.GetString().Trim(trimChars);
        }

        /// <summary>
        /// 判断对象是否为null 如果是String 同时判断是否为空字符串
        /// </summary>
        public static bool IsEmpty(this object obj)
        {
            return obj == null || string.IsNullOrEmpty(obj.ToString().Trim());
        }

        /// <summary>
        /// 判断对象是否不为null 如果是String 同时判断是否不为空字符串
        /// </summary>
        public static bool IsNotEmpty(this object obj)
        {
            return !obj.IsEmpty();
        }
    }
}
