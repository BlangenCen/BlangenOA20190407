using BlangenOA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlangenOA.IDAL
{
    public partial interface IKeyWordsRankDal : IBaseDal<KeyWordsRank>
    {
        /// <summary>
        /// 删除汇总表所有数据
        /// </summary>
        /// <returns></returns>
        bool DelteteAllKeyWordsRank();
        /// <summary>
        /// 汇总明细表的数据后插入
        /// </summary>
        /// <returns></returns>
        bool InsertKeyWordsRank();
        /// <summary>
        /// 获取用户搜索历史记录
        /// </summary>
        /// <param name="term">用户实时搜索内容</param>
        /// <returns></returns>
        List<string> GetSearchHistory(string term);
    }
}
