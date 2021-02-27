using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Com.Stone.HuLuBlog.Web.Models
{
    public class EmailVM
    {
        [Required(ErrorMessage = "发件人不能为空")]
        public string From { get; set; }
        [Required(ErrorMessage = "收件人不能为空")]
        public string To { get; set; }
        [Required(ErrorMessage = "主题不能为空")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "内容不能为空")]
        public string Body { get; set; }
    }
}