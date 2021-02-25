using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Com.Stone.HuLuBlog.Web.Models
{
    public class UserVM
    {
        public string ID { get; set; }

        //[Required(ErrorMessage = "用户名不能为空")]
        //[StringLength(maximumLength:16,MinimumLength = 2,ErrorMessage ="请输入2-16位的用户名")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "邮箱不能为空")]
        public string Email { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [StringLength(maximumLength: 16, MinimumLength = 8, ErrorMessage = "请输入8-16位的密码")]
        public string Password { get; set; }
    }
}