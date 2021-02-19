using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Stone.HuLuBlog.Domain.Model;
using Com.Stone.HuLuBlog.Infrastructure;

namespace Com.Stone.HuLuBlog.Application
{
    public interface IArticleService:IService<Article>
    {
        void UpdateArticleReadCount(string articleID);
        void AddArticleWithTag(Article article,string tagID);
        void UpdateArticleWithTag(Article article, string oldTagID, string newTagID);
        void DeleteArticleWithTag(Article article, string tagID);
        void UpdateArticleDeleteStatus(Article article);
        void UpdateArticleTagName(string tagID, string newName);
        PagedResult<Article> SearchArticleIndex(string keyword, int pageIndex = 1, int pageSize = 20);
        void UpdateAllArticleIndex();
    }
}
