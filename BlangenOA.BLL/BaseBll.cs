using BlangenOA.DALFactory;
using BlangenOA.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlangenOA.BLL
{
    public abstract class BaseBll<T>
        where T : class, new()
    {
        /// <summary>
        /// 线程内唯一DbSession
        /// </summary>
        public IDbSession DbSession
        {
            get => DbSessionFactory.CreateDbSession();
        }
        /// <summary>
        /// 当前Dal
        /// </summary>
        public IBaseDal<T> CurrentDal { get; set; }
        /// <summary>
        /// 设置当前Dal
        /// </summary>
        public abstract void SetCurrentDal();
        /// <summary>
        /// 当子类创建对象的时候设置Dal
        /// </summary>
        public BaseBll()
        {
            SetCurrentDal();
        }
        /// <summary>
        /// 增
        /// </summary>
        /// <param name="entity">增加的对象</param>
        /// <returns></returns>
        public T AddEntity(T entity)
        {
            CurrentDal.AddEntity(entity);
            return DbSession.SaveChanges() ? entity : null;
        }
        /// <summary>
        /// 删
        /// </summary>
        /// <param name="entity">删除的对象</param>
        /// <returns></returns>
        public bool DeleteEntity(T entity)
        {
            CurrentDal.DeleteEntity(entity);
            return DbSession.SaveChanges();
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="entity">编辑后的对象</param>
        /// <returns></returns>
        public T EditEntity(T entity)
        {
            CurrentDal.EditEntity(entity);
            return DbSession.SaveChanges() ? entity : null;
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="whereLambda">Lambda表达式</param>
        /// <returns></returns>
        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda)
        {
            return CurrentDal.LoadEntities(whereLambda);
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
            return CurrentDal.LoadPageEntities(pageIndex, pageSize, out pageCount, isAsc, whereLambda, orderByLambda);
        }
    }
}
