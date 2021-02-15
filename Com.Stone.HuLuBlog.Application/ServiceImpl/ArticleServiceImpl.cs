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
        /// 添加文章时，更新tag表冗余字段articCount
        /// </summary>
        /// <param name="article"></param>
        /// <param name="tagID"></param>
        public void AddArticleWithTag(Article article,string tagID)
        {

            try
            {
                //锁此线程
                lock (this)
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
            }
            catch (Exception ex)
            {
                ArticleRepository.RollBackTran();
                throw new Exception("事务执行失败", ex);
            }
            
        }

        /// <summary>
        /// 修改文章删除状态时，更新tag表冗余字段articCount
        /// </summary>
        /// <param name="article"></param>
        /// <param name="tagID"></param>
        /// <param name="subNum"></param>  修改值 1或-1
        public void UpdateArticleWithTag(Article article, string tagID, int subNum)
        {

            try
            {
                //锁此线程
                lock (this)
                {
                    ArticleRepository.BeginTran();

                    ArticleRepository.Update(article);

                    //直接更新articleCount字段  
                    ArticleTagRepository.SugarClient.Updateable<ArticleTag>()
                        .SetColumns(t => t.ArticleCount == t.ArticleCount + subNum)
                        .Where(t => t.ID == tagID)
                        .ExecuteCommand();

                    ArticleRepository.CommitTran();
                }
            }
            catch (Exception ex)
            {
                ArticleRepository.RollBackTran();
                throw new Exception("事务执行失败", ex);
            }

        }
    }
}
