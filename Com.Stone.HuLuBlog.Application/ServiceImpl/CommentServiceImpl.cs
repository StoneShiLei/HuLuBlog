using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Stone.HuLuBlog.Domain.Model;
using Com.Stone.HuLuBlog.Domain.Repositories;

namespace Com.Stone.HuLuBlog.Application.ServiceImpl
{
    partial class CommentServiceImpl : ApplicationService<Comment>, ICommentService
    {
        public ICommentRepository CommentRepository { get; }

        public CommentServiceImpl(ICommentRepository commentRepository) : base(commentRepository)
        {
            CommentRepository = commentRepository;
        }
    }
}

