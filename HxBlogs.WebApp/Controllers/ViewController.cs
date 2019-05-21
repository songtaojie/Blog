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
    [Route("{action}")]
    public class ViewController : Controller
    {
        private IUserService _userService;
        private IBlogService _blogService;
        private IBlogTagService _blogTagService;
        public ViewController(IUserService userService,IBlogService blogService,IBlogTagService blogTagServer)
        {
            this._userService = userService;
            this._blogService = blogService;
            this._blogTagService = blogTagServer;
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
            if (!string.IsNullOrEmpty(blog.BlogTags))
            {
                List<BlogTag> tagList = new List<BlogTag>();
                string[] tags = blog.BlogTags.Split(',');
                foreach (string tagId in tags)
                {
                    BlogTag blogTag = _blogTagService.QueryEntityByID(Convert.ToInt32(tagId));
                    if (blogTag != null)
                    {
                        tagList.Add(blogTag);
                    }
                }
                ViewBag.BlogTag = tagList;
            }
            return View(blog);
        }
        [HttpPost]
        public ActionResult LoadNewArticle()
        {
            var blogList = this._blogService.QueryEntities(b => true);
            ViewBag.BlogList = blogList;
            return PartialView("newarticle");
        }
    }
}