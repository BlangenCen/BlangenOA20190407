using BlangenOA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlangenOA.IDAL
{
    public partial interface IUserInfoDal : IBaseDal<UserInfo>
    {
        //这个接口放需要实现的除IBaseDal外的其他方法
    }
}
