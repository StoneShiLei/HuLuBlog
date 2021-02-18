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
        public readonly static string IndexDic = HttpContext.Current.Server.MapPath("~/IndexDic/");
        public readonly static string Stopwords = HttpContext.Current.Server.MapPath("~/Resources/");
        public readonly static string ConnStrHuLuBlog = ConfigurationManager.ConnectionStrings["HuLuBlog"].ConnectionString;
        public const string SecretKey = "84022499840224998402249984022499"; //RijndaelManaged 加密必须为32位                                                                             

    }
}
