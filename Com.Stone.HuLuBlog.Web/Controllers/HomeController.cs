﻿using Com.Stone.HuLuBlog.Application;
using Com.Stone.HuLuBlog.Domain.Model;
using Com.Stone.HuLuBlog.Infrastructure;
using Com.Stone.HuLuBlog.Infrastructure.Extensions;
using Com.Stone.HuLuBlog.Message;
using Com.Stone.HuLuBlog.Web.Models;
using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Com.Stone.HuLuBlog.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : BaseController
    {
        readonly IArticleService ArticleService;
        readonly IArticleTagService ArticleTagService;
        readonly IBus Bus;

        public HomeController(IUserService userService,IArticleService articleService,
            IArticleTagService articleTagService,IBus bus) : base(userService)
        {
            ArticleService = articleService;
            ArticleTagService = articleTagService;
            Bus = bus;
        }

        /// <summary>
        /// 主页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.UserID = User.ID;
            return View();
        }

        /// <summary>
        /// 关于本站
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            return View();
        }

        /// <summary>
        /// 导航条登陆可见按钮
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public PartialViewResult ManagementButtonPartial()
        {
            return PartialView(User.ToModel());
        }

        [HttpGet]
        public ActionResult Email()
        {
            return View();
        }

        /// <summary>
        /// home about页面给博主发送邮件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SendEmail(EmailVM emailVM,string recaptchaToken)
        {
            if (emailVM.To != App_Start.Configurations.HuLuEmail) ModelState.AddModelError(string.Empty, "检测到被篡改的收件人邮箱");

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
                return Json(ResponseModel.Error("发送失败：" + errors[0]), JsonRequestBehavior.DenyGet);
            }

            //验证码
            if (!RecaptchaV3Operation.ReCaptchaPassed(recaptchaToken, Request.UserHostAddress))
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
                    return Json(ResponseModel.Error("发送失败：" + error), JsonRequestBehavior.DenyGet);
                }
            }



            var email = new EmailMessage()
            {
                To = emailVM.To,
                From = emailVM.From,
                Subject = emailVM.Subject,
                Body = emailVM.Body
            };

            Bus.PubSub.PublishAsync(email);

            return Json(ResponseModel.Success("发送成功"));
        }


        public ActionResult Test()
        {
            //var input = "hello";
            //var email = new EmailMessage() { Body = input };
            //Bus.PubSub.Publish(email);

            //ViewBag.Test = LuceneOperation.Test("C#高级知识点&(ABP框架理论学习高级篇)——白金版");
            return View();
        }
        
    }
}