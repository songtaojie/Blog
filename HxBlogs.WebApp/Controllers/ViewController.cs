using Hx.Common.Helper;
using HxBlogs.IBLL;
using HxBlogs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HxBlogs.WebApp.Controllers
{
    [RoutePrefix("article")]
    public class ViewController : Controller
    {
        private IUserService _userService;
        private IBlogService _blogService;
        public ViewController(IUserService userService,IBlogService blogService)
        {
            this._userService = userService;
            this._blogService = blogService;
        }
        [Route("{userId}/{blogId}")]
        public ActionResult Index(string userId,string blogId)
        {
            //查询是否存在当前用户
            if (!string.IsNullOrEmpty(userId))
                userId = userId.Trim();
            User user = _userService.QueryEntity(u=>u.UserName == userId);
            Blog blog = _blogService.QueryEntityByID(Convert.ToInt32(Helper.FromHex(blogId)));
            if (user == null || blog==null) throw new NotFoundException("找不到您访问的页面!");
            ViewBag.User = user;
            return View(blog);
        }
    }
}