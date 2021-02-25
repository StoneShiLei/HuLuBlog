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
using Com.Stone.HuLuBlog.Web.Models;
using Configurations = Com.Stone.HuLuBlog.Web.App_Start.Configurations;

namespace Com.Stone.HuLuBlog.Web.Controllers
{

    public class AccountController : BaseController
    {
        public AccountController(IUserService userService) : base(userService)
        {
        }

        /// <summary>
        /// 登陆页面
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 登陆接口
        /// </summary>
        /// <param name="userVM"></param>
        /// <param name="isRemember">是否记住用户</param>
        /// <param name="recaptchaToken">验证码token</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public JsonResult Login(UserVM userVM,bool isRemember,string recaptchaToken)
        {
            //模型验证
            if (!ModelState.IsValid)
            {
                List<string> errors = new List<string>();
                foreach (var key in ModelState.Keys.ToList())
                {
                    foreach (var error in ModelState[key].Errors.ToList())
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                return Json(ResponseModel.Error("登陆失败：" + errors[0]), JsonRequestBehavior.DenyGet);
            }

            //验证码
            if(!RecaptchaV3Operation.ReCaptchaPassed(recaptchaToken,Request.UserHostAddress))
            {
                if (recaptchaToken.IsNullOrEmpty()) //token为null一般为前台验证码接口加载延迟
                {
                    ModelState.AddModelError("recaptcha", "请等待人机验证加载完毕");
                    string error = ModelState["recaptcha"].Errors[0].ErrorMessage;
                    return Json(ResponseModel.Error(error), JsonRequestBehavior.DenyGet);
                }
                else
                {
                    ModelState.AddModelError("recaptcha", "您未通过人机验证");
                    string error = ModelState["recaptcha"].Errors[0].ErrorMessage;
                    return Json(ResponseModel.Error("登陆失败：" + error), JsonRequestBehavior.DenyGet);
                }
            }

            object data = null;
            string md5Pwd = Utils.MD5Password(userVM.Password.Trim());
            User user = UserService.GetByClause(x => x.Email == userVM.Email.Trim() && x.Password == md5Pwd);
            if(user != null)
            {
                //记住密码信息存入到cookie里面
                if (isRemember)
                {
                    HttpCookie cookie = new HttpCookie(Configurations.COOKIE_KEY);
                    cookie.Values.Add("Email", userVM.Email.Trim());
                    cookie.Values.Add("Password", userVM.Password.Trim());
                    cookie.Values.Add("IsRemember", isRemember.ToString());
                    cookie.Expires = DateTime.Now.AddDays(30);
                    HttpContext.Response.Cookies.Add(cookie);
                }
                else
                {
                    //如果没有勾选则立即过期
                    if (HttpContext.Request.Cookies[Configurations.COOKIE_KEY] != null)
                    {
                        HttpCookie cookies = HttpContext.Request.Cookies[Configurations.COOKIE_KEY];
                        cookies.Expires = DateTime.Now.AddDays(-1); //立即过期
                        HttpContext.Response.Cookies.Add(cookies);
                    }
                }

                //登录成功后将user插入缓存
                HttpRuntimeCache cache = new HttpRuntimeCache();
                cache.Add(user.ID, user, Configurations.TOKEN_TIME * 60);
                User = user;//防止意外情况出现直接赋值给basecontroller
                string token = TokenOperation.SetToken(user.ID,Configurations.TOKEN_TIME);

                //加入到cookie
                HttpCookie tokenCookie = new HttpCookie(Configurations.TOKEN_KEY);
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

        /// <summary>
        /// 注销登陆接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult LogOut()
        {
            string id = TokenOperation.GetIdAndRefreshToken(Token,Configurations.TOKEN_TIME);
            if(id.IsNullOrEmpty())
            {
                return Json(ResponseModel.Error("注销失败，系统异常"), JsonRequestBehavior.AllowGet);
            }

            TokenOperation.DeleteCacheByToken(Token);
            TokenOperation.DeleteCacheByToken(id);
            HttpCookie cookie = new HttpCookie(Configurations.COOKIE_KEY)
            {
                Expires = DateTime.Now.AddDays(-1)
            };
            HttpCookie tokenCookie = new HttpCookie(Configurations.TOKEN_KEY)
            {
                Expires = DateTime.Now.AddDays(-1)
            };
            HttpContext.Response.Cookies.Add(cookie);
            HttpContext.Response.Cookies.Add(tokenCookie);
            return Json(ResponseModel.Success("注销成功"), JsonRequestBehavior.AllowGet);
        }
    }
}