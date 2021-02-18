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
    [AllowAnonymous]
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

        /// <summary>
        /// 主页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //热文排行
            List<Article> articles = ArticleService.GetAllByClause(a => a.IsDelete == false).OrderByDescending(a => a.ReadCount + a.CommentCount).Take(10).ToList();
            List<ArticleVM> articleVMs = articles.MapTo<List<Article>, List<ArticleVM>>();

            ViewBag.UserID = User.ID;
            return View(articleVMs);
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


        
        public JsonResult Test()
        {
            //var documentmodle = new DocumentModel()
            //{
            //    ID="1",Title= "主要特性 支持“标准”Markdown",
            //    Content= " / CommonMark和Github风格的语法，也可变身为代码编辑器； 支持实时预览、图片（跨域）上传、预格式文本/代码/表格插入、代码折叠、搜索替换、只读模式、自定义样式主" +
            //        "题和多语言语法高亮等功能； 支持ToC（Table of Contents）、Emoji表情、Task lists、@链接等Markdown扩展语法； 支持TeX科学公式"
            //};

            //LuceneOperation.AddIndex(documentmodle);
            //string a = "看了源码后有一个疑问，Dispatcher模式最终代码仍然是在主线程上执行的，而我在Unity中只有在有性" +
            //    "能需求时才才会使用线程，只是需要并行执行时使用协程就可以了。假如只是需要在线程代码上使用";
            //string b = "就可以大显身手了。本文主要从无到有讲述lucene.net的用法！ ... 多个排序条件。(建议采用默认的积分排序," +
            //    "设计良好的加权机制) ... Lucene.net(4.8.0) 学习问题记录六：Lucene 的索引系统和搜索过程分析 · weixin_30420305的 ...";
            //string c = " 详细计算方法Lucene源码(二)：文本相似度TF-IDF原理 ... 但是Lucene的计算方式不一样，它还引入了文档长度的加权因子，作用就是提高短" +
            //    "文档的分数， ... Lucene.net(4.8.0) 学习问题记录六：Lucene 的索引系统和搜索过程分析.";
            //var aa = new DocumentModel() { ID = "aa",Title = "源码",Content = a};
            //var bb = new DocumentModel() { ID = "bb", Title = "排序",Content = b};
            //var cc = new DocumentModel() { ID = "cc", Title = "计算",Content = c};
            //LuceneOperation.AddIndex(aa);
            //LuceneOperation.AddIndex(bb);
            //LuceneOperation.AddIndex(cc);


            return Json(LuceneOperation.Search("源码"), JsonRequestBehavior.AllowGet);
            //return Json("111", JsonRequestBehavior.AllowGet);
        }
        
    }
}