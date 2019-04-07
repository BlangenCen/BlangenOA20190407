using BlangenOA.Common;
using Quartz;

namespace BlangenOA.QuartzNet
{
    /// <summary>
    /// 完成工作任务的定义
    /// </summary>
    public class IndexJob : IJob
    {
        #region 各种尝试
        //private static readonly IApplicationContext ctx = ContextRegistry.GetContext();
        //private static readonly IKeyWordsRankBll keyWordsRankBll = (IKeyWordsRankBll)ctx.GetObject("KeyWordsRankBll");
        //IKeyWordsRankBll keyWordsRankBll = new KeyWordsRankBll(); 
        #endregion
        public void Execute(JobExecutionContext context)
        {
            //Console.WriteLine("雅士乐！" + DateTime.Now.ToString());

            //清除
            DelteteAllKeyWordsRank();
            //插入
            InsertKeyWordsRank();
        }
        /// <summary>
        /// 清除汇总表中的数据
        /// </summary>
        private void DelteteAllKeyWordsRank()
        {
            string sql = "truncate table KeyWordsRank";
            SqlHelper.ExecuteScalar(sql, System.Data.CommandType.Text);

        }
        /// <summary>
        /// 总结明细表插入汇总表
        /// </summary>
        private void InsertKeyWordsRank()
        {
            string sql = "insert into KeyWordsRank select newid(), SearchDetails.KeyWords, count(*) from SearchDetails where DateDiff(day, SearchDetails.SearchDateTime, getdate()) <= 7 group by SearchDetails.KeyWords";
            SqlHelper.ExecuteScalar(sql, System.Data.CommandType.Text);
        }
    }
}