using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlangenOA.IDAL
{
    public interface IBaseDal<T>
        where T : class, new()
    {
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
