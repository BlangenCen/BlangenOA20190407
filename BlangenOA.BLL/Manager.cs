 
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
	
	public partial class ActionInfoBll : BaseBll<ActionInfo>,IActionInfoBll
    {
		 public override void SetCurrentDal()
        {
            this.CurrentDal = this.DbSession.ActionInfoDal;
        }
    }   
	
	public partial class BooksBll : BaseBll<Books>,IBooksBll
    {
		 public override void SetCurrentDal()
        {
            this.CurrentDal = this.DbSession.BooksDal;
        }
    }   
	
	public partial class DepartmentBll : BaseBll<Department>,IDepartmentBll
    {
		 public override void SetCurrentDal()
        {
            this.CurrentDal = this.DbSession.DepartmentDal;
        }
    }   
	
	public partial class KeyWordsRankBll : BaseBll<KeyWordsRank>,IKeyWordsRankBll
    {
		 public override void SetCurrentDal()
        {
            this.CurrentDal = this.DbSession.KeyWordsRankDal;
        }
    }   
	
	public partial class R_UserInfo_ActionInfoBll : BaseBll<R_UserInfo_ActionInfo>,IR_UserInfo_ActionInfoBll
    {
		 public override void SetCurrentDal()
        {
            this.CurrentDal = this.DbSession.R_UserInfo_ActionInfoDal;
        }
    }   
	
	public partial class RoleInfoBll : BaseBll<RoleInfo>,IRoleInfoBll
    {
		 public override void SetCurrentDal()
        {
            this.CurrentDal = this.DbSession.RoleInfoDal;
        }
    }   
	
	public partial class SearchDetailsBll : BaseBll<SearchDetails>,ISearchDetailsBll
    {
		 public override void SetCurrentDal()
        {
            this.CurrentDal = this.DbSession.SearchDetailsDal;
        }
    }   
	
	public partial class UserInfoBll : BaseBll<UserInfo>,IUserInfoBll
    {
		 public override void SetCurrentDal()
        {
            this.CurrentDal = this.DbSession.UserInfoDal;
        }
    }   
	
}