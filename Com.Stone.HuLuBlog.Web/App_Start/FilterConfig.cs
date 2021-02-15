using Com.Stone.HuLuBlog.Web.App_Start;
using System.Web;
using System.Web.Mvc;

namespace Com.Stone.HuLuBlog.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());

            //注册自定义全局异常处理过滤器
            filters.Add(new GlobalExceptionHandleAttribute());
            //注册自定义全局登录验证过滤器
            filters.Add(new AuthAttribute());
        }
    }
}
