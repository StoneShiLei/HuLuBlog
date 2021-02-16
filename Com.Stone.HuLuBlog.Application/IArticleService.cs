using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Stone.HuLuBlog.Domain.Model;

namespace Com.Stone.HuLuBlog.Application
{
    public interface IArticleService:IService<Article>
    {
        void UpdateArticleReadCount(string articleID);
        void AddArticleWithTag(Article article,string tagID);
        void UpdateArticleWithTag(Article article, string tagID, int subNum);
    }
}
