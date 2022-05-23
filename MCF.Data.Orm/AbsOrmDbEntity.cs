using MCF.Lib.Data;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace MCF.Data.Orm
{
    public abstract class AbsOrmDbEntity : SQLHelper, IOrmDbEntity, ISQLHelper
    {
        public AbsOrmDbEntity(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        ///  更新
        /// </summary>
        public abstract int Update<TEntity>(string operates, string where, params SqlParameter[] parameters) where TEntity : EntityBase, new();

        /// <summary>
        ///  更新
        /// </summary>
        public abstract int Update<TEntity>(TEntity entity) where TEntity : EntityBase, new();

        /// <summary>
        ///  插入
        /// </summary>
        public abstract int Insert<TEntity>(TEntity entity) where TEntity : EntityBase, new();
        /// <summary>
        ///  删除
        /// </summary>
        public abstract int Delete<TEntity>(TEntity entity) where TEntity : EntityBase, new();

        /// <summary>
        ///  删除
        /// </summary>
        public abstract int Delete<TEntity>(string where, params SqlParameter[] parameters) where TEntity : EntityBase, new();

        /// <summary>
        /// 假删除(根据主键)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public abstract int FakeDeleteById<TEntity>(TEntity entity) where TEntity : EntityBase, new();

        /// <summary>
        /// 假删除(根据条件)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public abstract int FakeDeleteByCondition<TEntity>(string where, params SqlParameter[] parameters) where TEntity : EntityBase, new();

        /// <summary>
        ///  判断 是否存在该条件的实体
        /// </summary>
        public abstract bool Exists<TEntity>(string where, params SqlParameter[] parameters) where TEntity : EntityBase, new();

        /// <summary>
        ///  判断 是否存在该条件的实体
        /// </summary>
        public abstract bool Exists<TEntity>(TEntity entity) where TEntity : EntityBase, new();

        /// <summary>
        ///  查询 并返回DataTable
        /// </summary>
        public abstract DataTable SelectDataTable<TEntity>(string where, int top, params SqlParameter[] parameters) where TEntity : EntityBase, new();

        /// <summary>
        ///  查询100 并返回DataTable
        /// </summary>
        public abstract DataTable SelectDataTable<TEntity>(string where, params SqlParameter[] parameters) where TEntity : EntityBase, new();

        /// <summary>
        /// 分页查询 并返回DataTable
        /// </summary>
        public abstract DataTable SelectPageDataTable<TEntity>(int page, int pageSize, string orderBy, ref int totalCount, string where, params SqlParameter[] parameters) where TEntity : EntityBase, new();

        /// <summary>
        /// 分页查询 并返回DataTable
        /// </summary>
        public abstract DataTable SelectPageDataTable<TEntity>(int page, int pageSize, string orderBy, string where, params SqlParameter[] parameters) where TEntity : EntityBase, new();

        /// <summary>
        ///  查询 并返回List
        /// </summary>
        public abstract List<TEntity> Select<TEntity>(string where, int top, params SqlParameter[] parameters) where TEntity : EntityBase, new();

        /// <summary>
        ///  查询100 并返回List
        /// </summary>
        public abstract List<TEntity> Select<TEntity>(string where, params SqlParameter[] parameters) where TEntity : EntityBase, new();

        /// <summary>
        ///  返回数量
        /// </summary>
        public abstract int Count<TEntity>(string where, params SqlParameter[] parameters) where TEntity : EntityBase, new();

        /// <summary>
        /// 分页查询 并返回List
        /// </summary>
        public abstract List<TEntity> SelectPage<TEntity>(int page, int pageSize, string orderBy, ref int totalCount, string where, params SqlParameter[] parameters) where TEntity : EntityBase, new();

        /// <summary>
        /// 分页查询 并返回List
        /// </summary>
        public abstract List<TEntity> SelectPage<TEntity>(int page, int pageSize, string orderBy, string where, params SqlParameter[] parameters) where TEntity : EntityBase, new();

        /// <summary>
        ///  查询 并返回DataRow 没有返回null
        /// </summary>
        public abstract DataRow EntityDataRow<TEntity>(string where, params SqlParameter[] parameters) where TEntity : EntityBase, new();

        /// <summary>
        ///  查询 并返回TEntity 如果没有则返回null
        /// </summary>
        public abstract TEntity Entity<TEntity>(string where, params SqlParameter[] parameters) where TEntity : EntityBase, new();

        /// <summary>
        ///  查询 并返回TEntity 如果没有则返回新的对象
        /// </summary>
        public abstract TEntity SelectEntity<TEntity>(string where, params SqlParameter[] parameters) where TEntity : EntityBase, new();

        /// <summary>
        ///  获取插入SQL
        /// </summary>
        public abstract string GetInsertSql<TEntity>() where TEntity : EntityBase, new();

        /// <summary>
        ///  获取更新SQL
        /// </summary>
        public abstract string GetUpdateSql<TEntity>() where TEntity : EntityBase, new();

        /// <summary>
        ///  获取删除SQL
        /// </summary>
        public abstract string GetDeleteSql<TEntity>() where TEntity : EntityBase, new();

        /// <summary>
        ///  获取查询SQL
        /// </summary>
        public abstract string GetInsertSql<TEntity>(TEntity entity) where TEntity : EntityBase, new();

        /// <summary>
        ///  获取更新SQL
        /// </summary>
        public abstract string GetUpdateSql<TEntity>(TEntity entity) where TEntity : EntityBase, new();

        /// <summary>
        ///  获取删除SQL
        /// </summary>
        public abstract string GetDeleteSql<TEntity>(TEntity entity) where TEntity : EntityBase, new();

        /// <summary>
        /// 获取假删除的SQL(根据主键)
        /// 其本质是一条UPDATE语句，改变删除状态为1，暂定字段为isDel
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public abstract string GetFakeDeleteSqlById<TEntity>(TEntity entity) where TEntity : EntityBase, new();

        /// <summary>
        ///  获取查询SQL
        /// </summary>
        public abstract string GetSelectSql<TEntity>(int top = 0) where TEntity : EntityBase, new();

        /// <summary>
        ///  获取分页查询SQL
        /// </summary>
        public abstract string GetPageSql<TEntity>(int page, int pageSize, string orderBy, string where) where TEntity : EntityBase, new();

        /// <summary>
        ///  获取Parameters集合
        /// </summary>
        public abstract SqlParameter[] GetParameters<TEntity>(TEntity entity) where TEntity : EntityBase, new();

        /// <summary>
        ///  获取TEntity的TableName
        /// </summary>
        public abstract string GetTableName<TEntity>() where TEntity : EntityBase, new();

        public abstract PropertyInfo[] GetColumnPropertyInfo<TEntity>();
    }
}
