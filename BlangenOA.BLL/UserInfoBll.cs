using BlangenOA.DALFactory;
using BlangenOA.IBLL;
using BlangenOA.Model;
using BlangenOA.Model.EnumType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlangenOA.BLL
{
    public partial class UserInfoBll : BaseBll<UserInfo>, IUserInfoBll
    {
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="list">ID集合</param>
        /// <returns></returns>
        public bool DeleteEntities(List<int> list)
        {
            var userInfoList = this.LoadEntities(u => list.Contains(u.ID));
            foreach (UserInfo u in userInfoList)
            {
                //this.DbSession.UserInfoDal.DeleteEntity(u);
                this.CurrentDal.DeleteEntity(u);
            }
            //一次连接,提高性能
            return this.DbSession.SaveChanges();
        }
        /// <summary>
        /// 搜索数据
        /// </summary>
        /// <param name="userInfoSearch">封装的搜索条件数据</param>
        /// <param name="delFlag">删除标记</param>
        /// <returns></returns>
        public IQueryable<UserInfo> LoadSearchEntities(UserInfoSearch userInfoSearch, DeleteEnumType delFlag)
        {
            //拿到所有标记为delFlag的数据（z-index:1）
            var temp = this.LoadEntities(u => u.DelFlag == (short)delFlag);
            //拿到所有名称为UserName的数据（z:2）
            if (!string.IsNullOrEmpty(userInfoSearch.UserName))
            {
                temp = this.LoadEntities(u => u.UName == userInfoSearch.UserName);
            }
            //拿到所有备注为UserRemark的数据（z:3）
            if (!string.IsNullOrEmpty(userInfoSearch.UserRemark))
            {
                temp = this.LoadEntities(u => u.Remark == userInfoSearch.UserRemark);
            }
            userInfoSearch.PageCount = temp.Count();
            return temp.OrderBy(u => u.ID).Skip((userInfoSearch.PageIndex - 1) * userInfoSearch.PageSize).Take(userInfoSearch.PageSize);
        }
        /// <summary>
        /// 设置用户权限
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="actionID"></param>
        /// <param name="isPass"></param>
        /// <returns></returns>
        public bool SetUserAction(int userID, int actionID, bool isPass)
        {
            var userAction = this.DbSession.R_UserInfo_ActionInfoDal.LoadEntities(uA => uA.UserInfoID == userID && uA.ActionInfoID == actionID).FirstOrDefault();
            //判断用户是否已经有此权限,有则只需更改isPass的值,无则插入！
            if (userAction != null)
            {
                userAction.IsPass = isPass;
                this.DbSession.R_UserInfo_ActionInfoDal.EditEntity(userAction);
            }
            else
            {
                userAction = new R_UserInfo_ActionInfo()
                {
                    UserInfoID = userID,
                    ActionInfoID = actionID,
                    IsPass = isPass
                };
                this.DbSession.R_UserInfo_ActionInfoDal.AddEntity(userAction);
            }
            //记得保存
            return this.DbSession.SaveChanges();
        }

        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userRoleIDList"></param>
        /// <returns></returns>
        public bool SetUserRole(int userID, List<int> userRoleIDList)
        {
            var userInfo = this.CurrentDal.LoadEntities(u => u.ID == userID).FirstOrDefault();
            //先清除用户已有角色
            userInfo.RoleInfo.Clear();
            foreach (int id in userRoleIDList)
            {
                //将已选角色找出来 并打上标记
                var roleInfo = this.DbSession.RoleInfoDal.LoadEntities(r => r.ID == id).FirstOrDefault();
                userInfo.RoleInfo.Add(roleInfo);
            }
            return this.DbSession.SaveChanges();
        }

    }
}
