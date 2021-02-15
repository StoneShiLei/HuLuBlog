using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Stone.HuLuBlog.Domain.Model;
using Com.Stone.HuLuBlog.Domain.Repositories;

namespace Com.Stone.HuLuBlog.Application.ServiceImpl
{
    partial class ArticleTagServiceImpl : ApplicationService<ArticleTag>, IArticleTagService
    {
        public IArticleTagRepository ArticleTagRepository { get; }

        public ArticleTagServiceImpl(IArticleTagRepository articleTagRepository) : base(articleTagRepository)
        {
            ArticleTagRepository = articleTagRepository;
        }
    }
}
