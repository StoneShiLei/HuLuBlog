using Com.Stone.HuLuBlog.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Com.Stone.HuLuBlog.Web.App_Start
{
    /// <summary>
    /// 全局异常过滤器
    /// </summary>
    public class GlobalExceptionHandleAttribute : HandleErrorAttribute
    {
        public LogOperation Logger { get; }

        public GlobalExceptionHandleAttribute()
        {
            Logger = new LogOperation(typeof(GlobalExceptionHandleAttribute));
        }

        public override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                HttpException httpException = new HttpException(null, filterContext.Exception);
                var em = new ExceptionModel
                {
                    Exception = filterContext.Exception,
                    ExceptionMessage = filterContext.Exception.Message,
                    HttpCode = httpException.GetHttpCode(),
                    ContorllerName = Convert.ToString(filterContext.RouteData.Values["controller"]),
                    ActionName = Convert.ToString(filterContext.RouteData.Values["action"]),
                    QueryString = filterContext.HttpContext.Request.QueryString.ToString(),
                    Url = filterContext.HttpContext.Request.RawUrl
                };
                em.HandleErrorInfo = new HandleErrorInfo(filterContext.Exception, em.ContorllerName, em.ActionName);
                Logger.Debug(em.ToString());
                filterContext.Result = new ViewResult() { ViewName = "Error" };
            }
            base.OnException(filterContext);
        }
    }

    public class ExceptionModel
    {
        public string ContorllerName { get; set; }
        public string ActionName { get; set; }
        public string ExceptionMessage { get; set; }
        public Exception Exception { get; set; }
        public int HttpCode { get; set; }
        public HandleErrorInfo HandleErrorInfo { get; set; }
        public string Url { get; set; }
        public string QueryString { get; set; }

        public override string ToString()
        {
            return $"请求路径：{Url}，控制器：{this.ContorllerName}，方法：{this.ActionName}，路径参数：{QueryString}" +
                $"状态码：{this.HttpCode}，异常类型：{this.Exception.GetType().ToString()}，" +
                $"异常信息：{this.ExceptionMessage}";
        }
    }
}