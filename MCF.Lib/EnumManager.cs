using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MCF.Lib
{
    public class EnumManager
    {
        static EnumManager()
        {
        }

        /// <summary>
        /// 获取枚举类列表,三列 Id,Name,Value
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public static DataTable GetList(Type enumType)
        {
            DataTable dataTable = new DataTable(enumType.ToString());
            dataTable.Columns.Add(new DataColumn("Id", typeof(int)));
            dataTable.Columns.Add(new DataColumn("Value", typeof(int)));
            dataTable.Columns.Add(new DataColumn("Name", typeof(string)));
            FieldInfo[] fields = enumType.GetFields();
            for (int i = 1; i < fields.Length; i++)
            {
                dataTable.LoadDataRow(new object[]
                {
                    i,
                    fields[i].GetRawConstantValue(),
                    fields[i].Name
                }, true);
            }
            return dataTable;
        }

        /// <summary>
        /// 根据枚举中的数值获取其对应的属性值
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="id">数值</param>
        /// <returns>返回ColorEnum.Color 中的"Color"</returns>
        public static string GetName(Type enumType, int value)
        {
            DataTable list = GetList(enumType);
            DataRow[] array = list.Select("Value=" + value);
            if (array.Length > 0)
            {
                return array[0]["Name"].ToString();
            }
            return "";
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ValueIsExists(Type enumType, int value)
        {
            DataTable list = GetList(enumType);
            DataRow[] array = list.Select("Value=" + value);
            return array.Length > 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool NameIsExists(Type enumType, string name)
        {
            DataTable list = GetList(enumType);
            DataRow[] array = list.Select("name='" + name + "'");
            return array.Length > 0;
        }

        /// <summary>
        /// 根据枚举中的名称获取其对应的数值
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="name">名称(不区分大小写)</param>
        /// <returns>返回ColorEnum.Color 的int.Parase(ColorEnum.Color)的值</returns>
        public static int GetValue(Type enumType, string name)
        {
            DataTable list = GetList(enumType);
            DataRow[] array = list.Select("name='" + name + "'");
            if (array.Length > 0)
            {
                return array[0]["Value"].GetTrimString(new char[0]).GetInt32(0);
            }
            return -1;
        }
    }
}
