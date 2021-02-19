using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Stone.HuLuBlog.Domain.Model;
using Com.Stone.HuLuBlog.Domain.Repositories;
using Com.Stone.HuLuBlog.Infrastructure;

namespace Com.Stone.HuLuBlog.Application.ServiceImpl
{
    partial class ArticleServiceImpl : ApplicationService<Article>, IArticleService
    {
        public IArticleRepository ArticleRepository { get; }
        IArticleTagRepository ArticleTagRepository { get; }

        public ArticleServiceImpl(IArticleRepository articleRepository,
            IArticleTagRepository articleTagRepository):base(articleRepository)
        {
            ArticleRepository = articleRepository;
            ArticleTagRepository = articleTagRepository;
        }

        /// <summary>
        /// 根据关键字搜索文章 使用全文索引 分页
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public PagedResult<Article> SearchArticleIndex(string keyword,int pageIndex =1,int pageSize = 20)
        {
            var totalCount = 0;
            var idList = new List<string>();
            var models = LuceneOperation.Search(keyword);
            models.ForEach(model => idList.Add(model.ID));

            var pagedList = ArticleRepository.SugarClient.Queryable<Article>()
                                .Where(a => a.IsDelete == false && idList.Contains(a.ID))
                                .ToPageList(pageIndex, pageSize, ref totalCount);

            pagedList.ForEach(a =>
            {
                var model = models.FirstOrDefault(m => m.ID == a.ID);
                a.ArticleTitle = model?.Title;
                a.ArticleContent = model?.Content;
            });

            PagedResult<Article> results = new PagedResult<Article>(totalCount,
                (totalCount + pageSize - 1) / pageSize, pageSize, pageIndex, pagedList);

            return results;
        }

        /// <summary>
        /// 更新全部文章的索引至最新分词状态
        /// </summary>
        public void UpdateAllArticleIndex()
        {
            var allArticle = ArticleRepository.GetAllList();
            List<DocumentModel> modes = new List<DocumentModel>();
            foreach(var article in allArticle)
            {
                DocumentModel model = new DocumentModel()
                {
                    ID = article.ID,
                    Title = article.ArticleTitle,
                    Content = article.ArticleContent
                };

                modes.Add(model);
            }
            LuceneOperation.DeleteAllIndex();
            LuceneOperation.AddAllIndex(modes);
        }

        /// <summary>
        /// 更新文章阅读量
        /// </summary>
        /// <param name="articleID"></param>
        public void UpdateArticleReadCount(string articleID)
        {
            ArticleRepository.SugarClient.Updateable<Article>()
                .SetColumns(a => a.ReadCount == a.ReadCount + 1)
                .Where(a => a.ID == articleID)
                .ExecuteCommand();
        }

        /// <summary>
        /// 添加文章时，更新tag表冗余字段articCount，添加文章索引
        /// </summary>
        /// <param name="article"></param>
        /// <param name="tagID"></param>
        public void AddArticleWithTag(Article article,string tagID)
        {

            try
            {
                ArticleRepository.BeginTran();

                var dbArticle = ArticleRepository.Add(article);

                //直接更新articleCount字段  直接+1
                ArticleTagRepository.SugarClient.Updateable<ArticleTag>()
                    .SetColumns(t => t.ArticleCount == t.ArticleCount + 1)
                    .Where(t => t.ID == tagID)
                    .ExecuteCommand();

                //添加文章全文索引
                DocumentModel model = new DocumentModel()
                {
                    ID = dbArticle.ID,
                    Title = dbArticle.ArticleTitle,
                    Content = dbArticle.ArticleContent

                };
                LuceneOperation.AddIndex(model);

                ArticleRepository.CommitTran();
            }
            catch (Exception ex)
            {
                ArticleRepository.RollBackTran();
                throw new Exception("事务执行失败", ex);
            }
            
        }

        /// <summary>
        /// 修改文章时，更新tag表冗余字段articCount，更新文章索引
        /// </summary>
        /// <param name="article"></param>
        /// <param name="tagID"></param>
        public void UpdateArticleWithTag(Article article, string oldTagID,string newTagID)
        {

            try
            {
                ArticleRepository.BeginTran();

                ArticleRepository.Update(article);

                //旧标签的文章数目-1
                ArticleTagRepository.SugarClient.Updateable<ArticleTag>()
                    .SetColumns(t => t.ArticleCount == t.ArticleCount -1)
                    .Where(t => t.ID == oldTagID)
                    .ExecuteCommand();

                //新标签的文章数目+1
                ArticleTagRepository.SugarClient.Updateable<ArticleTag>()
                    .SetColumns(t => t.ArticleCount == t.ArticleCount + 1)
                    .Where(t => t.ID == newTagID)
                    .ExecuteCommand();

                //更新文章全文索引
                DocumentModel model = new DocumentModel()
                {
                    ID = article.ID,
                    Title = article.ArticleTitle,
                    Content = article.ArticleContent
                };
                LuceneOperation.UpdateIndex(model);

                ArticleRepository.CommitTran();
            }
            catch (Exception ex)
            {
                ArticleRepository.RollBackTran();
                throw new Exception("事务执行失败", ex);
            }

        }

        /// <summary>
        /// 硬删除文章时，更新tag表冗余字段articCount，删除文章索引
        /// </summary>
        /// <param name="article"></param>
        /// <param name="tagID"></param>
        public void DeleteArticleWithTag(Article article, string tagID)
        {

            try
            { 
                ArticleRepository.BeginTran();

                ArticleRepository.Remove(article.ID);

                //直接更新articleCount字段  
                ArticleTagRepository.SugarClient.Updateable<ArticleTag>()
                    .SetColumns(t => t.ArticleCount == t.ArticleCount - 1)
                    .Where(t => t.ID == tagID)
                    .ExecuteCommand();

                //删除文章全文索引
                DocumentModel model = new DocumentModel()
                {
                    ID = article.ID,
                };
                LuceneOperation.DeleteIndex(model);

                ArticleRepository.CommitTran();
            }
            catch (Exception ex)
            {
                ArticleRepository.RollBackTran();
                throw new Exception("事务执行失败", ex);
            }

        }

        /// <summary>
        /// 修改文章软删除状态
        /// </summary>
        /// <param name="article"></param>
        public void UpdateArticleDeleteStatus(Article article)
        {
            ArticleRepository.SugarClient.Updateable<Article>()
                .SetColumns(a => a.IsDelete == article.IsDelete)
                .SetColumns(a => a.DeleteDate == article.DeleteDate)
                .Where(a => a.ID == article.ID)
                .ExecuteCommand();
        }

        /// <summary>
        /// 修改标签名和文章表的冗余tagname
        /// </summary>
        /// <param name="tagID"></param>
        /// <param name="newName"></param>
        public void UpdateArticleTagName(string tagID,string newName)
        {
            try 
            {
                ArticleRepository.BeginTran();

                ArticleTagRepository.SugarClient.Updateable<ArticleTag>()
                    .SetColumns(t => t.TagName == newName)
                    .Where(t => t.ID == tagID)
                    .ExecuteCommand();
                ArticleRepository.SugarClient.Updateable<Article>()
                    .SetColumns(a => a.TagName == newName)
                    .Where(a => a.TagID == tagID)
                    .ExecuteCommand();

                ArticleRepository.CommitTran();
            }
            catch(Exception ex)
            {
                ArticleRepository.RollBackTran();
                throw new Exception("事务执行失败", ex);
            }
        }

    }
}
