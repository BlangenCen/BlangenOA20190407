using BlangenOA.Model;
using BlangenOA.Model.EnumType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlangenOA.IBLL
{
    public partial interface IUserInfoBll : IBaseBll<UserInfo>
    {
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <returns></returns>
        bool DeleteEntities(List<int> list);
        /// <summary>
        /// 搜索数据
        /// </summary>
        /// <param name="userInfoSearch">封装的搜索条件数据</param>
        /// <param name="delFlag">删除标记</param>
        /// <returns></returns>
        IQueryable<UserInfo> LoadSearchEntities(UserInfoSearch userInfoSearch, DeleteEnumType delFlag);
        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="userRoleIDList">角色ID集合</param>
        /// <returns></returns>
        bool SetUserRole(int userID, List<int> userRoleIDList);
        /// <summary>
        /// 设置用户权限
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="actionID">权限ID</param>
        /// <param name="isPass">允许/禁止</param>
        /// <returns></returns>
        bool SetUserAction(int userID, int actionID, bool isPass);
    }
}
