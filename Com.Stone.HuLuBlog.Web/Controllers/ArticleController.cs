﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        
        /// <summary>
        /// 发布文章页面
        /// </summary>
        /// <param name="articleID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddArticle(string articleID)
        {
            if(articleID.IsNullOrEmpty())
            {
                return View();
            }
            else
            {
                var article = ArticleService.GetByClause(a => a.ID == articleID.Trim() && a.IsDelete == false);
                if(article != null && article.UserID == User.ID)
                {
                    return View(article.ToModel());
                }
                else
                {
                    return View();
                }
            }
        }

        /// <summary>
        /// 发布文章接口
        /// </summary>
        /// <param name="articleVM"></param>
        /// <returns></returns>
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
            string imageUrl = string.Empty;
            Article article;

            if (!articleVM.HtmlContent.IsNullOrEmpty())
            {
                HtmlToText convert = new HtmlToText();
                articleContent = convert.Convert(articleVM.HtmlContent);
                articleContent = articleContent.Length < 300 ? articleContent : articleContent.Substring(0, 300);
                var imageUrlArray = Utils.GetHtmlImageUrlList(articleVM.HtmlContent);
                if (imageUrlArray.Length > 0) imageUrl = imageUrlArray[0];
            }

            //判断是新增还是修改
            if (articleVM.ID.IsNullOrEmpty())
            {
                article = new Article()
                {
                    ArticleTitle = articleVM.ArticleTitle,
                    ArticleContent = articleContent,
                    HtmlContent = articleVM.HtmlContent,
                    MarkDownContent = articleVM.MarkDownContent,
                    IsDelete = false,
                    TagID = articleVM.TagID,
                    TagName = articleVM.TagName,
                    UserID = User.ID,
                    UserName = User.UserName,
                    ImagePath = imageUrl
                };


                ArticleService.AddArticleWithTag(article, article.TagID);
            }
            else
            {
                article = ArticleService.GetByClause(a => a.ID == articleVM.ID.Trim() && a.IsDelete == false);
                if(article!= null && article.UserID == User.ID)
                {
                    article.ArticleTitle = articleVM.ArticleTitle;
                    article.ArticleContent = articleContent;
                    article.HtmlContent = articleVM.HtmlContent;
                    article.MarkDownContent = articleVM.MarkDownContent;
                    article.TagID = articleVM.TagID;
                    article.TagName = articleVM.TagName;
                    article.ImagePath = imageUrl;

                    ArticleService.Update(article);
                }
                else
                {
                    return Json(ResponseModel.Error("编辑失败"), JsonRequestBehavior.DenyGet);
                }
            }


            return Json(ResponseModel.Success("提交成功",article.ID),JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 编辑器上传图片接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> UploadImages()
        {
            if (Request.Files.Count == 0) throw new Exception("上传错误");

            var files = Request.Files;
            string fileName = files[0].FileName;
            string fileExtention = Path.GetExtension(fileName);
            string path = Utils.GetGuidStr() + fileExtention;
            string basePath = Server.MapPath("~/Upload/Images/");
            if (!Directory.Exists(basePath)) Directory.CreateDirectory(basePath);
            string serverPath = basePath + path;

            using (FileStream fs = System.IO.File.Create(serverPath))
            {
                await files[0].InputStream.CopyToAsync(fs);
            }

            return Json(new
            {
                success = 1,
                message = "upload success",
                url = Url.Content(App_Start.Configurations.UPLOAD_PATH) + path
            }, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 文章详情页面
        /// </summary>
        /// <param name="articleID"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ArticleDetail(string articleID)
        {
            var article = ArticleService.GetByClause(a => a.ID == articleID && a.IsDelete == false);
            if (article.UserID != User.ID)//如果访问文章的用户非文章作者则阅读量+1
            {
                ArticleService.UpdateArticleReadCount(article.ID);
            }
            var articleVM = article.ToModel();
            ViewBag.UserID = User.ID;
            return View(articleVM);
        }

        /// <summary>
        /// 假删除文章接口
        /// </summary>
        /// <param name="articleID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SoftDeleteArticle(string articleID)
        {
            Article article = ArticleService.GetByClause(a => a.ID == articleID && a.IsDelete == false);
            if(article != null && article.UserID == User.ID)
            {
                article.IsDelete = true;
                article.DeleteDate = DateTime.Now;
                ArticleService.UpdateArticleDeleteStatus(article);
                return Json(ResponseModel.Success("删除成功"), JsonRequestBehavior.DenyGet);
            }
            else 
            {
                return Json(ResponseModel.Error("删除失败"), JsonRequestBehavior.DenyGet);
            }
        }

        /// <summary>
        /// 彻底删除文章接口
        /// </summary>
        /// <param name="articleID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult HardDeleteArticle(string articleID)
        {

            var article = ArticleService.GetByPkValue(articleID);

            if (article != null && article.IsDelete && article.UserID == User.ID)
            {
                ArticleService.DeleteArticleWithTag(article, article.TagID);
                return Json(ResponseModel.Success("删除成功"));
            }
            else
            {
                return Json(ResponseModel.Error("删除失败，无效的文章id"));
            }
        }

        /// <summary>
        /// 恢复文章删除状态接口
        /// </summary>
        /// <param name="articleID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RecoverArticle(string articleID)
        {
            var article = ArticleService.GetByPkValue(articleID);

            if (article != null && article.IsDelete && article.UserID == User.ID)
            {
                ArticleService.UpdateArticleDeleteStatus(new Article() { ID = articleID, IsDelete = false, DeleteDate = null });
                return Json(ResponseModel.Success("恢复成功"));
            }
            else
            {
                return Json(ResponseModel.Error("恢复失败，无效的文章id"));
            }
        }

        /// <summary>
        /// 文章分页列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ChildActionOnly]
        public PartialViewResult ArticleListPartial(int pageSize)
        {
            ViewBag.PageSize = pageSize;
            return PartialView();
        }

        /// <summary>
        /// 文章分页列表视图模板
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public PartialViewResult ArticleListTemplatePartial(int pageIndex = 1, int pageSize = 10)
        {
            var pages = ArticleService.GetPagedList(a => a.IsDelete == false, pageIndex, pageSize,
                SortOrder.Descending, a => a.AddDateTime);
            var pageListVM = pages.PageData.MapTo<List<Article>, List<ArticleVM>>();

            ViewBag.TotalCount = pages.TotalRecords;
            return PartialView(pageListVM);
        }
    }
}