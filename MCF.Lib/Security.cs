using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MCF.Lib
{
    public class Security
    {
        private static byte[] KeysAES = new byte[]
        {
            65,
            114,
            101,
            121,
            111,
            117,
            109,
            121,
            83,
            110,
            111,
            119,
            109,
            97,
            110,
            63
        };

        /// <summary>
        /// 使用MD5算法加密（不可逆，无法解密）
        /// 把此方法加入CS页面然后直接调用就行了
        /// </summary>
        /// <param name="password">明文</param>
        /// <returns>密文</returns>
        public static string EncryptMD5(string password)
        {
            byte[] bytes = Encoding.Default.GetBytes(password.Trim());
            MD5 mD = new MD5CryptoServiceProvider();
            byte[] value = mD.ComputeHash(bytes);
            return BitConverter.ToString(value).Replace("-", "");
        }

        public static string EncryptAES(string encryptString, string encryptKey)
        {
            encryptKey = GetSubString(encryptKey, 32, "");
            encryptKey = encryptKey.PadRight(32, ' ');
            ICryptoTransform cryptoTransform = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 32)),
                IV = KeysAES
            }.CreateEncryptor();
            byte[] bytes = Encoding.UTF8.GetBytes(encryptString);
            byte[] inArray = cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);
            return Convert.ToBase64String(inArray);
        }

        public static string DecryptAES(string decryptString, string decryptKey)
        {
            string result;
            try
            {
                decryptKey = GetSubString(decryptKey, 32, "");
                decryptKey = decryptKey.PadRight(32, ' ');
                ICryptoTransform cryptoTransform = new RijndaelManaged
                {
                    Key = Encoding.UTF8.GetBytes(decryptKey),
                    IV = KeysAES
                }.CreateDecryptor();
                byte[] array = Convert.FromBase64String(decryptString);
                byte[] bytes = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
                result = Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                result = "";
            }
            return result;
        }

        /// <summary>
        /// 字符串如果操过指定长度则将超出的部分用指定字符串代替
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        private static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
        {
            return GetSubString(p_SrcString, 0, p_Length, p_TailString);
        }

        /// <summary>
        /// 取指定长度的字符串
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_StartIndex">起始位置</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        private static string GetSubString(string p_SrcString, int p_StartIndex, int p_Length, string p_TailString)
        {
            string text = p_SrcString;
            byte[] bytes = Encoding.UTF8.GetBytes(p_SrcString);
            char[] chars = Encoding.UTF8.GetChars(bytes);
            for (int i = 0; i < chars.Length; i++)
            {
                char c = chars[i];
                if (c > 'ࠀ' && c < '一' || c > '가' && c < '힣')
                {
                    string result;
                    if (p_StartIndex >= p_SrcString.Length)
                    {
                        result = "";
                    }
                    else
                    {
                        result = p_SrcString.Substring(p_StartIndex, p_Length + p_StartIndex > p_SrcString.Length ? p_SrcString.Length - p_StartIndex : p_Length);
                    }
                    return result;
                }
            }
            if (p_Length >= 0)
            {
                byte[] bytes2 = Encoding.Default.GetBytes(p_SrcString);
                if (bytes2.Length > p_StartIndex)
                {
                    int num = bytes2.Length;
                    if (bytes2.Length > p_StartIndex + p_Length)
                    {
                        num = p_Length + p_StartIndex;
                    }
                    else
                    {
                        p_Length = bytes2.Length - p_StartIndex;
                        p_TailString = "";
                    }
                    int num2 = p_Length;
                    int[] array = new int[p_Length];
                    int num3 = 0;
                    for (int j = p_StartIndex; j < num; j++)
                    {
                        if (bytes2[j] > 127)
                        {
                            num3++;
                            if (num3 == 3)
                            {
                                num3 = 1;
                            }
                        }
                        else
                        {
                            num3 = 0;
                        }
                        array[j] = num3;
                    }
                    if (bytes2[num - 1] > 127 && array[p_Length - 1] == 1)
                    {
                        num2 = p_Length + 1;
                    }
                    byte[] array2 = new byte[num2];
                    Array.Copy(bytes2, p_StartIndex, array2, 0, num2);
                    text = Encoding.Default.GetString(array2);
                    text += p_TailString;
                }
            }
            return text;
        }
    }
}
