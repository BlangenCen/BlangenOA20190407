using BlangenOA.Model;
using BlangenOA.Model.EnumType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlangenOA.IBLL
{
    public partial interface IRoleInfoBll : IBaseBll<RoleInfo>
    {
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <returns></returns>
        bool DeleteEntities(List<int> list);
        /// <summary>
        /// 设置角色权限
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <param name="roleActionIDList">权限集合</param>
        /// <returns></returns>
        bool SetRoleAction(int roleID, List<int> roleActionIDList);
    }
}
