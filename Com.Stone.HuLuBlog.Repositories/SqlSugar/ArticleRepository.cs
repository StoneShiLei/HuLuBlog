using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Stone.HuLuBlog.Domain.Model;
using Com.Stone.HuLuBlog.Domain.Repositories;

namespace Com.Stone.HuLuBlog.Repositories.SqlSugar
{
    public class ArticleRepository : SqlSugarRepository<Article>, IArticleRepository
    {
        public ArticleRepository(ISqlSugarRepositoryContext context) : base(typeof(HuLuBlogContext), context)
        {
        }
    }
}
