using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Com.Stone.HuLuBlog.Web.Models
{
    public class ArticleVM
    {
        public string ID { get; set; }

        public string UserID { get; set; }
        
        public string UserName { get; set; }

        [Required(ErrorMessage = "请选择分类")]
        public string TagID { get; set; }

        [Required(ErrorMessage = "请选择分类")]
        [StringLength(maximumLength: 16, MinimumLength = 1, ErrorMessage = "请输入1-16位的标签名称")]
        public string TagName { get; set; }

        [Required(ErrorMessage = "标题不能为空")]
        [StringLength(maximumLength: 50, MinimumLength = 1, ErrorMessage = "请输入1-50位的文章标题")]
        public string ArticleTitle { get; set; }

        public string ArticleContent { get; set; }

        public string HtmlContent { get; set; }

        public string MarkDownContent { get; set; }

        public DateTime AddDateTime { get; set; }

        public bool IsDelete { get; set; }

        [Required(ErrorMessage = "推荐状态不能为空")]
        public bool IsRecommend { get; set; }

        public DateTime? DeleteDate { get; set; }

        public int ReadCount { get; set; }

        public string ImagePath { get; set; }

        public int CommentCount { get; set; }
    }
}