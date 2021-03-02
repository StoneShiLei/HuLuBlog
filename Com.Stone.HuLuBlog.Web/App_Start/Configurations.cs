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
        public const string COMMENT_KEY = "HULUBLOG_COMMENT";
        public const int TOKEN_TIME = 30;  //token 过期时间 单位分钟
        public const string UPLOAD_PATH = "~/Upload/Images/";
        public const string MQ_CONNSTR = "host=localhost;PublisherConfirms=false;persistentMessages=true;prefetchcount=50;username=guest;password=guest";
        public const string HuLuEmail = "569812422@qq.com";
        public const string BlogEmail = "hulu@hafuhafu.cn";
    }
}