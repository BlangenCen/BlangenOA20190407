using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlangenOA.Model
{
    /// <summary>
    /// 构建用户搜索的条件
    /// </summary>
    public class UserInfoSearch : BaesSearch
    {
        public string UserName { get; set; }
        public string UserRemark { get; set; }
    }
}
