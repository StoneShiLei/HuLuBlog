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
    public class TagController : BaseController
    {
        readonly IArticleService ArticleService;
        readonly IArticleTagService ArticleTagService;

        public TagController(IUserService userService, IArticleService articleService,
            IArticleTagService articleTagService) : base(userService)
        {
            ArticleService = articleService;
            ArticleTagService = articleTagService;
        }

        /// <summary>
        /// 文章专栏tag列表
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult TagListPartial()
        {
            var tags = ArticleTagService.GetAllByClause(t => t.ArticleCount > 0).OrderByDescending(t => t.AddDateTime).ToList().MapTo<List<ArticleTag>, List<ArticleTagVM>>();
            return PartialView(tags);
        }

        /// <summary>
        /// 后台页面的tag管理表格接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult TagManageList()
        {
            var tags = ArticleTagService.GetAllByClause(t => t.UserID == User.ID).OrderByDescending(t => t.AddDateTime).ToList().MapTo<List<ArticleTag>, List<ArticleTagVM>>();

            return Json(ResponseModel.Success("获取成功", new
            {
                code = 0,
                count = tags.Count,
                data = tags
            }), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加tag
        /// </summary>
        /// <param name="articleTagVM"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 删除tag
        /// </summary>
        /// <param name="tagVM"></param>
        /// <returns></returns>
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
            if (tag != null && tag.UserID == User.ID && tag.ArticleCount <= 0)
            {
                ArticleTagService.Remove(tag.ID);
                return Json(ResponseModel.Success("删除成功"), JsonRequestBehavior.DenyGet);
            }
            else
            {
                return Json(ResponseModel.Error("删除失败，该分类下有文章存在"), JsonRequestBehavior.DenyGet);
            }
        }

        /// <summary>
        /// 重命名tag
        /// </summary>
        /// <param name="tagVM"></param>
        /// <returns></returns>
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
            if (tag != null && tag.UserID == User.ID)
            {
                ArticleService.UpdateArticleTagName(tag.ID, tagVM.TagName);
                return Json(ResponseModel.Success("标签名修改成功"));
            }
            else
            {
                return Json(ResponseModel.Error("重命名失败，标签id不存在"));
            }
        }

        /// <summary>
        /// 标签下拉框
        /// </summary>
        /// <param name="tagID"></param>
        /// <returns></returns>
        [ChildActionOnly]
        public PartialViewResult ArticleTagPartial(string tagID = null)
        {
            if (tagID == null) tagID = string.Empty;
            ViewBag.TagID = tagID;

            var articleTags = ArticleTagService.GetAll().ToList()
                .MapTo<List<ArticleTag>, List<ArticleTagVM>>()
                .OrderByDescending(t => t.ArticleCount).ToList();
            articleTags.Insert(0, new ArticleTagVM() { ID = string.Empty, TagName = string.Empty });

            return PartialView(articleTags);
        }
    }
}