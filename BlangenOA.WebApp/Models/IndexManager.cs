using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace BlangenOA.WebApp.Models
{
    public sealed class IndexManager
    {
        //单例模式
        private readonly static IndexManager indexManager = new IndexManager();
        private IndexManager()
        {

        }
        public static IndexManager CreateInstance()
        {
            return indexManager;
        }
        public Queue<ViewModelContent> queue = new Queue<ViewModelContent>();
        /// <summary>
        /// 向索引添加数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="title">标题</param>
        /// <param name="abstractContent">摘要</param>
        public void AddToLucene(int id, string title, string abstractContent)
        {
            queue.Enqueue(new ViewModelContent()
            {
                ID = id,
                Title = title,
                Abstract = abstractContent,
                LuceneEnumType = Model.EnumType.LuceneEnumType.Add
            });
        }
        /// <summary>
        /// 删除索引库中的数据
        /// </summary>
        /// <param name="id"></param>
        public void DeleteLucene(int id)
        {
            queue.Enqueue(new ViewModelContent()
            {
                ID = id,
                LuceneEnumType = Model.EnumType.LuceneEnumType.Delete
            });
        }
        /// <summary>
        /// 开始处理队列
        /// </summary>
        public void StartThread()
        {
            ThreadPool.QueueUserWorkItem(x =>
            {
                while (true)
                {
                    if (queue.Count() > 0)
                    {
                        HandleIndex();
                    }
                    else
                    {
                        Thread.Sleep(5000);
                    }
                }
            });
        }
        /// <summary>
        /// 处理索引库
        /// </summary>
        private void HandleIndex()
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
            //读取队列中的操作
            while (queue.Count() > 0)
            {
                ViewModelContent viewModelContent = queue.Dequeue();
                //执行了删除操作,不存在此索引也不报错
                writer.DeleteDocuments(new Term("ID", viewModelContent.ID.ToString()));
                if (viewModelContent.LuceneEnumType == Model.EnumType.LuceneEnumType.Delete)
                {
                    continue;
                }
                //表示一篇文档。
                Document document = new Document();
                //Field.Store.YES:表示是否存储原值。只有当Field.Store.YES在后面才能用doc.Get("number")取出值来.Field.Index. NOT_ANALYZED:不进行分词保存
                document.Add(new Field("ID", viewModelContent.ID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                //Field.Index. ANALYZED:进行分词保存:也就是要进行全文的字段要设置分词 保存（因为要进行模糊查询）
                //Lucene.Net.Documents.Field.TermVector.WITH_POSITIONS_OFFSETS:不仅保存分词还保存分词的距离。
                document.Add(new Field("Title", viewModelContent.Title, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                document.Add(new Field("Abstract", viewModelContent.Abstract, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                writer.AddDocument(document);
            }
            writer.Close();//会自动解锁。
            directory.Close();//不要忘了Close，否则索引结果搜不到
        }
    }
}