using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Stone.HuLuBlog.Domain.Model;
using Com.Stone.HuLuBlog.Domain.Repositories;

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
        /// 添加文章时，更新tag表冗余字段articCount
        /// </summary>
        /// <param name="article"></param>
        /// <param name="tagID"></param>
        public void AddArticleWithTag(Article article,string tagID)
        {

            try
            {
                ArticleRepository.BeginTran();

                ArticleRepository.Add(article);

                //直接更新articleCount字段  直接+1
                ArticleTagRepository.SugarClient.Updateable<ArticleTag>()
                    .SetColumns(t => t.ArticleCount == t.ArticleCount + 1)
                    .Where(t => t.ID == tagID)
                    .ExecuteCommand();

                ArticleRepository.CommitTran();
            }
            catch (Exception ex)
            {
                ArticleRepository.RollBackTran();
                throw new Exception("事务执行失败", ex);
            }
            
        }

        /// <summary>
        /// 硬删除文章时，更新tag表冗余字段articCount
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
