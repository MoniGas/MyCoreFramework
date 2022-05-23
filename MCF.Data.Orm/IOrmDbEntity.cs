using MCF.Lib.Data;
using System.Data;
using System.Data.SqlClient;

namespace MCF.Data.Orm
{
    public interface IOrmDbEntity : ISQLHelper
    {
        /// <summary>
        ///  更新
        /// </summary>
        int Update<TEntity>(string operates, string where, params SqlParameter[] parameters) where TEntity : EntityBase, new();

        /// <summary>
        ///  更新
        /// </summary>
        int Update<TEntity>(TEntity entity) where TEntity : EntityBase, new();

        /// <summary>
        ///  插入
        /// </summary>
        int Insert<TEntity>(TEntity entity) where TEntity : EntityBase, new();

        /// <summary>
        ///  删除
        /// </summary>
        int Delete<TEntity>(TEntity entity) where TEntity : EntityBase, new();

        /// <summary>
        ///  删除
        /// </summary>
        int Delete<TEntity>(string where, params SqlParameter[] parameters) where TEntity : EntityBase, new();

        /// <summary>
        ///  判断 是否存在该条件的实体
        /// </summary>
        bool Exists<TEntity>(string where, params SqlParameter[] parameters) where TEntity : EntityBase, new();

        /// <summary>
        ///  判断 是否存在该条件的实体
        /// </summary>
        bool Exists<TEntity>(TEntity entity) where TEntity : EntityBase, new();

        /// <summary>
        ///  查询 并返回DataTable
        /// </summary>
        DataTable SelectDataTable<TEntity>(string where, int top, params SqlParameter[] parameters) where TEntity : EntityBase, new();

        /// <summary>
        ///  查询100 并返回DataTable
        /// </summary>
        DataTable SelectDataTable<TEntity>(string where, params SqlParameter[] parameters) where TEntity : EntityBase, new();

        /// <summary>
        /// YiYanLib.Data.OrmHelper 分页查询 并返回DataTable
        /// </summary>
        DataTable SelectPageDataTable<TEntity>(int page, int pageSize, string orderBy, ref int totalCount, string where, params SqlParameter[] parameters) where TEntity : EntityBase, new();

        /// <summary>
        /// YiYanLib.Data.OrmHelper 分页查询 并返回DataTable
        /// </summary>
        DataTable SelectPageDataTable<TEntity>(int page, int pageSize, string orderBy, string where, params SqlParameter[] parameters) where TEntity : EntityBase, new();

        /// <summary>
        ///  查询 并返回List
        /// </summary>
        List<TEntity> Select<TEntity>(string where, int top, params SqlParameter[] parameters) where TEntity : EntityBase, new();

        /// <summary>
        ///  查询100 并返回List
        /// </summary>
        List<TEntity> Select<TEntity>(string where, params SqlParameter[] parameters) where TEntity : EntityBase, new();

        /// <summary>
        ///  返回数量
        /// </summary>
        int Count<TEntity>(string where, params SqlParameter[] parameters) where TEntity : EntityBase, new();

        /// <summary>
        /// YiYanLib.Data.OrmHelper 分页查询 并返回List
        /// </summary>
        List<TEntity> SelectPage<TEntity>(int page, int pageSize, string orderBy, ref int totalCount, string where, params SqlParameter[] parameters) where TEntity : EntityBase, new();

        /// <summary>
        /// YiYanLib.Data.OrmHelper 分页查询 并返回List
        /// </summary>
        List<TEntity> SelectPage<TEntity>(int page, int pageSize, string orderBy, string where, params SqlParameter[] parameters) where TEntity : EntityBase, new();

        /// <summary>
        ///  查询 并返回DataRow 没有返回null
        /// </summary>
        DataRow EntityDataRow<TEntity>(string where, params SqlParameter[] parameters) where TEntity : EntityBase, new();

        /// <summary>
        ///  查询 并返回TEntity 如果没有则返回null
        /// </summary>
        TEntity Entity<TEntity>(string where, params SqlParameter[] parameters) where TEntity : EntityBase, new();

        /// <summary>
        ///  查询 并返回TEntity 如果没有则返回新的对象
        /// </summary>
        TEntity SelectEntity<TEntity>(string where, params SqlParameter[] parameters) where TEntity : EntityBase, new();

        /// <summary>
        ///  获取查询SQL
        /// </summary>
        string GetInsertSql<TEntity>() where TEntity : EntityBase, new();

        /// <summary>
        ///  获取更新SQL
        /// </summary>
        string GetUpdateSql<TEntity>() where TEntity : EntityBase, new();

        /// <summary>
        ///  获取删除SQL
        /// </summary>
        string GetDeleteSql<TEntity>() where TEntity : EntityBase, new();

        /// <summary>
        ///  获取查询SQL
        /// </summary>
        string GetInsertSql<TEntity>(TEntity entity) where TEntity : EntityBase, new();

        /// <summary>
        ///  获取更新SQL
        /// </summary>
        string GetUpdateSql<TEntity>(TEntity entity) where TEntity : EntityBase, new();

        /// <summary>
        ///  获取删除SQL
        /// </summary>
        string GetDeleteSql<TEntity>(TEntity entity) where TEntity : EntityBase, new();

        /// <summary>
        ///  获取查询SQL
        /// </summary>
        string GetSelectSql<TEntity>(int top = 0) where TEntity : EntityBase, new();

        /// <summary>
        ///  获取分页查询SQL
        /// </summary>
        string GetPageSql<TEntity>(int page, int pageSize, string orderBy, string where) where TEntity : EntityBase, new();

        /// <summary>
        ///  获取Parameters集合
        /// </summary>
        SqlParameter[] GetParameters<TEntity>(TEntity entity) where TEntity : EntityBase, new();

        /// <summary>
        ///  获取TEntity的TableName
        /// </summary>
        string GetTableName<TEntity>() where TEntity : EntityBase, new();
    }
}
