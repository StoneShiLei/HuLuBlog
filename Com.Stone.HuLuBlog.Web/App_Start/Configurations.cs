using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Stone.HuLuBlog.Web.App_Start
{
    public static class Configurations
    {
        public const string COOKIE_KEY = "HULUBLOG";
        public const string TOKEN_KEY = "HULUBLOG_TOKEN";
        public const int TOKEN_TIME = 30;  //token 过期时间 单位分钟
        public readonly static string UPLOAD_PATH = HttpContext.Current.Server.MapPath("~/Upload/Images/");

    }
}