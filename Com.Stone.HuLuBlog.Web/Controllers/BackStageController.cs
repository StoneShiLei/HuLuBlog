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
    public class BackStageController : BaseController
    {
        readonly IArticleService ArticleService;
        readonly IArticleTagService ArticleTagService;

        public BackStageController(IUserService userService,IArticleService articleService,
            IArticleTagService articleTagService) : base(userService)
        {
            ArticleService = articleService;
            ArticleTagService = articleTagService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            //文章回收站
            var articles = ArticleService.GetAllByClause(a => a.UserID == User.ID && a.IsDelete == true).ToList();
            var articleVMs = articles.MapTo<List<Article>, List<ArticleVM>>().OrderByDescending(a => a.DeleteDate).ToList();
            return View(articleVMs);
        }

        [HttpGet]
        public JsonResult TagList()
        {
            var tags = ArticleTagService.GetAllByClause(t => t.UserID == User.ID).ToList().MapTo<List<ArticleTag>, List<ArticleTagVM>>()
                .OrderByDescending(t => t.ArticleCount).ToList();

            return Json(ResponseModel.Success("获取成功", new 
            { 
                code = 0,
                count = tags.Count,
                data = tags
            }),JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteArticle(string articleID)
        {

            var article = ArticleService.GetByPkValue(articleID);

            if(article!= null && article.IsDelete && article.UserID == User.ID)
            {
                ArticleService.DeleteArticleWithTag(article,article.TagID);
                return Json(ResponseModel.Success("删除成功"));
            }
            else
            {
                return Json(ResponseModel.Error("删除失败，无效的文章id"));
            }
        }

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

        [HttpPost]
        public JsonResult AddTag(ArticleTagVM articleTagVM)
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
                return Json(ResponseModel.Error("提交失败：" + errors[0]), JsonRequestBehavior.DenyGet);
            }

            var tag = new ArticleTag()
            {
                TagName = articleTagVM.TagName,
                UserID = User.ID
            };
            ArticleTagService.Add(tag);
            return Json(ResponseModel.Success("添加成功"), JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult DeleteTag(ArticleTagVM tagVM)
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
                return Json(ResponseModel.Error("提交失败：" + errors[0]), JsonRequestBehavior.DenyGet);
            }

            var tag = ArticleTagService.GetByPkValue(tagVM.ID);
            if(tag != null && tag.UserID == User.ID && tag.ArticleCount <= 0)
            {
                ArticleTagService.Remove(tag.ID);
                return Json(ResponseModel.Success("删除成功"), JsonRequestBehavior.DenyGet);
            }
            else
            {
                return Json(ResponseModel.Error("删除失败，该分类下有文章存在"), JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        public JsonResult RenameTag(ArticleTagVM tagVM)
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
                return Json(ResponseModel.Error("提交失败：" + errors[0]), JsonRequestBehavior.DenyGet);
            }

            var tag = ArticleTagService.GetByPkValue(tagVM.ID);
            if(tag!=null && tag.UserID == User.ID)
            {
                ArticleService.UpdateArticleTagName(tag.ID, tagVM.TagName);
                return Json(ResponseModel.Success("标签名修改成功"));
            }
            else
            {
                return Json(ResponseModel.Error("重命名失败，标签id不存在"));
            }
        }


    }
}