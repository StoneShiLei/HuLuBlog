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
        public readonly static string UserDict = HttpContext.Current.Server.MapPath("~/Resources/userdict.txt");
        public readonly static string ConnStrHuLuBlog = ConfigurationManager.ConnectionStrings["HuLuBlog"].ConnectionString;
        public const string PASSWORD_SECRET_KEY = "84022499840224998402249984022499"; //RijndaelManaged 加密必须为32位
        public const string SITE_KEY = "6LeeVGYaAAAAAKoheZ1xlftZ_z-y0KD7cc1lfJmq";
        public const string SECRET_KEY = "6LeeVGYaAAAAACKZh_KTpBtlCxj1LaOA1wac3gd8";
        public const string RECAPTCHA_URL = "https://recaptcha.net/recaptcha/api/siteverify"; 
    }
}
