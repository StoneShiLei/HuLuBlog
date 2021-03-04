using Com.Stone.HuLuBlog.Application;
using Com.Stone.HuLuBlog.Domain.Model;
using Com.Stone.HuLuBlog.Infrastructure;
using Com.Stone.HuLuBlog.Infrastructure.Extensions;
using Com.Stone.HuLuBlog.Message;
using Com.Stone.HuLuBlog.Web.Models;
using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Com.Stone.HuLuBlog.Web.Controllers
{
    [AllowAnonymous]
    public class CommentController : BaseController
    {
        ICommentService CommentService { get; }
        IArticleService ArticleService { get; }
        IBus Bus { get; }

        public CommentController(IUserService userService,ICommentService commentService,IArticleService articleService,IBus bus) : base(userService)
        {
            UserService = userService;
            CommentService = commentService;
            ArticleService = articleService;
            Bus = bus;
        }

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="articleID"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)] //html文本会引发“有潜在危险的 Request.Form 值
        public JsonResult AddComment(string recaptchaToken, CommentVM commentVM)
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
                return Json(ResponseModel.Error("评论失败：" + errors[0]), JsonRequestBehavior.DenyGet);
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
                    return Json(ResponseModel.Error("评论失败：" + error), JsonRequestBehavior.DenyGet);
                }
            }

            commentVM.CommentContent = Utils.ReplaceHtmlTag(commentVM.CommentContent);//去除html标签
            commentVM.AddDateTime = DateTime.Now;
            commentVM.ID = Utils.GetGuidStr();
            commentVM.UserID = User.ID;

            commentVM.ArticleID = commentVM.ArticleID == null ? string.Empty : commentVM.ArticleID;
            commentVM.PID = commentVM.PID == null ? string.Empty : commentVM.PID;
            commentVM.ReplyTo = commentVM.ReplyTo == null ? string.Empty : commentVM.ReplyTo;
            commentVM.ReplyToID = commentVM.ReplyToID == null ? string.Empty : commentVM.ReplyToID;

            //检查父级评论id
            if (!commentVM.PID.IsNullOrEmpty())
            {
                commentVM.IsChild = true; //防止篡改 包含pid的评论一定为子评论
                var parentComment = CommentService.GetByPkValue(commentVM.PID);
                if(parentComment == null) return Json(ResponseModel.Error("评论失败：回复的评论不存在"), JsonRequestBehavior.DenyGet);

                //发送邮件通知
                if(!commentVM.ReplyToID.IsNullOrEmpty())
                {
                    var replyComment = CommentService.GetByPkValue(commentVM.ReplyToID);

                    //验证回复目标的同一性
                    if (replyComment.UserName != commentVM.ReplyTo || replyComment.ArticleID != commentVM.ArticleID)
                    {
                        return Json(ResponseModel.Error("回复失败：无效的回复目标"), JsonRequestBehavior.DenyGet);
                    }

                    if (replyComment.IsReceive) //判断被回复者是否接收通知
                    {
                        var email = new EmailMessage()
                        {
                            To = parentComment.Email,
                            From = commentVM.Email,
                            Subject = "您在HuLuBlog的评论有新的回复",
                            Body = string.Format("您的评论：{0}\r\n 【{1}】 回复了您的评论：{2} \r\n 点击链接进行查看：{3}"
                            ,replyComment.CommentContent ,commentVM.UserName, commentVM.CommentContent, Request.UrlReferrer + Request.QueryString.ToString())
                        };

                        Bus.PubSub.PublishAsync(email);
                    }
                }
            }

            CommentService.AddCommentWithCommentCount(commentVM.ToEntity(),commentVM.ArticleID);



            //将评论的用户名和邮箱、通知设置写入cookie
            HttpCookie cookie = new HttpCookie(App_Start.Configurations.COMMENT_KEY);
            cookie.Values.Add("CommentEmail", HttpUtility.UrlEncode(commentVM.Email)); //字符编码  防止中文在cookie里变为乱码
            cookie.Values.Add("CommentUserName", HttpUtility.UrlEncode(commentVM.UserName));
            cookie.Values.Add("CommentIsReceive", commentVM.IsReceive.ToString());
            cookie.Expires = DateTime.Now.AddDays(30);
            HttpContext.Response.Cookies.Add(cookie);

            return Json(ResponseModel.Success("发表成功",commentVM.ID), JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 删除评论 仅限管理员
        /// </summary>
        /// <param name="commentID"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult DeleteComment(string commentID,string articleID)
        {
            var comment = CommentService.GetByPkValue(commentID);

            if (comment != null && !User.ID.IsNullOrEmpty())
            {
                CommentService.RemoveCommentWithCommentCount(comment.ID,articleID);

                return Json(ResponseModel.Success("删除成功"));
            }
            else
            {
                return Json(ResponseModel.Error("删除失败，没有权限或无效的评论id"));
            }
        }

        /// <summary>
        /// 评论列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult CommentListPartial(string articleID = "",int pageIndex = 1,int pageSize = 10)
        {
            ViewBag.UserID = User.ID;
            ViewBag.ArticleID = articleID;
            var commentPagedList = CommentService.GetAllCommentByArticleID(articleID, pageIndex, pageSize);
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = commentPagedList.TotalRecords;
            var commentList = commentPagedList.PageData;
            return PartialView(commentList.MapTo<List<Comment>,List<CommentVM>>());
        }

        /// <summary>
        /// 评论列表模块
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult CommentMoudlePartial(string articleID = "", string pid = "", string replyTo = "",string replyToID="", bool isChild = false)
        {
            ViewBag.UserID = User.ID;
            ViewBag.ArticleID = articleID;
            ViewBag.Legend = isChild ? "回复给：" + replyTo : "发表评论";
            ViewBag.PID = pid;
            ViewBag.IsChild = isChild;
            ViewBag.ReplyTo = replyTo;
            ViewBag.ReplyToID = replyToID;
            ViewBag.PageSize = 10;

            HttpCookie cookies = Request.Cookies[App_Start.Configurations.COMMENT_KEY];
            ViewBag.IsReceive = true;
            if (cookies != null && cookies.HasKeys)
            {
                ViewBag.Email = HttpUtility.UrlDecode(cookies["CommentEmail"]);
                ViewBag.UserName = HttpUtility.UrlDecode(cookies["CommentUserName"]);
                ViewBag.IsReceive = Convert.ToBoolean(cookies["CommentIsReceive"]);
            }

            return PartialView();
        }

    }
}