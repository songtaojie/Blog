using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HxBlogs.WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// 用户中心
    /// </summary>
    public class UserCenterController : Controller
    {
        // GET: Admin/UserCenter
        public ActionResult Index()
        {
            return View();
        }
    }
}