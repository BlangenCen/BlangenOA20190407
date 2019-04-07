using BlangenOA.DAL;
using BlangenOA.IDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlangenOA.DALFactory
{
    public partial class DbSession : IDbSession
    {
        //线程内EF唯一实例
        public DbContext Db
        {
            get
            {
                return DbContextFactory.CreateDbContext();
            }
        }
        //public IUserInfoDal CreateUserInfoDal()
        //{
        //    return new UserInfoDal();
        //}
        //public IUserInfoDal _userInfoDal;
        //public IUserInfoDal UserInfoDal
        //{
        //    get => _userInfoDal ?? AbstractFactory.CreateUserInfoDal();
        //    set => _userInfoDal = value;

        //}
        /// <summary>
        /// 一次连接，全部保存
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            return Db.SaveChanges() > 0;
        }
    }
}
