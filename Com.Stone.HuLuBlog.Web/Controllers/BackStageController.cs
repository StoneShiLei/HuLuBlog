using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Com.Stone.HuLuBlog.Application;
using Com.Stone.HuLuBlog.Domain.Model;
using Com.Stone.HuLuBlog.Infrastructure;

namespace Com.Stone.HuLuBlog.Web.Controllers
{
    public class BackStageController : BaseController
    {
        public BackStageController(IUserService userService) : base(userService)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}