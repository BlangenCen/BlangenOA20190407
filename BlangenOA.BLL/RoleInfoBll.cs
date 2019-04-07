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
    public partial class RoleInfoBll : BaseBll<RoleInfo>, IRoleInfoBll
    {
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="list">ID集合</param>
        /// <returns></returns>
        public bool DeleteEntities(List<int> list)
        {
            var roleInfoList = this.LoadEntities(u => list.Contains(u.ID));
            foreach (RoleInfo r in roleInfoList)
            {
                //this.DbSession.UserInfoDal.DeleteEntity(u);
                this.CurrentDal.DeleteEntity(r);
            }
            //一次连接,提高性能
            return this.DbSession.SaveChanges();
        }
        /// <summary>
        /// 设置角色权限
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="roleActionIDList"></param>
        /// <returns></returns>
        public bool SetRoleAction(int roleID, List<int> roleActionIDList)
        {
            var roleInfo = this.CurrentDal.LoadEntities(r => r.ID == roleID).FirstOrDefault();
            //先清除用户已有权限
            roleInfo.ActionInfo.Clear();
            foreach (int id in roleActionIDList)
            {
                //将已选权限找出来 并打上标记
                var actionInfo = this.DbSession.ActionInfoDal.LoadEntities(a => a.ID == id).FirstOrDefault();
                roleInfo.ActionInfo.Add(actionInfo);
            }
            return this.DbSession.SaveChanges();
        }
    }
}
