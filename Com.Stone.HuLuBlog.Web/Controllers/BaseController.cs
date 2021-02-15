using Com.Stone.HuLuBlog.Application;
using Com.Stone.HuLuBlog.Domain.Model;
using Com.Stone.HuLuBlog.Infrastructure;
using Com.Stone.HuLuBlog.Infrastructure.Extensions;
using System;
using System.Web;
using System.Web.Mvc;
using Configurations = Com.Stone.HuLuBlog.Infrastructure.Configurations;

namespace Com.Stone.HuLuBlog.Web.Controllers
{
    public class BaseController : Controller
    {
        protected IUserService UserService;
        protected new User User;
        protected string Token;

        public BaseController(IUserService userService)
        {
            UserService = userService;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpCookie cookies = filterContext.HttpContext.Request.Cookies[App_Start.Configurations.TOKEN_KEY];
            string token = string.Empty;
            if (cookies != null && cookies.HasKeys) token = cookies["Token"];
            Token = token;

            //请求时未含有token则认为用户未登录，构造一个空的User以供使用
            if (token.IsNullOrEmpty())
            {
                User = new User()
                {
                    ID = string.Empty,
                    Email = string.Empty,
                    UserName = string.Empty,
                    Password = string.Empty,
                    AddDateTime = DateTime.Now
                };
            }
            else
            {
                string userID = TokenOperation.GetIdByToken(token);
                if (userID.IsNullOrEmpty()) throw new Exception("获取用户id失败");

                //从缓存中获取已登录的用户信息，如没有，则从数据库获取，且存入缓存
                HttpRuntimeCache cache = new HttpRuntimeCache();
                User user = cache.Get<User>(userID);

                if (user.IsNullOrEmpty())
                {
                    user = UserService.GetByPkValue(userID);
                    cache.Add(userID, user, Configurations.TOKEN_TIME * 60);
                }

                User = user;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}