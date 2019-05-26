using Hx.Common.Helper;
using HxBlogs.IBLL;
using HxBlogs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HxBlogs.WebApp.Controllers
{
    [RoutePrefix("article")]
    [Route("{action}")]
    public class ViewController : BaseNoAuthController
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
        [Route("{username}/{blogId}")]
        public ActionResult Index(string username, string blogId)
        {
            //查询是否存在当前用户
            if (!string.IsNullOrEmpty(username))
                username = username.Trim();
            User user = _userService.QueryEntity(u=>u.UserName == username);
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
        [Route("{username}/side/loadarticle")]
        public ActionResult LoadSideArticle(string username,string id)
        {
            //查询是否存在当前用户
            if (!string.IsNullOrEmpty(username))
                username = username.Trim();
            User user = _userService.QueryEntity(u => u.UserName == username);
            if (user == null) throw new NotFoundException("找不到您访问的页面!");
            IEnumerable<Blog> blogList = null;
            if (string.IsNullOrEmpty(id))
            {
                blogList = this._blogService.QueryEntities(b => b.UserId == user.Id)
                    .OrderByDescending(b => b.PublishDate)
                    .Take(5);
            }
            else
            {
                int blogId = Convert.ToInt32(Helper.FromHex(id));
                blogList = this._blogService.QueryEntities(b => b.UserId == user.Id && b.Id != blogId)
                    .OrderByDescending(b => b.PublishDate)
                    .Take(5);
            }
            ViewBag.BlogList = blogList;
            ViewBag.UserName = username;
            return PartialView("sidearticle");
        }

        /// <summary>
        /// 加载随笔分类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("{username}/side/loadtag")]
        public async Task<ActionResult> LoadSideTag(string username)
        {
            //查询是否存在当前用户
            if (!string.IsNullOrEmpty(username))
                username = username.Trim();
            User user = _userService.QueryEntity(u => u.UserName == username);
            if (user == null) throw new NotFoundException("找不到您访问的页面!");
            var result = await this._blogTagService.QueryEntitiesAsync(t => t.UserId == user.Id);
            Dictionary<int, int> tagCountList = new Dictionary<int, int>();
            var tagList = result.OrderBy(t => t.Order);
            foreach (BlogTag tag in tagList)
            {
                int count = this._blogService.Count(b =>b.UserId==user.Id && b.BlogTags.Contains(tag.Id.ToString()));
                tagCountList.Add(tag.Id, count);
            }
            ViewBag.TagCountList = tagCountList;
            ViewBag.UserName = username;
            return PartialView("sidetag", tagList);
        }

        /// <summary>
        /// 加载随笔归档
        /// </summary>
        /// <returns></returns>
        
        [HttpPost]
        [Route("{username}/side/loadarchive")]
        public ActionResult LoadSideArchive(string username)
        {
            //查询是否存在当前用户
            if (!string.IsNullOrEmpty(username))
                username = username.Trim();
            User user = _userService.QueryEntity(u => u.UserName == username);
            if (user == null) throw new NotFoundException("找不到您访问的页面!");
            var archiveList = this._blogService.QueryEntities(b => b.UserId == user.Id && b.IsPublish=="Y")
                .GroupBy(b => b.CreateTime.ToString("yyyyMM"))
                .Select(g => new { Key = g.Key,Count = g.Count()})
                .OrderByDescending(b=>b.Key);
            Dictionary<string, int> result = new Dictionary<string, int>();
            foreach (var item in archiveList)
            {
                result.Add(item.Key, item.Count);
            }
            ViewBag.ArchiveList = result;
            ViewBag.UserName = username;
            return PartialView("sidearchive");
        }
        [Route("{username}/archive/{year:int}/{month:int}")]
        public ActionResult Archive(string username,int year,int month)
        {
            //查询是否存在当前用户
            if (!string.IsNullOrEmpty(username))
                username = username.Trim();
            User user = _userService.QueryEntity(u => u.UserName == username);
            if (user == null) throw new NotFoundException("找不到您访问的页面!");
            ViewBag.User = user;
            ViewBag.Year = year;
            ViewBag.Month = month;
            return View();
        }
    }
}