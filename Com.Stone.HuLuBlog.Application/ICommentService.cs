using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Stone.HuLuBlog.Domain.Model;
using Com.Stone.HuLuBlog.Infrastructure;

namespace Com.Stone.HuLuBlog.Application
{
    public interface ICommentService : IService<Comment>
    {
        PagedResult<Comment> GetAllCommentByArticleID(string articleID, int pageIndex, int pageSize);
        void AddCommentWithCommentCount(Comment comment, string articleID);
        void RemoveCommentWithCommentCount(string commentID, string articleID);
    }
}
