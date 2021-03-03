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
        readonly IFriendLinkService FriendLinkService;

        public BackStageController(IUserService userService, IArticleService articleService,
            IArticleTagService articleTagService, IFriendLinkService friendLinkService) : base(userService)
        {
            ArticleService = articleService;
            ArticleTagService = articleTagService;
            FriendLinkService = friendLinkService;
        }

        /// <summary>
        /// 后台管理页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            //文章回收站
            var articles = ArticleService.GetAllByClause(a => a.UserID == User.ID && a.IsDelete == true).ToList();
            var articleVMs = articles.MapTo<List<Article>, List<ArticleVM>>().OrderByDescending(a => a.DeleteDate).ToList();
            return View(articleVMs);
        }

        /// <summary>
        /// 添加友链
        /// </summary>
        /// <param name="friendLinkVM"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddFriendLink(FriendLinkVM friendLinkVM)
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

            var friendLink = new FriendLink()
            {
                Title = friendLinkVM.Title,
                Domain = friendLinkVM.Domain,
                Url = friendLinkVM.Url,
                IconUrl = friendLinkVM.IconUrl
            };
            FriendLinkService.Add(friendLink);
            return Json(ResponseModel.Success("添加成功"), JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 修改友链
        /// </summary>
        /// <param name="friendLinkVM"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateFriendLink(FriendLinkVM friendLinkVM)
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


            var friendLink = FriendLinkService.GetByPkValue(friendLinkVM.ID);
            if (friendLink != null)
            {
                friendLink.Domain = friendLinkVM.Domain;
                friendLink.IconUrl = friendLinkVM.IconUrl;
                friendLink.Title = friendLinkVM.Title;
                friendLink.Url = friendLinkVM.Url;
                FriendLinkService.Update(friendLink);
                return Json(ResponseModel.Success("友链修改成功"));
            }
            else
            {
                return Json(ResponseModel.Error("友链修改失败，id不存在"));
            }
        }

        /// <summary>
        /// 删除友链
        /// </summary>
        /// <param name="friendLinkVM"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteFriendLink(FriendLinkVM friendLinkVM)
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

            var friendLink = FriendLinkService.GetByPkValue(friendLinkVM.ID);
            if (friendLink != null)
            {
                FriendLinkService.Remove(friendLink.ID);
                return Json(ResponseModel.Success("删除成功"), JsonRequestBehavior.DenyGet);
            }
            else
            {
                return Json(ResponseModel.Error("删除失败，该友链不存在"), JsonRequestBehavior.DenyGet);
            }
        }

        /// <summary>
        /// 获取全部友链
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult FriendLinkList()
        {
            var friendLinks = FriendLinkService.GetAll().OrderBy(t => t.AddDateTime).ToList().MapTo<List<FriendLink>, List<FriendLinkVM>>();

            return Json(ResponseModel.Success("获取成功", new
            {
                code = 0,
                count = friendLinks.Count,
                data = friendLinks
            }), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 更新全部文章索引
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateIndex()
        {
            try
            {
                ArticleService.UpdateAllArticleIndex();
                return Json(ResponseModel.Success("更新成功"), JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(ResponseModel.Error("更新失败", ex), JsonRequestBehavior.DenyGet);
            }
        }

    }
}