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
        /// <summary>
        /// 加载最新文章
        /// </summary>
        /// <param name="id">当前页面的博客id，侧边栏排除掉</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadNewArticle(string id)
        {
            int blogId = Convert.ToInt32(Helper.FromHex(id));
            var blogList = this._blogService.QueryEntities(b => b.Id != blogId).OrderByDescending(b=>b.PublishDate).Take(5);
            ViewBag.BlogList = blogList;
            return PartialView("newarticle");
        }

        /// <summary>
        /// 加载个人标签
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadPersonTag()
        {
            var tagList = this._blogTagService.QueryEntities(t => true).OrderBy(t => t.Order).Take(5);
            Dictionary<int, int> tagCountList = new Dictionary<int, int>();
            foreach (BlogTag tag in tagList)
            {
                int count = this._blogService.Count(b => b.BlogTags.Contains(tag.Id.ToString()));
                tagCountList.Add(tag.Id, count);
            }
            ViewBag.TagList = tagList;
            ViewBag.TagCountList = tagCountList;
            return PartialView("persontag");
        }

        /// <summary>
        /// 加载个人标签
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadArchive()
        {
            var archiveList = this._blogService.QueryEntities(b => b.UserId == UserContext.LoginUser.Id && b.IsPublish=="Y" && b.IsDeleted=="N")
                .GroupBy(b => b.CreateTime.ToString("yyyyMM"))
                .Select(g => new { Key = g.Key,Count = g.Count()})
                .OrderByDescending(b=>b.Key);
            Dictionary<string, int> result = new Dictionary<string, int>();
            foreach (var item in archiveList)
            {
                result.Add(item.Key, item.Count);
            }
            ViewBag.ArchiveList = result;
            return PartialView("archive");
        }
    }
}