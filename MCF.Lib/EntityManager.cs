using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MCF.Lib
{
    public static class EntityManager
    {
        public static DataTable ConvertDataTable<TEntity>(this List<TEntity> list) where TEntity : class, new()
        {
            DataTable dataTable = new DataTable();
            dataTable.TableName = typeof(TEntity).Name;
            PropertyInfo[] array = (from p in typeof(TEntity).GetProperties()
                                    where p.PropertyType.FullName.StartsWith("System.")
                                    select p).ToArray();
            PropertyInfo[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                PropertyInfo propertyInfo = array2[i];
                dataTable.Columns.Add(new DataColumn(propertyInfo.Name));
            }
            foreach (TEntity current in list)
            {
                DataRow dataRow = dataTable.NewRow();
                PropertyInfo[] array3 = array;
                for (int j = 0; j < array3.Length; j++)
                {
                    PropertyInfo propertyInfo2 = array3[j];
                    dataRow[propertyInfo2.Name] = propertyInfo2.GetValue(current, null);
                }
                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }

        public static TEntity ConvertTEntity<TEntity>(this DataRow dr) where TEntity : class, new()
        {
            PropertyInfo[] properties = typeof(TEntity).GetProperties();
            DataColumnCollection columns = dr.Table.Columns;
            TEntity tEntity = Activator.CreateInstance<TEntity>();
            PropertyInfo[] array = properties;
            for (int i = 0; i < array.Length; i++)
            {
                PropertyInfo propertyInfo = array[i];
                if (columns.Contains(propertyInfo.Name))
                {
                    if (dr[propertyInfo.Name] == DBNull.Value)
                    {
                        propertyInfo.SetValue(tEntity, null, null);
                    }
                    else
                    {
                        propertyInfo.SetValue(tEntity, dr[propertyInfo.Name], null);
                    }
                }
            }
            return tEntity;
        }

        public static List<TEntity> ConvertTEntity<TEntity>(this DataTable dt) where TEntity : class, new()
        {
            PropertyInfo[] properties = typeof(TEntity).GetProperties();
            DataColumnCollection columns = dt.Columns;
            List<TEntity> list = new List<TEntity>();
            foreach (DataRow dataRow in dt.Rows)
            {
                TEntity tEntity = Activator.CreateInstance<TEntity>();
                PropertyInfo[] array = properties;
                for (int i = 0; i < array.Length; i++)
                {
                    PropertyInfo propertyInfo = array[i];
                    if (columns.Contains(propertyInfo.Name))
                    {
                        if (dataRow[propertyInfo.Name] == DBNull.Value)
                        {
                            propertyInfo.SetValue(tEntity, null, null);
                        }
                        else
                        {
                            propertyInfo.SetValue(tEntity, dataRow[propertyInfo.Name], null);
                        }
                    }
                }
                list.Add(tEntity);
            }
            return list;
        }

        /// <summary>
        /// 复制实体 不可用于Linq操作
        /// </summary>
        public static TEntity CopyEntity<TEntity>(this TEntity e) where TEntity : class, new()
        {
            List<PropertyInfo> list = typeof(TEntity).GetProperties().ToList();
            TEntity tEntity = Activator.CreateInstance<TEntity>();
            foreach (PropertyInfo current in list)
            {
                current.SetValue(tEntity, current.GetValue(e, null), null);
            }
            return tEntity;
        }
    }
}
