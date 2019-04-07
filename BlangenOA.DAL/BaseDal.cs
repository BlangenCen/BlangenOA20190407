using BlangenOA.IDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlangenOA.DAL
{
    public class BaseDal<T>
        where T : class, new()
    {
        /// <summary>
        /// 线程内唯一EF实例
        /// </summary>
        public DbContext Db
        {
            get
            {
                return DbContextFactory.CreateDbContext();
            }
        }
        /// <summary>
        /// 增
        /// </summary>
        /// <param name="entity">增加的对象</param>
        /// <returns></returns>
        public T AddEntity(T entity)
        {
            Db.Entry(entity).State = EntityState.Added;
            return entity;
        }
        /// <summary>
        /// 删
        /// </summary>
        /// <param name="entity">删除的对象</param>
        /// <returns></returns>
        public bool DeleteEntity(T entity)
        {
            Db.Entry(entity).State = EntityState.Deleted;
            return true;
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="entity">编辑后的对象</param>
        /// <returns></returns>
        public T EditEntity(T entity)
        {
            Db.Entry(entity).State = EntityState.Modified;
            return entity;
        }
        /// <summary>
        /// 查
        /// </summary>
        /// <param name="entity">查询的对象</param>
        /// <returns></returns>
        public T FindEntity(T entity)
        {
            return Db.Set<T>().Find(entity);
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="whereLambda">Lambda表达式</param>
        /// <returns></returns>
        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda)
        {
            return Db.Set<T>().Where(whereLambda);
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="K">按K排序</typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页尺寸</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="isAsync">是否升序</param>
        /// <param name="whereLambda">数据表达式</param>
        /// <param name="orderByLambda">排序表达式</param>
        /// <returns></returns>
        public IQueryable<T> LoadPageEntities<K>(int pageIndex, int pageSize, out int pageCount, bool isAsc, Expression<Func<T, bool>> whereLambda, Expression<Func<T, K>> orderByLambda)
        {
            var temp = Db.Set<T>().Where(whereLambda);
            pageCount = temp.Count();
            if (isAsc)
            {
                temp = temp.OrderBy(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else
            {
                temp = temp.OrderByDescending(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            return temp;
        }
    }
}
