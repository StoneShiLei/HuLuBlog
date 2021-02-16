﻿using Com.Stone.HuLuBlog.Application;
using Com.Stone.HuLuBlog.Domain.Model;
using Com.Stone.HuLuBlog.Infrastructure;
using Com.Stone.HuLuBlog.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Com.Stone.HuLuBlog.Web.Controllers
{
    public class HomeController : BaseController
    {
        readonly IArticleService ArticleService;
        readonly IArticleTagService ArticleTagService;

        public HomeController(IUserService userService,IArticleService articleService,
            IArticleTagService articleTagService) : base(userService)
        {
            ArticleService = articleService;
            ArticleTagService = articleTagService;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            //热文排行
            List<Article> articles = ArticleService.GetAllByClause(a => a.IsDelete == false).OrderByDescending(a => a.ReadCount + a.CommentCount).Take(10).ToList();
            List<ArticleVM> articleVMs = articles.MapTo<List<Article>, List<ArticleVM>>();

            ViewBag.UserID = User.ID;
            return View(articleVMs);
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public PartialViewResult BackStagePartial()
        {
            return PartialView(User.ToModel());
        }


        [AllowAnonymous]
        public JsonResult Test()
        {
            return Json("1", JsonRequestBehavior.AllowGet);
        }
        
    }
}