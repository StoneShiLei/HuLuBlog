using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Com.Stone.HuLuBlog.Application;
using Com.Stone.HuLuBlog.Domain.Model;
using Com.Stone.HuLuBlog.Infrastructure;
using Com.Stone.HuLuBlog.Infrastructure.Extensions;
using Com.Stone.HuLuBlog.Web.Models;

namespace Com.Stone.HuLuBlog.Web.Controllers
{
    public class ArticleController : BaseController
    {
        readonly IArticleService ArticleService;
        readonly IArticleTagService ArticleTagService;

        public ArticleController(IUserService userService,IArticleService articleService,
            IArticleTagService articleTagService) : base(userService)
        {
            ArticleService = articleService;
            ArticleTagService = articleTagService;
        }

        [AllowAnonymous]
        public ActionResult AddArticle()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)] //html文本会引发“有潜在危险的 Request.Form 值”
        public JsonResult AddArticle(ArticleVM articleVM)
        {
            //模型验证
            if(!ModelState.IsValid)
            {
                List<string> errors = new List<string>();
                foreach (var key in ModelState.Keys.ToList())
                {
                    foreach (var error in ModelState[key].Errors.ToList())
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                return Json(ResponseModel.Error("提交失败：" + errors[0]),JsonRequestBehavior.DenyGet);
            }



            string articleContent = string.Empty;

            if(!articleVM.HtmlContent.IsNullOrEmpty())
            {
                HtmlToText convert = new HtmlToText();
                articleContent = convert.Convert(articleVM.HtmlContent);
                articleContent = articleContent.Length < 200 ? articleContent : articleContent.Substring(0, 200);
            }

            var article = new Article()
            {
                ArticleTitle = articleVM.ArticleTitle,
                ArticleContent = articleContent,
                HtmlContent = articleVM.HtmlContent,
                MarkDownContent = articleVM.MarkDownContent,
                IsDelete = false,
                TagID = articleVM.TagID,
                TagName = articleVM.TagName,
                UserID = User.ID,
                UserName = User.UserName
            };

            
            ArticleService.AddArticleWithTag(article,article.TagID);

            return Json(ResponseModel.Success("提交成功",article.ID),JsonRequestBehavior.DenyGet);
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public PartialViewResult ArticleTagPartial(string tagID = null)
        {
            if (tagID == null) tagID = string.Empty;
            ViewBag.TagID = tagID;

            var articleTags = ArticleTagService.GetAll().ToList()
                .MapTo<List<ArticleTag>,List<ArticleTagVM>>()
                .OrderByDescending(t => t.ArticleCount).ToList();
            articleTags.Insert(0,new ArticleTagVM() { ID = string.Empty, TagName = string.Empty });

            return PartialView(articleTags);
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public PartialViewResult ArticleListPartial(int pageSize)
        {
            ViewBag.PageSize = pageSize;
            return PartialView();
        }

        [AllowAnonymous]
        public PartialViewResult ArticleListTemplatePartial(int pageIndex, int pageSize)
        {
            var pages = ArticleService.GetPagedList(a => a.IsDelete == false, pageIndex, pageSize,
                SortOrder.Descending, a => a.AddDateTime);
            var pageListVM = pages.PageData.MapTo<List<Article>, List<ArticleVM>>();

            ViewBag.TotalCount = pages.TotalRecords;
            return PartialView(pageListVM);
        }

        //[AllowAnonymous]  //分页js实现
        //public JsonResult ArticleListPaged(int pageIndex, int pageSize)
        //{
        //    var pages = ArticleService.GetPagedList(a => a.IsDelete == false, pageIndex, pageSize,
        //        SortOrder.Descending, a => a.AddDateTime);
        //    var pageListVM = pages.PageData.MapTo<List<Article>, List<ArticleVM>>();

        //    return Json(ResponseModel.Success(null, new { TotalCount = pages.TotalRecords, PageList = pageListVM }), JsonRequestBehavior.AllowGet);
        //}
    }
}