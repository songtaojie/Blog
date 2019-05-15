using HxBlogs.IBLL;
using HxBlogs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HxBlogs.WebApp.Controllers
{
    [RoutePrefix("view")]
    public class ViewController : Controller
    {
        private IUserInfoService _userService;
        public ViewController(IUserInfoService userService)
        {
            this._userService = userService;
        }
        [Route("{userId}/{blogId}")]
        // GET: View
        public ActionResult Index(string userId,string blogId)
        {
            //查询是否存在当前用户
            if (!string.IsNullOrEmpty(userId))
                userId = userId.Trim();
            UserInfo user = _userService.QueryEntity(u=>u.UserName == userId);
            if (user == null) throw new NotFoundException("找不到您访问的页面");
            ViewData["user"] = userId;
            return View();
        }
    }
}