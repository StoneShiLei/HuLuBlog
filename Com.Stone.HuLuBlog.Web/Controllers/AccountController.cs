using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Com.Stone.HuLuBlog.Application;
using Com.Stone.HuLuBlog.Domain.Model;
using Com.Stone.HuLuBlog.Infrastructure;
using Com.Stone.HuLuBlog.Infrastructure.Extensions;
using Com.Stone.HuLuBlog.Web.App_Start;
using Configurations = Com.Stone.HuLuBlog.Infrastructure.Configurations;

namespace Com.Stone.HuLuBlog.Web.Controllers
{

    public class AccountController : BaseController
    {
        public AccountController(IUserService userService) : base(userService)
        {
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            HttpCookie cookies = Request.Cookies[App_Start.Configurations.COOKIE_KEY];
            ViewBag.IsChecked = false;
            if(cookies != null && cookies.HasKeys)
            {
                ViewBag.Email = cookies["Email"];
                ViewBag.Password = cookies["Password"];
                ViewBag.IsChecked = Convert.ToBoolean(cookies["IsRemember"]);
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public JsonResult Login(string email,string password,bool isRemember)
        {
            object data = null;

            string md5Pwd = Utils.MD5Password(password.Trim());
            User user = UserService.GetByClause(x => x.Email == email.Trim() && x.Password == md5Pwd);
            if(user != null)
            {
                //记住密码信息存入到cookie里面
                if (isRemember)
                {
                    HttpCookie cookie = new HttpCookie(App_Start.Configurations.COOKIE_KEY);
                    cookie.Values.Add("Email", email.Trim());
                    cookie.Values.Add("Password", password.Trim());
                    cookie.Values.Add("IsRemember", isRemember.ToString());
                    cookie.Expires = DateTime.Now.AddDays(30);
                    HttpContext.Response.Cookies.Add(cookie);
                }
                else
                {
                    //如果没有勾选则立即过期
                    if (HttpContext.Request.Cookies[App_Start.Configurations.COOKIE_KEY] != null)
                    {
                        HttpCookie cookies = HttpContext.Request.Cookies[App_Start.Configurations.COOKIE_KEY];
                        cookies.Expires = DateTime.Now.AddDays(-1); //立即过期
                        HttpContext.Response.Cookies.Add(cookies);
                    }
                }

                //登录成功后将user插入缓存
                HttpRuntimeCache cache = new HttpRuntimeCache();
                cache.Add(user.ID, user, Configurations.TOKEN_TIME * 60);
                User = user;//防止意外情况出现直接赋值给basecontroller

                string token = TokenOperation.SetToken(user.ID);
                HttpCookie tokenCookie = new HttpCookie(App_Start.Configurations.TOKEN_KEY);
                tokenCookie.Values.Add("Token", token);
                tokenCookie.Expires = DateTime.Now.AddMinutes(Configurations.TOKEN_TIME);
                HttpContext.Response.Cookies.Add(tokenCookie);

                data = ResponseModel.Success();
            }
            else
            {
                data = ResponseModel.Error("用户名密码错误");
            }

            return Json(data, JsonRequestBehavior.DenyGet);
        } 


        [HttpGet]
        public JsonResult LogOut()
        {
            string id = TokenOperation.GetIdByToken(Token);
            if(id.IsNullOrEmpty())
            {
                return Json(ResponseModel.Error("注销失败，系统异常"), JsonRequestBehavior.AllowGet);
            }

            TokenOperation.DeleteCacheByToken(Token);
            TokenOperation.DeleteCacheByToken(id);
            HttpCookie cookie = new HttpCookie(App_Start.Configurations.COOKIE_KEY)
            {
                Expires = DateTime.Now.AddDays(-1)
            };
            HttpCookie tokenCookie = new HttpCookie(App_Start.Configurations.TOKEN_KEY)
            {
                Expires = DateTime.Now.AddDays(-1)
            };
            HttpContext.Response.Cookies.Add(cookie);
            HttpContext.Response.Cookies.Add(tokenCookie);
            return Json(ResponseModel.Success("注销成功"), JsonRequestBehavior.AllowGet);
        }
    }
}