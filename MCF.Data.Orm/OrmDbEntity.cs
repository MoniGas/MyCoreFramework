using MCF.Lib;
using MCF.Lib.Data;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace MCF.Data.Orm
{
    public class OrmDbEntity : AbsOrmDbEntity
    {
        public OrmDbEntity(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// 更新
        /// </summary>
        public override int Update<TEntity>(TEntity entity)
        {
            string updateSql = GetUpdateSql(entity);
            return ExecuteSql(updateSql, GetParameters(entity));
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        public override int Update<TEntity>(string operates, string where, params SqlParameter[] parameters)
        {
            return Update(GetTableName<TEntity>(), operates, where, parameters);
        }

        /// <summary>
        /// 插入
        /// </summary>
        public override int Insert<TEntity>(TEntity entity)
        {
            string insertSql = GetInsertSql(entity);
            return ExecuteSql(insertSql, GetParameters(entity));
        }

        /// <summary>
        /// 批量插入大数据量（万以上)
        /// </summary>
        /// <param name="dt">要传入的DataTable对象</param>
        /// <param name="DestinationTableName">要插入的目标表</param>
        /// <param name="BatchSize">写入数据库一批数量,如果为0代表全部一次性插入</param>
        /// <param name="BulkCopyTimeout">超时时间,默认30s</param>
        public async void BulkCopyAsync(DataTable dt, string DestinationTableName, int BatchSize, int BulkCopyTimeout)
        {
            await Task.Delay(100);
            var conn = GetConnection(connectionString);
            SqlBulkCopyOptions options = SqlBulkCopyOptions.KeepIdentity | SqlBulkCopyOptions.KeepNulls;//批量插入时是数据库自增、自己指定数据的null值
            var sbc = new SqlBulkCopy(conn, options, null);
            sbc.DestinationTableName = DestinationTableName;//默认插入表与实体名称一致
            sbc.BatchSize = BatchSize;//为0代表全部一次性插入
            sbc.BulkCopyTimeout = BulkCopyTimeout;//超时时间
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sbc.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
            }
            sbc.WriteToServer(dt);//将数据批量写入数据库
            conn.Close();
        }

        /// <summary>
        /// 删除
        /// </summary>
        public override int Delete<TEntity>(TEntity entity)
        {
            string deleteSql = GetDeleteSql(entity);
            return ExecuteSql(deleteSql, GetParameters(entity));
        }

        /// <summary>
        /// 删除
        /// </summary>
        public override int Delete<TEntity>(string where, params SqlParameter[] parameters)
        {
            return Delete(GetTableName<TEntity>(), where, parameters);
        }

        /// <summary>
        /// 假删除(根据主键)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override int FakeDeleteById<TEntity>(TEntity entity)
        {
            string fakeDelete = GetFakeDeleteSqlById(entity);
            return ExecuteSql(fakeDelete, GetParameters(entity));
        }

        /// <summary>
        /// 假删除(根据条件)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override int FakeDeleteByCondition<TEntity>(string where, params SqlParameter[] parameters)
        {
            return FakeDeleteByCondition(GetTableName<TEntity>(), where, parameters);
        }

        /// <summary>
        /// 判断 是否存在该条件的实体
        /// </summary>
        public override bool Exists<TEntity>(string where, params SqlParameter[] parameters)
        {
            return Exists(GetTableName<TEntity>(), where, parameters);
        }

        /// <summary>
        /// 判断 是否存在该条件的实体
        /// </summary>
        public override bool Exists<TEntity>(TEntity entity)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendSelf(" and ");
            stringBuilder.AppendSelf(entity.GetPkField());
            stringBuilder.AppendSelf("='");
            stringBuilder.AppendSelf((from p in GetParameters(entity)
                                      where p.ParameterName == new StringBuilder("@").Append(entity.GetPkField()).ToString()
                                      select p).FirstOrDefault().Value.GetString());
            stringBuilder.AppendSelf("'");
            return Exists<TEntity>(stringBuilder.ToString(), new SqlParameter[0]);
        }

        /// <summary>
        /// 查询 并返回DataTable
        /// </summary>
        public override DataTable SelectDataTable<TEntity>(string where, int top, params SqlParameter[] parameters)
        {
            string sql = GetSelectSql<TEntity>(top) + where;
            return FillDataTable(sql, parameters);
        }

        /// <summary>
        /// 查询100 并返回DataTable
        /// </summary>
        public override DataTable SelectDataTable<TEntity>(string where, params SqlParameter[] parameters)
        {
            return SelectDataTable<TEntity>(where, 100, parameters);
        }

        /// <summary>
        /// 分页查询 并返回DataTable
        /// </summary>
        public override DataTable SelectPageDataTable<TEntity>(int page, int pageSize, string orderBy, ref int totalCount, string where, params SqlParameter[] parameters)
        {
            string pageSql = GetPageSql<TEntity>(page, pageSize, orderBy, where);
            DataSet dataSet = FillDataSet(pageSql, parameters);
            totalCount = dataSet.Tables[1].GetValue(0, 0).GetInt32(0);
            return dataSet.Tables[0];
        }

        /// <summary>
        /// 分页查询 并返回DataTable
        /// </summary>
        public override DataTable SelectPageDataTable<TEntity>(int page, int pageSize, string orderBy, string where, params SqlParameter[] parameters)
        {
            int num = 0;
            return SelectPageDataTable<TEntity>(page, pageSize, orderBy, ref num, where, parameters);
        }

        /// <summary>
        /// 查询 并返回List
        /// </summary>
        public override List<TEntity> Select<TEntity>(string where, int top, params SqlParameter[] parameters)
        {
            DataTable dataTable = SelectDataTable<TEntity>(where, top, parameters);
            List<TEntity> list = new List<TEntity>();
            foreach (DataRow dr in dataTable.Rows)
            {
                list.Add(dr.ConvertTEntity<TEntity>());
            }
            return list;
        }

        /// <summary>
        /// 查询100 并返回List
        /// </summary>
        public override List<TEntity> Select<TEntity>(string where, params SqlParameter[] parameters)
        {
            return Select<TEntity>(where, 100000, parameters);
        }

        /// <summary>
        /// 返回数量
        /// </summary>
        public override int Count<TEntity>(string where, params SqlParameter[] parameters)
        {
            return Count(GetTableName<TEntity>(), "1", where, parameters);
        }

        /// <summary>
        /// 分页查询 并返回List
        /// </summary>
        public override List<TEntity> SelectPage<TEntity>(int page, int pageSize, string orderBy, ref int totalCount, string where, params SqlParameter[] parameters)
        {
            DataTable dataTable = SelectPageDataTable<TEntity>(page, pageSize, orderBy, ref totalCount, where, parameters);
            List<TEntity> list = new List<TEntity>();
            foreach (DataRow dr in dataTable.Rows)
            {
                list.Add(dr.ConvertTEntity<TEntity>());
            }
            return list;
        }

        /// <summary>
        /// 分页查询 并返回List
        /// </summary>
        public override List<TEntity> SelectPage<TEntity>(int page, int pageSize, string orderBy, string where, params SqlParameter[] parameters)
        {
            int num = 0;
            return SelectPage<TEntity>(page, pageSize, orderBy, ref num, where, parameters);
        }

        /// <summary>
        /// 查询 并返回DataRow 没有返回null
        /// </summary>
        public override DataRow EntityDataRow<TEntity>(string where, params SqlParameter[] parameters)
        {
            string sql = GetSelectSql<TEntity>(1) + where;
            return FillDataRow(sql, parameters);
        }

        /// <summary>
        /// 查询 并返回TEntity 如果没有则返回null
        /// </summary>
        public override TEntity Entity<TEntity>(string where, params SqlParameter[] parameters)
        {
            DataRow dataRow = EntityDataRow<TEntity>(where, parameters);
            if (dataRow != null)
            {
                return dataRow.ConvertTEntity<TEntity>();
            }
            return default;
        }

        /// <summary>
        /// 查询 并返回TEntity 如果没有则返回新的对象
        /// </summary>
        public override TEntity SelectEntity<TEntity>(string where, params SqlParameter[] parameters)
        {
            TEntity tEntity = Entity<TEntity>(where, parameters);
            if (tEntity != null)
            {
                return tEntity;
            }
            return Activator.CreateInstance<TEntity>();
        }

        /// <summary>
        /// 获取查询SQL
        /// </summary>
        public override string GetInsertSql<TEntity>()
        {
            return GetInsertSql(Activator.CreateInstance<TEntity>());
        }

        /// <summary>
        /// 获取更新SQL
        /// </summary>
        public override string GetUpdateSql<TEntity>()
        {
            return GetUpdateSql(Activator.CreateInstance<TEntity>());
        }

        /// <summary>
        /// 获取删除SQL
        /// </summary>
        public override string GetDeleteSql<TEntity>()
        {
            return GetDeleteSql(Activator.CreateInstance<TEntity>());
        }

        /// <summary>
        /// 获取插入SQL
        /// </summary>
        public override string GetInsertSql<TEntity>(TEntity entity)
        {
            PropertyInfo[] columnPropertyInfo = GetColumnPropertyInfo<TEntity>();
            string idenField = entity.GetIdenField();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendSelf(string.Join(",", from p in columnPropertyInfo
                                                      where p.Name.ToUpper() != idenField.ToUpper()
                                                      select new StringBuilder("[").Append(p.Name).Append("]").ToString()));
            StringBuilder stringBuilder2 = new StringBuilder();
            stringBuilder2.AppendSelf(string.Join(",", from p in columnPropertyInfo
                                                       where p.Name.ToUpper() != idenField.ToUpper()
                                                       select new StringBuilder("@").Append(p.Name).ToString()));
            StringBuilder stringBuilder3 = new StringBuilder("INSERT INTO ");
            stringBuilder3.AppendSelf(GetTableName<TEntity>());
            stringBuilder3.AppendSelf(" ( ");
            stringBuilder3.AppendSelf(stringBuilder.ToString());
            stringBuilder3.AppendSelf(" ) VALUES ( ");
            stringBuilder3.AppendSelf(stringBuilder2.ToString());
            stringBuilder3.AppendSelf(" )");
            return stringBuilder3.ToString();
        }

        /// <summary>
        /// 获取更新SQL
        /// </summary>
        public override string GetUpdateSql<TEntity>(TEntity entity)
        {
            PropertyInfo[] columnPropertyInfo = GetColumnPropertyInfo<TEntity>();
            string idenField = entity.GetIdenField();
            string pkField = entity.GetPkField();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendSelf("UPDATE ");
            stringBuilder.AppendSelf(GetTableName<TEntity>());
            stringBuilder.AppendSelf(" SET ");
            stringBuilder.AppendSelf(string.Join(",", from p in columnPropertyInfo
                                                      where p.Name.ToUpper() != idenField.ToUpper() && p.Name != pkField.ToUpper()
                                                      select new StringBuilder("[").Append(p.Name).Append("]=@").Append(p.Name).ToString()));
            stringBuilder.AppendSelf(" WHERE ");
            stringBuilder.AppendSelf(pkField);
            stringBuilder.AppendSelf("=@");
            stringBuilder.AppendSelf(pkField);
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 获取假删除语句（根据主键）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string GetFakeDeleteSqlById<TEntity>(TEntity entity)
        {
            string idenField = entity.GetIdenField();
            string pkField = entity.GetPkField();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendSelf("UPDATE ");
            stringBuilder.AppendSelf(GetTableName<TEntity>());
            stringBuilder.AppendSelf(" SET IsDel=1 ");
            stringBuilder.AppendSelf(" WHERE ");
            stringBuilder.AppendSelf(pkField);
            stringBuilder.AppendSelf("=@");
            stringBuilder.AppendSelf(pkField);
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 获取删除SQL
        /// </summary>
        public override string GetDeleteSql<TEntity>(TEntity entity)
        {
            StringBuilder stringBuilder = new StringBuilder("delete ");
            stringBuilder.AppendSelf(GetTableName<TEntity>());
            stringBuilder.AppendSelf(" where ");
            stringBuilder.AppendSelf(entity.GetPkField());
            stringBuilder.AppendSelf("=@");
            stringBuilder.AppendSelf(entity.GetPkField());
            return stringBuilder.ToString();
        }



        /// <summary>
        /// 获取查询SQL
        /// </summary>
        public override string GetSelectSql<TEntity>(int top = 0)
        {
            StringBuilder stringBuilder = new StringBuilder("select ");
            stringBuilder.AppendSelf(top == 0 ? "" : new StringBuilder("top(").Append(top).Append(")").ToString());
            stringBuilder.AppendSelf(" * from ");
            stringBuilder.AppendSelf(GetTableName<TEntity>());
            stringBuilder.AppendSelf(" where 1=1");
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 获取分页查询SQL
        /// </summary>
        public override string GetPageSql<TEntity>(int page, int pageSize, string orderBy, string where)
        {
            return GetPageSql(GetTableName<TEntity>(), "*", page, pageSize, orderBy, where);
        }

        /// <summary>
        /// 获取Parameters集合
        /// </summary>
        public override SqlParameter[] GetParameters<TEntity>(TEntity entity)
        {
            PropertyInfo[] columnPropertyInfo = GetColumnPropertyInfo<TEntity>();
            List<SqlParameter> list = new List<SqlParameter>();
            list = (from p in columnPropertyInfo
                    select new SqlParameter(new StringBuilder("@").Append(p.Name).ToString(), p.GetValue(entity, null) == null ? DBNull.Value : p.GetValue(entity, null))).ToList();
            return list.ToArray();
        }

        /// <summary>
        /// 获取TEntity的TableName
        /// </summary>
        public override string GetTableName<TEntity>()
        {
            TEntity tEntity = Activator.CreateInstance<TEntity>();
            return tEntity.GetTableName();
        }

        public override PropertyInfo[] GetColumnPropertyInfo<TEntity>()
        {
            return typeof(TEntity).GetProperties();
        }
    }
}
