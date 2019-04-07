using BlangenOA.IBLL;
using BlangenOA.Model;
using BlangenOA.WebApp.Models;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlangenOA.WebApp.Controllers
{
    public class SearchController : Controller
    {
        IBooksBll BooksBll { get; set; }
        ISearchDetailsBll SearchDetailsBll { get; set; }
        IKeyWordsRankBll KeyWordsRankBll { get; set; }
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 表单中有多个Submit，单击每个Submit都会提交表单，
        /// 但是只会将用户所单击的表单元素的value值提交到服务端。
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchAbstract()
        {
            if (!string.IsNullOrEmpty(Request["searchAbstract"]))
            {
                //搜索框有内容
                if (!string.IsNullOrEmpty(Request["txtAbstract"]))
                {
                    List<ViewModelContent> list = GetSearchResult(Request["txtAbstract"]);
                    ViewBag.searchResult = list;
                }
                return View("Index");
            }
            else
            {
                //需禁用！
                CreateIndex();
                return Content("创建索引成功");
            }
        }
        /// <summary>
        /// 动态显示用户搜索历史
        /// </summary>
        /// <returns></returns>
        public  ActionResult AutoComplete()
        {
            //客户端会一直发送  ?term=xxx
            string term = Request["term"];
            List<string> list = KeyWordsRankBll.GetSearchHistory(term);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取搜索结果
        /// </summary>
        /// <param name="msg">搜索内容</param>
        /// <returns>搜索结果</returns>
        private List<ViewModelContent> GetSearchResult(string msg)
        {
            List<ViewModelContent> viewModelList = new List<ViewModelContent>();
            string indexPath = @"D:\stu\DotNet\练习\BlangenOA\lucenedir";
            //对用户输入的搜索条件进行拆分。
            List<string> keywordList = Common.WebCommon.PanguSplitWords(msg);
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NoLockFactory());
            IndexReader reader = IndexReader.Open(directory, true);
            IndexSearcher searcher = new IndexSearcher(reader);
            //添加搜索条件
            PhraseQuery query = new PhraseQuery();
            foreach (string keyword in keywordList)
            {
                //Abstract中含有keyword的文章
                query.Add(new Term("Abstract", keyword));
            }
            //多个查询条件的词之间的最大距离.在文章中相隔太远 也就无意义.（例如 “大学生”这个查询条件和"简历"这个查询条件之间如果间隔的词太多也就没有意义了。）
            query.SetSlop(100);
            //TopScoreDocCollector是盛放查询结果的容器
            TopScoreDocCollector collector = TopScoreDocCollector.create(1000, true);
            //根据query查询条件进行查询，查询结果放入collector容器
            searcher.Search(query, null, collector);
            //得到所有查询结果中的文档,GetTotalHits():表示总条数   TopDocs(300, 20);//表示得到300（从300开始），到320（结束）的文档内容.//可以用来实现分页功能
            ScoreDoc[] docs = collector.TopDocs(0, collector.GetTotalHits()).scoreDocs;
            for (int i = 0; i < docs.Length; i++)
            {
                //
                //搜索ScoreDoc[]只能获得文档的id,这样不会把查询结果的Document一次性加载到内存中。降低了内存压力，需要获得文档的详细内容的时候通过searcher.Doc来根据文档id来获得文档的详细内容对象Document.
                int docId = docs[i].doc;//得到查询结果文档的id（Lucene内部分配的id）
                Document doc = searcher.Doc(docId);//找到文档id对应的文档详细信息
                viewModelList.Add(new ViewModelContent()
                {
                    // 取出放进字段的值
                    ID = Convert.ToInt32(doc.Get("ID")),
                    Title = doc.Get("Title"),
                    //高亮显示
                    Abstract = Common.WebCommon.CreateHightLight(msg, doc.Get("Abstract"))
                });
            }
            //用户将搜索内容存储到明细表中
            SearchDetailsBll.AddEntity(new SearchDetails()
            {
                Id = Guid.NewGuid(),
                KeyWords = msg,
                SearchDateTime = DateTime.Now
            });
            return viewModelList;
        }

        /// <summary>
        /// 创建索引
        /// </summary>
        private void CreateIndex()
        {
            //目录最好放在配置文件中,注意和磁盘上文件夹的大小写一致，否则会报错。将创建的分词内容放在该目录下。
            string indexPath = @"D:\stu\DotNet\练习\BlangenOA\lucenedir";
            //指定索引文件(打开索引目录,并加锁) FS指的是就是FileSystem
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory());
            //IndexReader:对索引进行读取的类。该语句的作用：判断索引库文件夹是否存在以及索引特征文件是否存在。
            bool isUpdate = IndexReader.IndexExists(directory);
            if (isUpdate)
            {
                //同时只能有一段代码对索引库进行写操作。当使用IndexWriter打开directory时会自动对索引库文件上锁。
                //如果索引目录被锁定（比如索引过程中程序异常退出），则首先解锁（提示一下：如果我现在正在写着已经加锁了，但是还没有写完，这时候又来一个请求，那么不就解锁了吗？这个问题后面会解决）
                if (IndexWriter.IsLocked(directory))
                {
                    IndexWriter.Unlock(directory);
                }
            }
            //向索引库中写索引。这时在这里加锁。
            IndexWriter writer = new IndexWriter(directory, new PanGuAnalyzer(), !isUpdate, IndexWriter.MaxFieldLength.UNLIMITED);
            //读取数据库中的书籍
            List<Books> list = BooksBll.LoadEntities(b => true).ToList();
            foreach (Books book in list)
            {
                //表示一篇文档。
                Document document = new Document();
                //Field.Store.YES:表示是否存储原值。只有当Field.Store.YES在后面才能用doc.Get("number")取出值来.Field.Index. NOT_ANALYZED:不进行分词保存
                document.Add(new Field("ID", book.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                //Field.Index. ANALYZED:进行分词保存:也就是要进行全文的字段要设置分词 保存（因为要进行模糊查询）
                //Lucene.Net.Documents.Field.TermVector.WITH_POSITIONS_OFFSETS:不仅保存分词还保存分词的距离。
                document.Add(new Field("Title", book.Title, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                document.Add(new Field("Abstract", book.ContentDescription, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                writer.AddDocument(document);
            }
            writer.Close();//会自动解锁。
            directory.Close();//不要忘了Close，否则索引结果搜不到
        }
    }
}