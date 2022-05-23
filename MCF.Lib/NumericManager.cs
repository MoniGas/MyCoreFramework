using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MCF.Lib
{
    public static class NumericManager
    {
        /// <summary>
        /// 将字符串转换为Int32,转换错误返回默认值
        /// </summary>
        public static int GetInt32(this string str, int defaultReturn = 0)
        {
            if (str.IsInt32())
            {
                return int.Parse(str.ToString());
            }
            return defaultReturn;
        }

        public static int? GetInt32Nullable(this string obj)
        {
            if (obj.IsInt32())
            {
                return new int?(int.Parse(obj.ToString()));
            }
            return null;
        }

        /// <summary>
        /// 将字符串转换为Float,转换错误返回0
        /// </summary>
        public static float GetFloat(this string obj, float defaultReturn = 0f)
        {
            if (obj.IsFloat())
            {
                return float.Parse(obj.ToString());
            }
            return defaultReturn;
        }

        /// <summary>
        /// 将字符串转换为Decimal,转换错误返回0
        /// </summary>
        public static decimal GetDecimal(this string obj, [DecimalConstant(0, 0, 0u, 0u, 0u)] decimal defaultReturn = default)
        {
            decimal result = 0m;
            if (!decimal.TryParse(obj.GetString(), out result))
            {
                result = defaultReturn;
            }
            return result;
        }

        /// <summary>
        /// 将字符串转换为Double,转换错误返回0
        /// </summary>
        public static double GetDouble(this string obj, double defaultReturn = 0.0)
        {
            double result = 0.0;
            if (!double.TryParse(obj.GetString(), out result))
            {
                result = defaultReturn;
            }
            return result;
        }

        /// <summary>
        /// 判断字符串时候可转换为Int32类型
        /// </summary>
        public static bool IsInt32(this string obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.ToString().Length == 0)
            {
                return false;
            }
            int num = 0;
            return int.TryParse(obj.ToString(), out num);
        }

        /// <summary>
        /// 判断字符串时候可转换为Float类型
        /// </summary>
        public static bool IsFloat(this string obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.ToString().Length == 0)
            {
                return false;
            }
            float num = 0f;
            return float.TryParse(obj.ToString(), out num);
        }

        /// <summary>
        /// 判断字符串时候可转换为Double类型
        /// </summary>
        public static bool IsDouble(this string obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.ToString().Length == 0)
            {
                return false;
            }
            double num = 0.0;
            return double.TryParse(obj.ToString(), out num);
        }

        /// <summary>
        /// 判断字符串时候可转换为Decimal类型
        /// </summary>
        public static bool IsDecimal(this string obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.ToString().Length == 0)
            {
                return false;
            }
            decimal num = 0m;
            return decimal.TryParse(obj.ToString(), out num);
        }
    }
}
