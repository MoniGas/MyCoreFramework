using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MCF.Lib
{
    public static class IQueryableManager
    {
        /// <summary>
        ///  返回实体，不存在返回null
        /// </summary>
        public static TEntity Entity<TEntity>(this IQueryable<TEntity> iq, Expression<Func<TEntity, bool>> predicate) where TEntity : class, new()
        {
            return iq.Select(predicate).FirstOrDefault();
        }

        /// <summary>
        ///  返回实体，不存在返回新的实体
        /// </summary>
        public static TEntity SelectEntity<TEntity>(this IQueryable<TEntity> iq, Expression<Func<TEntity, bool>> predicate) where TEntity : class, new()
        {
            TEntity tEntity = iq.Entity(predicate);
            if (tEntity == null)
            {
                return Activator.CreateInstance<TEntity>();
            }
            return tEntity;
        }

        /// <summary>
        ///  判断实体是否存在
        /// </summary>
        public static bool Exists<TEntity>(this IQueryable<TEntity> iq, Expression<Func<TEntity, bool>> predicate) where TEntity : class, new()
        {
            return iq.Count(predicate) != 0;
        }

        /// <summary>
        ///  返回查询结果列表
        /// </summary>
        public static IQueryable<TEntity> Select<TEntity>(this IQueryable<TEntity> iq, Expression<Func<TEntity, bool>> predicate) where TEntity : class, new()
        {
            return iq.Where(predicate);
        }

        /// <summary>
        ///  分页返回查询结果列表
        /// </summary>
        public static List<TEntity> SelectPage<TEntity>(this IQueryable<TEntity> iq, int pageNum, int pageSize = 10) where TEntity : class, new()
        {
            int num = 0;
            return iq.SelectPage(ref num, pageNum, pageSize);
        }

        /// <summary>
        ///  分页返回查询结果列表
        /// </summary>
        public static List<TEntity> SelectPage<TEntity>(this IQueryable<TEntity> iq, ref int totalCount, int pageNum, int pageSize = 10) where TEntity : class, new()
        {
            totalCount = iq.Count();
            if (pageNum <= 0)
            {
                pageNum = 1;
            }
            return iq.Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
