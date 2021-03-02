using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Com.Stone.HuLuBlog.Web.Models
{
    public class CommentVM
    {
        public string ID { get; set; }

        public string ArticleID { get; set; }

        public string UserID { get; set; }

        [Required(ErrorMessage = "用户名不能为空")]
        public string UserName { get; set; }

        [StringLength(maximumLength: 1000, ErrorMessage = "评论限制1000个字符")]
        [Required(ErrorMessage = "评论内容不能为空")]
        public string CommentContent { get; set; }

        public DateTime AddDateTime { get; set; }

        [Required(ErrorMessage ="邮箱地址不能为空")]
        public string Email { get; set; }

        public string PID { get; set; }

        public bool IsReceive { get; set; }

        public bool IsChild { get; set; }

        public string ReplyTo { get; set; }

        public string ReplyToID { get; set; }

        public List<CommentVM> ChildComments { get; set; }
    }
}