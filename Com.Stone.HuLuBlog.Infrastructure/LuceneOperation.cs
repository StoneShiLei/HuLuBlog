using Lucene.Net.Index;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Lucene.Net.Store;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using Com.Stone.HuLuBlog.Infrastructure.Jieba;
using Lucene.Net.Search.VectorHighlight;
using Com.Stone.HuLuBlog.Infrastructure.Extensions;

namespace Com.Stone.HuLuBlog.Infrastructure
{
    public static class LuceneOperation
    {
        #region search

        /// <summary>
        /// 返回查询结果集
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static List<DocumentModel> Search(string query)
        {
            query = query.ToLower().Replace(" ","");

            var searcher = LuceneHelper.GetSearcherSingle();
            //var keyWordQuery = new BooleanQuery();
            //var hlQuery = new BooleanQuery();
            //foreach (var item in GetSearchKeyWords(query))
            //{
            //    var key = "*" + item + "*";
            //    keyWordQuery.Add(new WildcardQuery(new Term("Title", key)), Occur.SHOULD);
            //    keyWordQuery.Add(new WildcardQuery(new Term("Content", key)), Occur.SHOULD);
            //    hlQuery.Add(new WildcardQuery(new Term("Title", key)),Occur.SHOULD);
            //    hlQuery.Add(new WildcardQuery(new Term("Content", key)),Occur.SHOULD);
            //}

            var titleQuery = new PhraseQuery();
            var contentQuery = new PhraseQuery();

            var keywordList = GetSearchKeyWords(query);
            foreach (var itme in keywordList)
            {
                titleQuery.Add(new Term("Title", itme));
            }

            foreach (var itme in keywordList)
            {
                contentQuery.Add(new Term("Content", itme));
            }

            titleQuery.Slop = 4;
            contentQuery.Slop = 10;

            var bq = new BooleanQuery
            {
                { titleQuery, Occur.SHOULD },
                {contentQuery,Occur.SHOULD }
            };

            var hits = searcher.Search(bq, 500).ScoreDocs;
            var results = new List<DocumentModel>();
            foreach(var hit in hits)
            {
                var highlighter = GetHighlighter();
                FieldQuery tq = highlighter.GetFieldQuery(titleQuery);
                FieldQuery cq = highlighter.GetFieldQuery(contentQuery);

                string titleHL = highlighter.GetBestFragment(tq, searcher.IndexReader, hit.Doc, "Title", 200);
                string contentHL = highlighter.GetBestFragment(cq, searcher.IndexReader, hit.Doc, "Content", 200);

                var model = new DocumentModel
                {
                    ID = searcher.Doc(hit.Doc).Get("ID"),
                    Title = titleHL.IsNullOrEmpty() ? searcher.Doc(hit.Doc).Get("Title") : titleHL,
                    Content = contentHL.IsNullOrEmpty() ? searcher.Doc(hit.Doc).Get("Content") : contentHL
                };
                results.Add(model);
            }

            return results;
        }

        private static FastVectorHighlighter GetHighlighter()
        {
            SimpleFragListBuilder fragListBuilder = new SimpleFragListBuilder();
            ScoreOrderFragmentsBuilder fragmentsBuilder = new ScoreOrderFragmentsBuilder(
                    BaseFragmentsBuilder.COLORED_PRE_TAGS,
                    BaseFragmentsBuilder.COLORED_POST_TAGS);
            return new FastVectorHighlighter(true, false, fragListBuilder,
                    fragmentsBuilder);
        }

        /// <summary>
        /// 用于测试分词结果
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Test(string text)
        {
            text = text.ToLower();
            List<string> keywords = new List<string>();

            var az = new JiebaAnalyzer(JiebaNet.Segmenter.TokenizerMode.Search);
            using (var ts = az.GetTokenStream(null,text))
            {
                ts.Reset();
                var ct = ts.GetAttribute<Lucene.Net.Analysis.TokenAttributes.ICharTermAttribute>();
                while (ts.IncrementToken())
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < ct.Length; i++)
                    {
                        sb.Append(ct.Buffer[i]);
                    }
                    string item = sb.ToString();
                    if (!keywords.Contains(item))
                    {
                        keywords.Add(item);
                    }
                }
            }

            StringBuilder sbr = new StringBuilder();
            foreach(var item in keywords)
            {
                sbr.AppendFormat(" {0} ", item);
            }
            return sbr.ToString().Trim();
            
        }

        /// <summary>
        /// 拆分查询关键词
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private static List<string> GetSearchKeyWords(string query)
        {
            List<string> keywords = new List<string>();
            var analyzer = new JiebaAnalyzer(JiebaNet.Segmenter.TokenizerMode.Default);
            using (var ts = analyzer.GetTokenStream(null, query))
            {
                ts.Reset();
                var ct = ts.GetAttribute<Lucene.Net.Analysis.TokenAttributes.ICharTermAttribute>();
                while (ts.IncrementToken())
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < ct.Length; i++)
                    {
                        sb.Append(ct.Buffer[i]);
                    }
                    string item = sb.ToString();
                    if (!keywords.Contains(item))
                    {
                        keywords.Add(item);
                    }
                }
            }

            return keywords;
        }

        #endregion

        #region index

        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="doc"></param>
        public static void AddIndex(DocumentModel documentModel)
        {
            var doc = ConvertToDocument(documentModel);

            LuceneHelper.GetIndexWriterSingle().AddDocument(doc);
            LuceneHelper.GetIndexWriterSingle().Commit();
        }

        /// <summary>
        /// 更新索引
        /// </summary>
        /// <param name="doc"></param>
        public static void UpdateIndex(DocumentModel documentModel)
        {
            var doc = ConvertToDocument(documentModel);

            LuceneHelper.GetIndexWriterSingle().UpdateDocument(new Term("ID", documentModel.ID), doc);
            LuceneHelper.GetIndexWriterSingle().Commit();
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="documentModel"></param>
        public static void DeleteIndex(DocumentModel documentModel)
        {
            LuceneHelper.GetIndexWriterSingle().DeleteDocuments(new Term("ID", documentModel.ID));
            LuceneHelper.GetIndexWriterSingle().Commit();
        }

        /// <summary>
        /// 删除全部索引
        /// </summary>
        public static void DeleteAllIndex()
        {
            LuceneHelper.GetIndexWriterSingle().DeleteAll();
            LuceneHelper.GetIndexWriterSingle().Commit();
        }

        /// <summary>
        /// 生成全部索引
        /// </summary>
        /// <param name="documentModels"></param>
        public static void AddAllIndex(List<DocumentModel> documentModels)
        {
            
            LuceneHelper.GetIndexWriterSingle().AddDocuments(ConvertToDocuments(documentModels));
            LuceneHelper.GetIndexWriterSingle().Commit();
        }

        #endregion

        #region mappper

        private static List<Document> ConvertToDocuments(IEnumerable<DocumentModel> models)
        {
            return models.Select(m => ConvertToDocument(m)).ToList();
        }

        private static List<DocumentModel> ConvertToModelList(IEnumerable<Document> documents)
        {
            return documents.Select(ConvertToModel).ToList();
        }

        /// <summary>
        /// 转化查询结果集
        /// </summary>
        /// <param name="hits"></param>
        /// <param name="searcher"></param>
        /// <returns></returns>
        private static List<DocumentModel> ConvertToModelList(IEnumerable<ScoreDoc> hits, IndexSearcher searcher)
        {
            return hits.Select(hit => ConvertToModel(searcher.Doc(hit.Doc))).ToList();
        }

        /// <summary>
        /// 将documentModel转化成索引doc
        /// </summary>
        /// <param name="documentModel"></param>
        /// <returns></returns>
        private static Document ConvertToDocument(DocumentModel documentModel)
        {
            //保存至索引文件，按照分词结果保存，且保存词与词之间的距离
            FieldType type = new FieldType
            {
                StoreTermVectorOffsets = true, //存储偏移量
                StoreTermVectorPositions = true, //存储位置
                StoreTermVectors = true, //存储向量
                IsIndexed =true,
                IsTokenized =true,
                IsStored = true
            };

            var id = new StringField("ID", documentModel.ID, Field.Store.YES);
            var title = new Field("Title", documentModel.Title.ToLower(), type);
            var content = new Field("Content", documentModel.Content.ToLower(), type);

            title.Boost = 2f; //增加标题索引权重

            Document doc = new Document
            {
                id,title,content
            };

            return doc;
        }

        /// <summary>
        /// 将索引doc转化为model
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        private static DocumentModel ConvertToModel(Document document)
        {
            return new DocumentModel()
            {
                ID = document.Get("ID"),
                Title = document.Get("Title"),
                Content = document.Get("Content")
            };
        }

        #endregion

        
    }

    //定义索引传输类型
    public class DocumentModel 
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }

    internal static class LuceneHelper
    {
        private static readonly object _writerLock = new object();
        private static readonly object _readerLock = new object();
        private static readonly object _searcherLock = new object();
        private static IndexWriter _writer;
        private static DirectoryReader _reader;
        private static IndexSearcher _searcher;

        /// <summary>
        /// 获取单例writer
        /// </summary>
        /// <returns></returns>
        public static IndexWriter GetIndexWriterSingle()
        {
            lock (_writerLock)
            {
                if (_writer == null)
                {
                    try
                    {
                        IndexWriterConfig config = new IndexWriterConfig(Lucene.Net.Util.LuceneVersion.LUCENE_48,new JiebaAnalyzer(JiebaNet.Segmenter.TokenizerMode.Search));
                        config.OpenMode = OpenMode.CREATE_OR_APPEND;
                        var directory = FSDirectory.Open(new DirectoryInfo(Configurations.IndexDic), new NativeFSLockFactory());

                        //如果文件夹死锁则强行释放锁
                        if (IndexWriter.IsLocked(directory)) IndexWriter.Unlock(directory);
                        var lockFilePath = Path.Combine(Configurations.IndexDic, "write.lock");
                        if (File.Exists(lockFilePath)) File.Delete(lockFilePath);

                        _writer = new IndexWriter(directory, config);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("创建单例writer失败", ex);
                    }
                }
                return _writer;
            }
        }

        /// <summary>
        /// 获取单例reader
        /// </summary>
        /// <returns></returns>
        public static DirectoryReader GetIndexReaderSingle()
        {
            lock(_readerLock)
            {
                try
                {
                    if (_reader == null)
                    {
                        var directory = FSDirectory.Open(new DirectoryInfo(Configurations.IndexDic),NoLockFactory.GetNoLockFactory());
                        _reader = DirectoryReader.Open(directory);
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception("创建单例reader失败", ex);
                }
                return _reader;
            }
        }

        /// <summary>
        /// 获取单例searcher
        /// </summary>
        /// <returns></returns>
        public static IndexSearcher GetSearcherSingle()
        {
            lock(_searcherLock)
            {
                try
                {
                    if(_searcher == null)
                    {
                        var changedReader = DirectoryReader.OpenIfChanged(GetIndexReaderSingle());
                        if (changedReader != null)
                        {
                            GetIndexReaderSingle().Dispose();
                            _reader = changedReader;
                        }

                        _searcher = new IndexSearcher(GetIndexReaderSingle());
                    }

                    return _searcher;
                }
                catch (Exception ex)
                {
                    throw new Exception("获取Searcher失败",ex);
                }
            }
        }
    }
}
