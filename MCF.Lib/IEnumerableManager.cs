using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCF.Lib
{
    public static class IEnumerableManager
    {
        /// <summary>
        /// WltLib.Linq.IEnumerableManager返回实体，不存在返回null
        /// </summary>
        public static TEntity Entity<TEntity>(this IEnumerable<TEntity> ie, Func<TEntity, bool> predicate) where TEntity : class, new()
        {
            return ie.Where(predicate).FirstOrDefault();
        }

        /// <summary>
        /// WltLib.Linq.IEnumerableManager返回实体，不存在返回新的实体
        /// </summary>
        public static TEntity SelectEntity<TEntity>(this IEnumerable<TEntity> ie, Func<TEntity, bool> predicate) where TEntity : class, new()
        {
            TEntity tEntity = ie.Entity(predicate);
            if (tEntity == null)
            {
                return Activator.CreateInstance<TEntity>();
            }
            return tEntity;
        }

        /// <summary>
        /// WltLib.Linq.IEnumerableManager返回查询结果列表
        /// </summary>
        public static List<TEntity> SelectPage<TEntity>(this IEnumerable<TEntity> ie, int pageNum, int pageSize = 10) where TEntity : class, new()
        {
            if (pageNum <= 0)
            {
                pageNum = 1;
            }
            return ie.Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// WltLib.Linq.IEnumerableManager判断实体是否存在
        /// </summary>
        public static bool Exists<TEntity>(this IEnumerable<TEntity> ie, Func<TEntity, bool> predicate) where TEntity : class, new()
        {
            return ie.Count(predicate) != 0;
        }
    }
}
