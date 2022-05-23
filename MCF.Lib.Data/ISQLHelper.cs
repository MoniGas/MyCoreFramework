using System.Data;
using System.Data.SqlClient;

namespace MCF.Lib.Data
{
    public interface ISQLHelper
    {
        /// <summary>
        /// 获取连接对象
        /// </summary>
        SqlConnection GetConnection(string connectionString);

        /// <summary>
        /// 获取适配器
        /// </summary>
        SqlDataAdapter GetDataAdapter(SqlCommand command);

        /// <summary>
        /// WltLib.Data.SQLHelper--执行更改
        /// </summary>
        int ExecuteSql(string sql, CommandType commandType, params SqlParameter[] parameters);

        /// <summary>
        /// WltLib.Data.SQLHelper--执行更改
        /// </summary>
        int ExecuteSql(string sql, params SqlParameter[] parameters);

        /// <summary>
        /// WltLib.Data.SQLHelper--执行更改
        /// </summary>
        int ExecuteProc(string sql, params SqlParameter[] parameters);

        /// <summary>
        /// 执行更改 事务内
        /// </summary>
        int ExecuteSqlTran(List<string> list_sql, List<SqlParameter[]> list_para = null);

        /// <summary>
        /// 执行更改 事务内
        /// </summary>
        int ExecuteSqlTran(List<Tuple<string, SqlParameter[]>> list_sql);

        /// <summary>
        /// 获取DataTable
        /// </summary>
        DataTable FillDataTable(string sql, params SqlParameter[] parameters);

        /// <summary>
        /// 获取DataTable
        /// </summary>
        DataTable FillDataTableProc(string sql, params SqlParameter[] parameters);

        /// <summary>
        /// 获取DataSet
        /// </summary>
        DataSet FillDataSet(string sql, CommandType commandType, params SqlParameter[] parameters);

        /// <summary>
        /// 获取DataSet
        /// </summary>
        DataSet FillDataSet(string sql, params SqlParameter[] parameters);

        /// <summary>
        /// 获取DataSet
        /// </summary>
        DataSet FillDataSetProc(string sql, params SqlParameter[] parameters);

        /// <summary>
        /// 获取DataRow 没有则返回null
        /// </summary>
        DataRow FillDataRow(string sql, params SqlParameter[] parameters);

        /// <summary>
        /// 获取DataRow 没有则返回null
        /// </summary>
        DataRow FillDataRowProc(string sql, params SqlParameter[] parameters);

        /// <summary>
        /// 获取DataReader
        /// </summary>
        SqlDataReader ExecuteReader(string sql, params SqlParameter[] parameters);

        /// <summary>
        /// 获取第一行第一列
        /// </summary>
        object ExecuteScalar(string sql, params SqlParameter[] parameters);

        /// <summary>
        /// YiYanLib.Data.SQLHelper--获取分页数据
        /// </summary>
        DataTable GetTablePageForSegment(string segment, int page, int pageSize, string orderBy, ref int totalCount, string where, params SqlParameter[] parameters);

        DataSet GetTablePageForSegment(string segment, int page, int pageSize, string orderBy, string where, string afterSql, params SqlParameter[] parameters);
    }
}
