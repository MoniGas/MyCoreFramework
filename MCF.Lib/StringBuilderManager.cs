using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCF.Lib
{
    public static class StringBuilderManager
    {
        public static void AppendSelf(this StringBuilder sb, string value)
        {
            sb = sb.Append(value);
        }

        public static void AppendSelfFormat(this StringBuilder sb, string format, params object[] args)
        {
            sb = sb.AppendFormat(format, args);
        }

        public static void InsertSelf(this StringBuilder sb, int index, string value)
        {
            sb = sb.Insert(index, value);
        }

        public static void RemoveSelf(this StringBuilder sb, int startIndex, int length)
        {
            sb = sb.Remove(startIndex, length);
        }

        public static void ReplaceSelf(this StringBuilder sb, string oldChar, string newChar)
        {
            sb = sb.Replace(oldChar, newChar);
        }
    }
}
