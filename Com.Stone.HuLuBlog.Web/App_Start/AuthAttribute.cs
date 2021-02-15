using Com.Stone.HuLuBlog.Infrastructure;
using Com.Stone.HuLuBlog.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Com.Stone.HuLuBlog.Web.App_Start
{
    public class AuthAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                //是否跳过登录验证
                if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                   || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
                {
                    return;
                }

                HttpCookie cookies = filterContext.HttpContext.Request.Cookies[Configurations.TOKEN_KEY];
                string token = string.Empty;
                if (cookies != null && cookies.HasKeys) token = cookies["Token"];

                if (token.IsNullOrEmpty()) throw new Exception("token为空");

                string userGuid = TokenOperation.GetIdByToken(token);
                if (userGuid.IsNullOrEmpty())
                {
                    //object data = ResponseModel.Error("请先登录");
                    //JsonResult result = new JsonResult()
                    //{
                    //    Data = data,
                    //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    //};

                    //filterContext.Result = result;

                    throw new Exception("用户ID为空");
                }
                else
                {
                    //pass 通过登录
                }
            }
            catch (Exception ex)
            {
                //跳转到主页
                filterContext.Result = new RedirectToRouteResult(
                        new System.Web.Routing.RouteValueDictionary(new
                        {
                            controller = "Home",
                            action = "Index"
                        }));
                //throw new Exception("请先登录！", ex);
            }
        }
    }
}