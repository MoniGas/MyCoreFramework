using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using MCF.Lib;

namespace MCF.Lib.Data
{
    public static class DataManager
    {
        /// <summary>
        /// WltLib.Data.DataManager--将DataReader转化成DataTable
        /// </summary>
        public static DataTable ConvertDataReaderToDataTable(SqlDataReader dataReader)
        {
            DataTable dataTable = new DataTable();
            try
            {
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    DataColumn dataColumn = new DataColumn();
                    dataColumn.DataType = dataReader.GetFieldType(i);
                    dataColumn.ColumnName = dataReader.GetName(i);
                    dataTable.Columns.Add(dataColumn);
                }
                while (dataReader.Read())
                {
                    DataRow dataRow = dataTable.NewRow();
                    for (int j = 0; j < dataReader.FieldCount; j++)
                    {
                        dataRow[j] = dataReader[j].ToString();
                    }
                    dataTable.Rows.Add(dataRow);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dataReader.Close();
            }
            return dataTable;
        }

        /// <summary>
        /// 根据ConnectionStrings的名字获取连接字符串
        /// </summary>
        public static string GetConnectionString(string name)
        {
            return AppSettingsJson.Configuration.GetConnectionString(name);
        }

        /// <summary>
        /// WltLib.Data.DataManager--绑定Parameter
        /// </summary>
        public static SqlCommand BindParameters(this SqlCommand comm, List<SqlParameter> parameters)
        {
            return comm.BindParameters(parameters.ToArray());
        }

        /// <summary>
        /// WltLib.Data.DataManager--绑定Parameter
        /// </summary>
        public static SqlCommand BindParameters(this SqlCommand comm, params SqlParameter[] parameters)
        {
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    SqlParameter sqlParameter = parameters[i];
                    sqlParameter.IsNullable = true;
                    if (sqlParameter.Value == null)
                    {
                        sqlParameter.Value = DBNull.Value;
                    }
                    comm.Parameters.Add(sqlParameter);
                }
            }
            return comm;
        }

        /// <summary>
        /// WltLib.Data.DataManager--获取DataRow中索引的值
        /// </summary>
        public static string GetValue(this DataRow dr, int index = 0)
        {
            if (dr.Table.Columns.Count > index)
            {
                return dr[index].GetTrimString(new char[0]);
            }
            return "";
        }

        /// <summary>
        /// WltLib.Data.DataManager--获取DataRow中列的值
        /// </summary>
        public static string GetValue(this DataRow dr, string colName)
        {
            if (dr.Table.Columns.Contains(colName))
            {
                return dr[colName].GetTrimString(new char[0]);
            }
            return "";
        }

        /// <summary>
        /// WltLib.Data.DataManager--获取DataTable中行列的值
        /// </summary>
        public static string GetValue(this DataTable dt, int row = 0, int index = 0)
        {
            if (dt.Rows.Count > row && dt.Columns.Count > index)
            {
                return dt.Rows[row][index].GetTrimString(new char[0]);
            }
            return "";
        }

        /// <summary>
        /// WltLib.Data.DataManager--获取DataTable中行列的值
        /// </summary>
        public static string GetValue(this DataTable dt, int row = 0, string colName = "")
        {
            if (dt.Rows.Count > row && dt.Columns.Contains(colName))
            {
                return dt.Rows[row][colName].GetTrimString(new char[0]);
            }
            return "";
        }

        /// <summary>
        /// 关闭数据库的连接
        /// </summary>
        internal static void CloseConn(this SqlConnection connection)
        {
            try
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// WltLib.Data.SQLHelper--获取命令对象
        /// </summary>
        public static SqlCommand GetCommand(this SqlConnection connection)
        {
            return new SqlCommand
            {
                Connection = connection
            };
        }

        /// <summary>
        /// 构造SqlCommand对象
        /// </summary>
        public static SqlCommand GetCommand(this SqlConnection connection, string sql, CommandType commandType, params SqlParameter[] parameters)
        {
            SqlCommand command = connection.GetCommand();
            command.CommandText = sql;
            command.CommandType = commandType;
            command.BindParameters(parameters);
            command.Connection = connection;
            return command;
        }

        /// <summary>
        /// 在事物中执行数据库操作
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool ExecuteMultiTran(List<Tuple<SqlConnection, List<Tuple<string, SqlParameter[]>>>> list, ref Exception info)
        {
            bool flag = true;
            List<SqlConnection> list2 = new List<SqlConnection>();
            List<SqlTransaction> list3 = new List<SqlTransaction>();
            try
            {
                foreach (Tuple<SqlConnection, List<Tuple<string, SqlParameter[]>>> current in list)
                {
                    SqlConnection item = current.Item1;
                    if (item.State != ConnectionState.Open)
                    {
                        item.Open();
                    }
                    SqlTransaction sqlTransaction = item.BeginTransaction();
                    list2.Add(item);
                    list3.Add(sqlTransaction);
                    foreach (Tuple<string, SqlParameter[]> current2 in current.Item2)
                    {
                        if (!current2.Item1.IsEmpty())
                        {
                            SqlCommand sqlCommand = new SqlCommand();
                            sqlCommand.Connection = item;
                            sqlCommand.Transaction = sqlTransaction;
                            sqlCommand.Parameters.Clear();
                            sqlCommand.CommandText = current2.Item1;
                            if (current2.Item2 != null)
                            {
                                SqlParameter[] item2 = current2.Item2;
                                for (int i = 0; i < item2.Length; i++)
                                {
                                    SqlParameter value = item2[i];
                                    sqlCommand.Parameters.Add(value);
                                }
                            }
                            sqlCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                info = ex;
                flag = false;
            }
            if (flag)
            {
                using (List<SqlTransaction>.Enumerator enumerator3 = list3.GetEnumerator())
                {
                    while (enumerator3.MoveNext())
                    {
                        SqlTransaction current3 = enumerator3.Current;
                        current3.Commit();
                    }
                    goto IL_1A1;
                }
            }
            foreach (SqlTransaction current4 in list3)
            {
                current4.Rollback();
            }
        IL_1A1:
            foreach (SqlConnection current5 in list2)
            {
                if (current5.State != ConnectionState.Closed)
                {
                    current5.Close();
                }
            }
            return flag;
        }

        public static bool ExecuteMultiTran(List<Tuple<SqlConnection, Tuple<string, SqlParameter[]>>> list, ref Exception info)
        {
            return ExecuteMultiTran((from p in list
                                     select new Tuple<SqlConnection, List<Tuple<string, SqlParameter[]>>>(p.Item1, new List<Tuple<string, SqlParameter[]>>
            {
                p.Item2
            })).ToList<Tuple<SqlConnection, List<Tuple<string, SqlParameter[]>>>>(), ref info);
        }

        /// <summary>
        /// 将一组事务的语句转换成一个语句
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static Tuple<string, List<SqlParameter>> GetSqlForTran(List<Tuple<string, SqlParameter[]>> list)
        {
            string text = "";
            List<SqlParameter> list2 = new List<SqlParameter>();
            foreach (Tuple<string, SqlParameter[]> current in list)
            {
                string id = Guid.NewGuid().ToString().Replace("-", "");
                text = text + current.Item1.Replace("@", "@_" + id + "_") + ";";
                list2.AddRange(from p in current.Item2
                               select new SqlParameter(p.ParameterName.Replace("@", "@_" + id + "_"), p.Value));
            }
            return new Tuple<string, List<SqlParameter>>(text, list2);
        }
    }
}
