using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Com.Stone.HuLuBlog.Web.Models
{
    public class FriendLinkVM
    {
        [Required(ErrorMessage = "ID不能为空")]
        public string ID { get; set; }

        public string IconUrl { get; set; }
        [Required(ErrorMessage = "标题不能为空")]
        public string Title { get; set; }
        [Required(ErrorMessage = "域名不能为空")]
        public string Domain { get; set; }
        [Required(ErrorMessage = "Url不能为空")]
        public string Url { get; set; }
    }
}