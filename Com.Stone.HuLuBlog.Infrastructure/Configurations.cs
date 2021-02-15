using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Com.Stone.HuLuBlog.Infrastructure
{
    public static class Configurations
    {
        public readonly static string LogFolder = HttpContext.Current.Server.MapPath("~/Log/");
        public readonly static string ConnStrHuLuBlog = ConfigurationManager.ConnectionStrings["HuLuBlog"].ConnectionString;
        public readonly static string SecretKey = "84022499840224998402249984022499"; //RijndaelManaged 加密必须为32位                                                                             
        public readonly static int TOKEN_TIME = 30;  //token 过期时间 单位分钟


    }
}
