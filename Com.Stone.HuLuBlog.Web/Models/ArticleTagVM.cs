using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Com.Stone.HuLuBlog.Web.Models
{
    public class ArticleTagVM
    {
        [Required]
        public string ID { get; set; }

        [Required(ErrorMessage = "标签名不能为空")]
        [StringLength(maximumLength: 16, MinimumLength = 1, ErrorMessage = "请输入1-16位的标签名称")]
        public string TagName { get; set; }

        public int ArticleCount { get; set; }
    }
}