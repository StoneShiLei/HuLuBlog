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

        [Required]
        public string ArticleID { get; set; }

        [Required]
        public string UserID { get; set; }

        public string UserName { get; set; }

        [StringLength(maximumLength: 300, ErrorMessage = "评论限制300个字符")]
        public string CommentContent { get; set; }

        public string AddDateTime { get; set; }

        //[remote]
        //public string Captcha { get; set; }
    }
}