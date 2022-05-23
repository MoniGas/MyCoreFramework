using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace MCF.Lib.Data
{
    public class SQLHelper : ISQLHelper
    {
        public string connectionString = "";

        /// <summary>
        /// 构造函数 得到连接字符串
        /// </summary>
        protected SQLHelper(string connectionString)
        {
            if (connectionString.IsEmpty())
            {
                connectionString = DataManager.GetConnectionString("ConfigName");
            }
            this.connectionString = connectionString;
        }

        /// <summary>
        ///根据ConnectionString中的名字获取本类的实例
        /// </summary>
        public static SQLHelper GetInstenceByConfigName(string configName)
        {
            return new SQLHelper(DataManager.GetConnectionString(configName));
        }

        /// <summary>
        ///根据连接字符串获取本类的实例
        /// </summary>
        public static SQLHelper GetInstenceByConnectionString(string connectionString)
        {
            return new SQLHelper(connectionString);
        }

        /// <summary>
        ///获取连接对象
        /// </summary>
        public SqlConnection GetConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        /// <summary>
        ///获取适配器
        /// </summary>
        public SqlDataAdapter GetDataAdapter(SqlCommand command)
        {
            return new SqlDataAdapter(command);
        }

        /// <summary>
        /// 打开数据库的连接
        /// </summary>
        private SqlConnection OpenConn()
        {
            SqlConnection result;
            try
            {
                SqlConnection connection = GetConnection(connectionString);
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                result = connection;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        /// <summary>
        ///执行更改
        /// </summary>
        public int ExecuteSql(string sql, CommandType commandType, params SqlParameter[] parameters)
        {
            SqlTransaction sqlTransaction = null;
            SqlConnection sqlConnection = null;
            int result = 0;
            try
            {
                sqlConnection = OpenConn();
                sqlTransaction = sqlConnection.BeginTransaction();
                SqlCommand command = sqlConnection.GetCommand(sql, commandType, parameters);
                command.Transaction = sqlTransaction;
                result = command.ExecuteNonQuery();
                sqlTransaction.Commit();
            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                sqlConnection.CloseConn();
            }
            return result;
        }

        /// <summary>
        ///执行更改
        /// </summary>
        public int ExecuteSql(string sql, params SqlParameter[] parameters)
        {
            return ExecuteSql(sql, CommandType.Text, parameters);
        }

        /// <summary>
        ///执行更改
        /// </summary>
        public int ExecuteProc(string sql, params SqlParameter[] parameters)
        {
            return ExecuteSql(sql, CommandType.StoredProcedure, parameters);
        }

        /// <summary>
        ///执行更改 事务内
        /// </summary>
        public int ExecuteSqlTran(List<string> list_sql, List<SqlParameter[]> list_para = null)
        {
            int num = 0;
            SqlConnection sqlConnection = null;
            SqlTransaction sqlTransaction = null;
            try
            {
                sqlConnection = OpenConn();
                sqlTransaction = sqlConnection.BeginTransaction();
                SqlCommand command = sqlConnection.GetCommand();
                command.Transaction = sqlTransaction;
                for (int i = 0; i < list_sql.Count; i++)
                {
                    command.Parameters.Clear();
                    command.CommandText = list_sql[i];
                    if (list_para != null)
                    {
                        SqlParameter[] array = list_para[i];
                        for (int j = 0; j < array.Length; j++)
                        {
                            SqlParameter value = array[j];
                            command.Parameters.Add(value);
                        }
                    }
                    num += command.ExecuteNonQuery();
                }
                sqlTransaction.Commit();
            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                sqlConnection.CloseConn();
            }
            return num;
        }

        public int ExecuteSqlTran(List<Tuple<string, SqlParameter[]>> list_sql)
        {
            List<string> list = new List<string>();
            List<SqlParameter[]> list2 = new List<SqlParameter[]>();
            foreach (Tuple<string, SqlParameter[]> current in list_sql)
            {
                list.Add(current.Item1);
                list2.Add(current.Item2);
            }
            return ExecuteSqlTran(list, list2);
        }

        /// <summary>
        ///获取DataTable
        /// </summary>
        public DataTable FillDataTable(string sql, params SqlParameter[] parameters)
        {
            DataTable dataTable = new DataTable();
            return FillDataSet(sql, parameters).Tables[0];
        }

        /// <summary>
        ///获取DataTable
        /// </summary>
        public DataTable FillDataTableProc(string sql, params SqlParameter[] parameters)
        {
            DataTable dataTable = new DataTable();
            return FillDataSetProc(sql, parameters).Tables[0];
        }

        /// <summary>
        ///获取DataSet
        /// </summary>
        public DataSet FillDataSet(string sql, CommandType commandType, params SqlParameter[] parameters)
        {
            SqlConnection connection = null;
            DataSet dataSet = new DataSet();
            try
            {
                connection = OpenConn();
                SqlCommand command = connection.GetCommand(sql, commandType, parameters);
                SqlDataAdapter dataAdapter = GetDataAdapter(command);
                dataAdapter.Fill(dataSet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.CloseConn();
            }
            return dataSet;
        }

        /// <summary>
        ///获取DataSet
        /// </summary>
        public DataSet FillDataSet(string sql, params SqlParameter[] parameters)
        {
            return FillDataSet(sql, CommandType.Text, parameters);
        }

        /// <summary>
        ///获取DataSet
        /// </summary>
        public DataSet FillDataSetProc(string sql, params SqlParameter[] parameters)
        {
            return FillDataSet(sql, CommandType.StoredProcedure, parameters);
        }

        /// <summary>
        ///获取DataRow 没有则返回null
        /// </summary>
        public DataRow FillDataRow(string sql, params SqlParameter[] parameters)
        {
            DataTable dataTable = FillDataTable(sql, parameters);
            if (dataTable.Rows.Count == 0)
            {
                return null;
            }
            return dataTable.Rows[0];
        }

        public DataRow FillDataRowProc(string sql, params SqlParameter[] parameters)
        {
            DataTable dataTable = FillDataTableProc(sql, parameters);
            if (dataTable.Rows.Count == 0)
            {
                return null;
            }
            return dataTable.Rows[0];
        }

        /// <summary>
        ///获取DataReader
        /// </summary>
        public SqlDataReader ExecuteReader(string sql, params SqlParameter[] parameters)
        {
            SqlConnection connection = null;
            SqlDataReader result = null;
            try
            {
                connection = OpenConn();
                SqlCommand command = connection.GetCommand(sql, CommandType.Text, parameters);
                result = command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                connection.CloseConn();
                throw new Exception(ex.Message);
            }
            return result;
        }

        /// <summary>
        ///获取第一行第一列
        /// </summary>
        public object ExecuteScalar(string sql, params SqlParameter[] parameters)
        {
            SqlConnection connection = null;
            object result = new object();
            try
            {
                connection = OpenConn();
                SqlCommand command = connection.GetCommand(sql, CommandType.Text, parameters);
                result = command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.CloseConn();
            }
            return result;
        }

        /// <summary>
        /// SQLHelper--更新
        /// </summary>
        protected int Update(string tableName, string operates, string where, params SqlParameter[] parameters)
        {
            if (tableName.IsEmpty() || operates.IsEmpty())
            {
                return 0;
            }
            return ExecuteSql(new StringBuilder("UPDATE ").Append(tableName).Append(" SET ").Append(operates).Append(" WHERE 1=1 ").Append(where).ToString(), parameters);
        }

        /// <summary>
        ///获取分页查询SQL
        /// </summary>
        protected virtual string GetPageSql(string tableName, string fields, int page, int pageSize, string orderBy, string where)
        {
            StringBuilder stringBuilder = new StringBuilder("SELECT TOP(").Append(pageSize).Append(") ").Append(fields).Append(" ");
            stringBuilder = stringBuilder.Append(" FROM (SELECT ROW_NUMBER() OVER (ORDER BY ").Append(orderBy).Append(") AS RowNumber,").Append(fields).Append(" FROM ").Append(tableName).Append(" WHERE 1=1 ").Append(where).Append(") TempTable");
            stringBuilder = stringBuilder.Append(" WHERE RowNumber > ").Append(pageSize).Append("*(").Append(page).Append("-1) ;SELECT COUNT(1) FROM ").Append(tableName).Append(" WHERE 1=1 ").Append(where);
            return stringBuilder.ToString();
        }

        /// <summary>
        ///获取分页查询SQL 无获取记录数
        /// </summary>
        protected virtual string GetPageSqlNoTotalCount(string tableName, string fields, int page, int pageSize, string orderBy, string where)
        {
            StringBuilder stringBuilder = new StringBuilder("SELECT TOP(").Append(pageSize).Append(") ").Append(fields).Append(" ");
            stringBuilder = stringBuilder.Append(" FROM (SELECT ROW_NUMBER() OVER (ORDER BY ").Append(orderBy).Append(") AS RowNumber,").Append(fields).Append(" FROM ").Append(tableName).Append(" WHERE 1=1 ").Append(where).Append(") TempTable");
            stringBuilder = stringBuilder.Append(" WHERE RowNumber > ").Append(pageSize).Append("*(").Append(page).Append("-1) ");
            return stringBuilder.ToString();
        }

        /// <summary>
        ///删除
        /// </summary>
        protected int Delete(string tableName, string where, params SqlParameter[] parameters)
        {
            if (tableName.IsEmpty())
            {
                return 0;
            }
            return ExecuteSql(new StringBuilder("DELETE FROM ").Append(tableName).Append(" WHERE 1=1 ").Append(where).ToString(), parameters);
        }

        protected int FakeDeleteByCondition(string tableName, string where, params SqlParameter[] parameters)
        {
            if (tableName.IsEmpty())
            {
                return 0;
            }
            return ExecuteSql(new StringBuilder("Update ").Append(tableName).Append(" SET isDel=1").Append(" WHERE 1=1").Append(where).ToString(), parameters);
        }

        /// <summary>
        ///根据条件获取数据的行数
        /// </summary>
        protected int Count(string tableName, string field, string where, params SqlParameter[] parameters)
        {
            StringBuilder stringBuilder = new StringBuilder("SELECT COUNT(").Append(string.IsNullOrEmpty(field) ? "1" : field).Append(") FROM ").Append(tableName).Append(" WHERE 1=1 ").Append(where);
            return ExecuteScalar(stringBuilder.ToString(), parameters).GetString().GetInt32(0);
        }

        /// <summary>
        ///判断是否存在该条件的数据
        /// </summary>
        protected bool Exists(string tableName, string where, params SqlParameter[] parameters)
        {
            return Count(tableName, "1", where, parameters) > 0;
        }

        /// <summary>
        /// YiYanLib.Data.SQLHelper--获取分页数据
        /// </summary>
        public DataTable GetTablePageForSegment(string segment, int page, int pageSize, string orderBy, ref int totalCount, string where, params SqlParameter[] parameters)
        {
            if (!segment.IsEmpty())
            {
                segment = new StringBuilder("with segment as (").Append(segment).Append(")").ToString();
            }
            string sql = new StringBuilder(segment).Append(GetPageSqlNoTotalCount("segment", "*", page, pageSize, orderBy, where)).Append(";").Append(segment).Append("select count(1) from segment where 1=1 ").Append(where).ToString();
            DataSet dataSet = FillDataSet(sql, parameters);
            totalCount = int.Parse(dataSet.Tables[1].Rows[0][0].ToString());
            return dataSet.Tables[0];
        }

        /// <summary>
        /// YiYanLib.Data.SQLHelper--获取分页数据 并执行其他SQL
        /// </summary>
        /// <returns></returns>
        public DataSet GetTablePageForSegment(string segment, int page, int pageSize, string orderBy, string where, string afterSql, params SqlParameter[] parameters)
        {
            if (!segment.IsEmpty())
            {
                segment = new StringBuilder("with segment as (").Append(segment).Append(")").ToString();
            }
            string sql = new StringBuilder(segment).Append(GetPageSqlNoTotalCount("segment", "*", page, pageSize, orderBy, where)).Append(";").Append(segment).Append("select count(1) from segment where 1=1 ").Append(where).Append(";").Append(afterSql).ToString();
            return FillDataSet(sql, parameters);
        }


    }
}
