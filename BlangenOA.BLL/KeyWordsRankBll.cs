using BlangenOA.DAL;
using BlangenOA.IBLL;
using BlangenOA.Model;
using System.Collections.Generic;

namespace BlangenOA.BLL
{
    public partial class KeyWordsRankBll : BaseBll<KeyWordsRank>, IKeyWordsRankBll
    {
        KeyWordsRankDal keyWordsRankDal = new KeyWordsRankDal();
        /// <summary>
        /// 删除汇总表所有数据
        /// </summary>
        /// <returns></returns>
        public bool DelteteAllKeyWordsRank()
        {
            return keyWordsRankDal.DelteteAllKeyWordsRank();
        }
        /// <summary>
        /// 汇总明细表的数据后插入
        /// </summary>
        /// <returns></returns>
        public bool InsertKeyWordsRank()
        {
            return keyWordsRankDal.InsertKeyWordsRank();
        }
        /// <summary>
        /// 获取用户搜索历史记录
        /// </summary>
        /// <param name="term">用户实时搜索内容</param>
        /// <returns></returns>
        public List<string> GetSearchHistory(string term)
        {
            return keyWordsRankDal.GetSearchHistory(term);
        }
    }
}
