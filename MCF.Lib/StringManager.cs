using MCF.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MCF.Lib
{
    public static class StringManager
    {
        public static StringBuilder ToBuilder(this string str)
        {
            return new StringBuilder(str);
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string Split(this string str, char separator, int index = 0)
        {
            if (str.IsEmpty())
            {
                return "";
            }
            if (!str.Contains(separator))
            {
                return "";
            }
            if (str.Split(new char[]
            {
                separator
            }).Length < index + 1)
            {
                return "";
            }
            return str.Split(new char[]
            {
                separator
            })[index];
        }

        /// <summary>
        /// 将第Index个位置的字符转换成小写
        /// </summary>
        /// <param name="str"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string ToLower(this string str, params int[] index)
        {
            if (index.Length == 0)
            {
                return str.ToLower();
            }
            char[] array = str.ToCharArray();
            str = "";
            for (int i = 0; i < array.Length; i++)
            {
                if (index.Contains(i + 1))
                {
                    str += array[i].ToString().ToLower();
                }
                else
                {
                    str += array[i];
                }
            }
            return str;
        }

        /// <summary>
        /// 将第Index个位置的字符转换成大写
        /// </summary>
        public static string ToUpper(this string str, params int[] index)
        {
            if (index.Length == 0)
            {
                return str.ToUpper();
            }
            char[] array = str.ToCharArray();
            str = "";
            for (int i = 0; i < array.Length; i++)
            {
                if (index.Contains(i + 1))
                {
                    str += array[i].ToString().ToUpper();
                }
                else
                {
                    str += array[i];
                }
            }
            return str;
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="str">要截取的字符串</param>
        /// <param name="startIndex">开始位置</param>
        /// <param name="length">截取长度</param>
        /// <param name="isMarket">是否标记</param>
        /// <param name="marketStr">标记字符串</param>
        /// <param name="isCompletion">是否补全</param>
        /// <param name="completionStr">补全字符串</param>
        /// <returns>完成的字符串</returns>
        public static string SubString(string str, int startIndex, int length, bool isMarket, string marketStr, bool isCompletion, string completionStr)
        {
            string text = str.GetString();
            if (text.Length - startIndex > length)
            {
                text = text.Substring(startIndex, length);
                if (isMarket)
                {
                    if (marketStr.IsEmpty())
                    {
                        marketStr = "...";
                    }
                    text += marketStr;
                }
            }
            else
            {
                text = text.Remove(0, startIndex);
                if (isCompletion)
                {
                    if (completionStr.IsEmpty())
                    {
                        completionStr = " ";
                    }
                    int num = length - text.Length;
                    for (int i = 0; i < num; i++)
                    {
                        text += completionStr;
                    }
                }
            }
            return text;
        }

        public static string SubString(this string str, int startIndex, int length, bool isMarket = true, string marketStr = "...")
        {
            if (str == null)
            {
                str = "";
            }
            if (str.Length - startIndex > length)
            {
                str = str.Substring(startIndex, length);
                if (isMarket)
                {
                    str += marketStr;
                }
            }
            else
            {
                str = str.Remove(0, startIndex);
            }
            return str;
        }

        public static string SubString(this string str, int startIndex, bool isMarket = true, string marketStr = "...")
        {
            return str.SubString(0, str.Length, isMarket, marketStr);
        }

        public static string CompletionString(string str, int length, string completionStr)
        {
            if (completionStr.IsEmpty())
            {
                completionStr = " ";
            }
            int num = length - str.Length;
            for (int i = 0; i < num; i++)
            {
                str += completionStr;
            }
            return str;
        }

        /// <summary>
        /// 检查字符串长度汉字与非汉字混合的情况
        /// </summary>
        /// <param name="str">要判断的字符串</param>
        /// <returns>字符串长度</returns>
        public static int CharLength(string str)
        {
            int num = 0;
            if (!str.IsEmpty())
            {
                char[] array = str.ToString().ToCharArray();
                for (int i = 0; i < array.Length; i++)
                {
                    if (Convert.ToInt32(array[i]) > 255)
                    {
                        num += 2;
                    }
                    else
                    {
                        num++;
                    }
                }
            }
            return num;
        }

        /// <summary>
        /// 源字符串是否包含指定字符串
        /// </summary>
        /// <param name="source_str">源字符串</param>
        /// <param name="comparison_str">指定字符串</param>
        /// <param name="split">分隔符</param>
        /// <returns>结果</returns>
        public static bool Contains(this string source_str, string comparison_str, char split)
        {
            return comparison_str.IsEmpty() || source_str.Contains(split + comparison_str + split) || source_str.StartsWith(comparison_str + split) || source_str.EndsWith(split + comparison_str) || source_str == comparison_str;
        }

        /// <summary>
        /// ASCII码转字符
        /// </summary>
        /// <param name="asciiCode"></param>
        /// <returns></returns>
        public static string IntToChar(int asciiCode)
        {
            if (asciiCode >= 0 && asciiCode <= 255)
            {
                ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
                byte[] bytes = new byte[]
                {
                    (byte)asciiCode
                };
                return aSCIIEncoding.GetString(bytes);
            }
            throw new Exception("ASCII Code is not valid.");
        }

        /// <summary>
        /// 得到汉字首字母
        /// </summary>
        /// <param name="paramChinese"></param>
        /// <returns></returns>
        public static string GetFirstLetter(string paramChinese)
        {
            string text = "";
            int length = paramChinese.Length;
            for (int i = 0; i <= length - 1; i++)
            {
                text += GetCharSpellCode(paramChinese.Substring(i, 1));
            }
            return text;
        }

        /// <summary>    
        /// 得到一个汉字的拼音第一个字母，如果是一个英文字母则直接返回大写字母    
        /// </summary>    
        /// <param name="CnChar">单个汉字</param>    
        /// <returns>单个大写字母</returns>    
        private static string GetCharSpellCode(string paramChar)
        {
            byte[] bytes = Encoding.Default.GetBytes(paramChar);
            if (bytes.Length == 1)
            {
                return paramChar.ToUpper();
            }
            int num = bytes[0];
            int num2 = bytes[1];
            long num3 = num * 256 + num2;
            if (num3 >= 45217L && num3 <= 45252L)
            {
                return "A";
            }
            if (num3 >= 45253L && num3 <= 45760L)
            {
                return "B";
            }
            if (num3 >= 45761L && num3 <= 46317L)
            {
                return "C";
            }
            if (num3 >= 46318L && num3 <= 46825L)
            {
                return "D";
            }
            if (num3 >= 46826L && num3 <= 47009L)
            {
                return "E";
            }
            if (num3 >= 47010L && num3 <= 47296L)
            {
                return "F";
            }
            if (num3 >= 47297L && num3 <= 47613L)
            {
                return "G";
            }
            if (num3 >= 47614L && num3 <= 48118L)
            {
                return "H";
            }
            if (num3 >= 48119L && num3 <= 49061L)
            {
                return "J";
            }
            if (num3 >= 49062L && num3 <= 49323L)
            {
                return "K";
            }
            if (num3 >= 49324L && num3 <= 49895L)
            {
                return "L";
            }
            if (num3 >= 49896L && num3 <= 50370L)
            {
                return "M";
            }
            if (num3 >= 50371L && num3 <= 50613L)
            {
                return "N";
            }
            if (num3 >= 50614L && num3 <= 50621L)
            {
                return "O";
            }
            if (num3 >= 50622L && num3 <= 50905L)
            {
                return "P";
            }
            if (num3 >= 50906L && num3 <= 51386L)
            {
                return "Q";
            }
            if (num3 >= 51387L && num3 <= 51445L)
            {
                return "R";
            }
            if (num3 >= 51446L && num3 <= 52217L)
            {
                return "S";
            }
            if (num3 >= 52218L && num3 <= 52697L)
            {
                return "T";
            }
            if (num3 >= 52698L && num3 <= 52979L)
            {
                return "W";
            }
            if (num3 >= 52980L && num3 <= 53688L)
            {
                return "X";
            }
            if (num3 >= 53689L && num3 <= 54480L)
            {
                return "Y";
            }
            if (num3 >= 54481L && num3 <= 55289L)
            {
                return "Z";
            }
            return "?";
        }

        /// <returns>已经去除后的文字</returns>
        public static string ClearHTML(string Htmlstring)
        {
            Htmlstring = Regex.Replace(Htmlstring, "<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "([\\r\\n])[\\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(iexcl|#161);", "¡", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(cent|#162);", "¢", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(pound|#163);", "£", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(copy|#169);", "©", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&#(\\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            return Htmlstring;
        }
    }
}
