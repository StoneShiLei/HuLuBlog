using Com.Stone.HuLuBlog.Application;
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
            //List<Article> list = new List<Article>();
            //for(int i=0;i<=60;i++)
            //{
            //    Article article = new Article()
            //    {
            //        ArticleContent = "测试文本测试文本测试文本测试文本测试文本测试文本测试文本测试文本测试文本测试文本测试文本" +
            //        "测试文本测试文本测试文本测试文本测试文本测试文本测试文本测试文本测试文本测试文本" +
            //        "测试文本测试文本测试文本测试文本测试文本测试文本测试文本测试文本测试文本测试文本",
            //        ArticleTitle = "这是标题这是标题这是标题",
            //        CommentCount = 20,
            //        IsDelete = false,
            //        ReadCount = 6000,
            //        TagID = "xxx",
            //        TagName = "闲聊",
            //        UserID = User.ID,
            //        UserName = User.UserName,

            //    };

            //    list.Add(article);

            //}

            //ArticleService.Add(list);

            //var a = new ArticleTag() { TagName = "闲聊", UserID = "49e1417ff5ff479f9ed3dc67d83dbf62" };
            //var b = new ArticleTag() { TagName = "C#", UserID = "49e1417ff5ff479f9ed3dc67d83dbf62" };
            //var c = new ArticleTag() { TagName = "前端", UserID = "49e1417ff5ff479f9ed3dc67d83dbf62" };
            //var d = new ArticleTag() { TagName = "后端", UserID = "49e1417ff5ff479f9ed3dc67d83dbf62" };
            //var e = new ArticleTag() { TagName = ".Net", UserID = "49e1417ff5ff479f9ed3dc67d83dbf62" };
            //var f = new ArticleTag() { TagName = ".Net Core", UserID = "49e1417ff5ff479f9ed3dc67d83dbf62" };

            //ArticleTagService.Add(new List<ArticleTag>() { a, b, c, d, e, f });

            return Json("1", JsonRequestBehavior.AllowGet);
        }
        
    }
}