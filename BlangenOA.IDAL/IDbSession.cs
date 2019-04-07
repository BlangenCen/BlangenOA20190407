using BlangenOA.IDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlangenOA.IDAL
{
    public partial interface IDbSession
    {
        //线程内EF唯一实例
        DbContext Db { get; }
        //IUserInfoDal UserInfoDal { get; set; }
        bool SaveChanges();
    }
}
