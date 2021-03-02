using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Stone.HuLuBlog.Domain.Model;
using Com.Stone.HuLuBlog.Domain.Repositories;
using Com.Stone.HuLuBlog.Infrastructure;
using Com.Stone.HuLuBlog.Infrastructure.Extensions;
using SqlSugar;

namespace Com.Stone.HuLuBlog.Application.ServiceImpl
{
    partial class CommentServiceImpl : ApplicationService<Comment>, ICommentService
    {
        public ICommentRepository CommentRepository { get; }
        public IArticleRepository ArticleRepository { get; }

        public CommentServiceImpl(ICommentRepository commentRepository,IArticleRepository articleRepository) : base(commentRepository)
        {
            CommentRepository = commentRepository;
            ArticleRepository = articleRepository;
        }

        /// <summary>
        /// 分页查询文章评论
        /// </summary>
        /// <param name="articleID"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PagedResult<Comment> GetAllCommentByArticleID(string articleID,int pageIndex,int pageSize)
        {
            var commentDict = new Dictionary<string, Comment>();
            int totalCount = 0;
            var x = CommentRepository.SugarClient.Queryable<Comment>()
                .Where(c => string.Equals(c.ArticleID,articleID)&& !c.IsChild)
                .ToList();

            //效率优化 todo   思路：先分页查询父评论  然后再查出父评论关联的子评论
            var commentList = CommentRepository.SugarClient.Queryable<Comment>()
                                .Where(c => c.ArticleID == articleID && !c.IsChild)
                                .OrderBy(c => c.AddDateTime,OrderByType.Desc)
                                .ToPageList(pageIndex, pageSize, ref totalCount);
            var childCommentList = CommentRepository.SugarClient.Queryable<Comment>()
                                .Where(s => s.IsChild && commentList.Select(c => c.ID).Contains(s.PID))
                                .ToList();

            commentList = commentList.Concat(childCommentList).ToList();

            //用字典关联父子关系
            foreach (var comment in commentList)
            {
                if(comment.IsChild) //为子级评论
                {
                    if(commentDict.ContainsKey(comment.PID)) //字典中已有父级评论
                    {
                        if (commentDict[comment.PID].ChildComments == null) commentDict[comment.PID].ChildComments = new List<Comment>();
                        commentDict[comment.PID].ChildComments.Add(comment);
                    }
                    else //如果字典中不包含父级评论
                    {
                        var parent = commentList.Where(c => !c.IsChild && c.ID == comment.PID).FirstOrDefault();
                        if (parent.ChildComments == null) parent.ChildComments = new List<Comment>();
                        parent.ChildComments.Add(comment);
                        commentDict.Add(parent.ID, parent);
                    }
                }
                else //父级评论
                {
                    if(!commentDict.ContainsKey(comment.ID))//如果未包含key
                    {
                        commentDict.Add(comment.ID, comment);
                    }
                    else //已包含key
                    {
                        continue;
                    }
                }
            }

            PagedResult<Comment> result = new PagedResult<Comment>(totalCount,(totalCount + pageSize -1) / pageSize,pageSize,pageIndex, commentDict.Values.OrderByDescending(c => c.AddDateTime).ToList());

            return result;
        }

        /// <summary>
        /// 添加评论，同时增加文章评论数量
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="articleID"></param>
        public void AddCommentWithCommentCount(Comment comment,string articleID)
        {
            try
            {
                CommentRepository.BeginTran();
                Add(comment);
                if(!articleID.IsNullOrEmpty())
                {
                    ArticleRepository.SugarClient.Updateable<Article>()
                        .SetColumns(a => a.CommentCount == a.CommentCount + 1)
                        .Where(a => a.ID == articleID)
                        .ExecuteCommand();
                }
                CommentRepository.CommitTran();
            }
            catch(Exception ex)
            {
                CommentRepository.RollBackTran();
                throw new Exception("事务执行失败", ex);
            }
        }

        /// <summary>
        /// 删除评论，同时减少文章评论数量
        /// </summary>
        /// <param name="commentID"></param>
        /// <param name="articleID"></param>
        public void RemoveCommentWithCommentCount(string commentID,string articleID)
        {
            try
            {
                CommentRepository.BeginTran();
                Remove(commentID);
                if (!articleID.IsNullOrEmpty())
                {
                    ArticleRepository.SugarClient.Updateable<Article>()
                        .SetColumns(a => a.CommentCount == a.CommentCount - 1)
                        .Where(a => a.ID == articleID)
                        .ExecuteCommand();
                }
                CommentRepository.CommitTran();

            }
            catch (Exception ex)
            {
                CommentRepository.RollBackTran();
                throw new Exception("事务执行失败", ex);
            }
        }
    }
}

