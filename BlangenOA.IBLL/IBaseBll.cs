using BlangenOA.DALFactory;
using BlangenOA.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlangenOA.IBLL
{
    public interface IBaseBll<T>
        where T : class, new()
    {
        IDbSession DbSession { get; }
        IBaseDal<T> CurrentDal { get; set; }
        //CRUD
        T AddEntity(T entity);
        bool DeleteEntity(T entity);
        T EditEntity(T entity);
        //数据展示用
        IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda);
        //分页用
        IQueryable<T> LoadPageEntities<K>(int pageIndex, int pageSize, out int pageCount, bool isAsc, Expression<Func<T, bool>> whereLambda, Expression<Func<T, K>> orderByLambda);
    }
}
