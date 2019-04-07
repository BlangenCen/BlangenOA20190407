using BlangenOA.IDAL;
using BlangenOA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace BlangenOA.DAL
{
    public partial class KeyWordsRankDal : BaseDal<KeyWordsRank>, IKeyWordsRankDal
    {
        /// <summary>
        /// 删除汇总表所有数据
        /// </summary>
        /// <returns></returns>
        public bool DelteteAllKeyWordsRank()
        {
            string sql = "truncate table KeyWordsRank";
            return this.Db.Database.ExecuteSqlCommand(sql) > 0;
        }
        /// <summary>
        /// 汇总明细表的数据后插入
        /// </summary>
        /// <returns></returns>
        public bool InsertKeyWordsRank()
        {
            string sql = "insert into KeyWordsRank select newid(), SearchDetails.KeyWords, count(*) from SearchDetails where DateDiff(day, SearchDetails.SearchDateTime, getdate()) <= 7 group by SearchDetails.KeyWords";
            return this.Db.Database.ExecuteSqlCommand(sql) > 0;
        }
        /// <summary>
        /// 获取用户搜索历史记录
        /// </summary>
        /// <param name="term">用户实时搜索内容</param>
        /// <returns></returns>
        public List<string> GetSearchHistory(string term)
        {
            string sql = "select KeyWords from KeyWordsRank where KeyWords like @term order by KeyWords";
            return this.Db.Database.SqlQuery<string>(sql, new SqlParameter("@term", System.Data.SqlDbType.NChar) { Value = term + "%" }).ToList();
        }
    }
}
