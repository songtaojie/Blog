using Hx.Common.Helper;
using HxBlogs.IBLL;
using HxBlogs.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StackExchange.Profiling;

namespace HxBlogs.WebApp.Controllers
{
    [RoutePrefix("article")]
    [Route("{action}")]
    public class ViewController : BaseNoAuthController
    {
        private IUserService _userService;
        private IBlogService _blogService;
        private IBlogTagService _blogTagService;
        public ViewController(IUserService userService, IBlogService blogService, IBlogTagService blogTagServer)
        {
            this._userService = userService;
            this._blogService = blogService;
            this._blogTagService = blogTagServer;
        }
        [Route("{username}/{blogId}")]
        public ActionResult CkView(string username, string blogId)
        {
            //查询是否存在当前用户
            if (!string.IsNullOrEmpty(username))
                username = username.Trim();
            User user = _userService.GetEntity(u => u.UserName == username);
            Blog blog = _blogService.GetEntityByID(Convert.ToInt64(Helper.FromHex(blogId)));
            if (user == null || blog == null || blog.UserId != user.Id) throw new NotFoundException("找不到您访问的页面!");
            ViewBag.User = user;
            if (!string.IsNullOrEmpty(blog.BlogTags))
            {
                string tags = blog.BlogTags;
                List<BlogTag> tagList = _blogTagService.GetEntities(b => tags.Contains(b.Id + "")).ToList();
                ViewBag.BlogTag = tagList;
            }
            if (blog.IsMarkDown)
            {
                return View("mdview", blog);
            }
            return View(blog);
        }



        #region 加载侧边栏
        /// <summary>
        /// 加载最新文章
        /// </summary>
        /// <param name="id">当前页面的博客id，侧边栏排除掉</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{username}/side/loadarticle/{hexId?}")]
        public ActionResult LoadSideArticle(string username, string hexId)
        {
            User user = GetUser(username);
            IEnumerable<Blog> blogList = null;
            if (string.IsNullOrEmpty(hexId))
            {
                blogList = this._blogService.GetEntities(b => b.UserId == user.Id)
                    .OrderByDescending(b => b.PublishDate)
                    .Take(5);
            }
            else
            {
                long blogId = Convert.ToInt64(Helper.FromHex(hexId));
                blogList = this._blogService.GetEntities(b => b.UserId == user.Id && b.Id != blogId)
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
        [HttpGet]
        [Route("{username}/side/loadtag")]
        public ActionResult LoadSideTag(string username)
        {
            User user = GetUser(username);
            List<BlogTag> result = this._blogTagService.GetEntities(t => t.UserId == user.Id)
                .OrderBy(t => t.Order)
                .ToList();
            Dictionary<long, int> tagCountList = new Dictionary<long, int>();
            foreach (BlogTag tag in result)
            {
                int count = this._blogService.Count(b => b.UserId == user.Id && b.BlogTags.Contains(tag.Id.ToString()));
                tagCountList.Add(tag.Id, count);
            }
            ViewBag.TagCountList = tagCountList;
            ViewBag.UserName = username;
            return PartialView("sidetag", result);
        }

        /// <summary>
        /// 加载随笔归档
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{username}/side/loadarchive")]
        public ActionResult LoadSideArchive(string username)
        {
            User user = GetUser(username);
            IEnumerable<Blog> blogList = this._blogService.GetEntities(b => b.UserId == user.Id);

            var archiveList = blogList.GroupBy(b => b.CreateTime.ToString("yyyyMM"))
                .Select(g => new { Key = g.Key, Count = g.Count() })
                .OrderByDescending(b => b.Key);
            Dictionary<string, int> result = new Dictionary<string, int>();
            foreach (var item in archiveList)
            {
                result.Add(item.Key, item.Count);
            }
            ViewBag.ArchiveList = result;
            ViewBag.UserName = username;
            return PartialView("sidearchive");
        }
        #endregion

        #region 加载内容
        /// <summary>
        /// 随笔归档
        /// </summary>
        /// <param name="username"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        [Route("{username}/archive/{year:int}/{month:int}")]
        public ActionResult Archive(string username, int year, int month)
        {
            User user = GetUser(username);
            ViewBag.User = user;
            ViewBag.Year = year;
            ViewBag.Month = month;
            return View();
        }

        [Route("{username}/archivearticle/{year:int}/{month:int}")]
        public ActionResult LoadArticle(string username, int year, int month)
        {
            User user = GetUser(username);
            IEnumerable<Blog> blogList = this._blogService.GetEntities(b => b.UserId == user.Id && b.CreateTime.Year == year && b.CreateTime.Month == month)
                .OrderByDescending(b => b.PersonTop)
                .ThenByDescending(b => b.PublishDate);
            ViewBag.User = user;
            return PartialView("article", blogList.ToList());
        }
        #endregion

        #region 置顶和删除操作
        [HttpPost]
        public ActionResult TopBlog(string id)
        {
            long blogId = Convert.ToInt64(Helper.FromHex(id));
            AjaxResult result = new AjaxResult();
            Blog blog = this._blogService.GetEntityByID(blogId);
            if (blog == null)
            {
                result.Success = false;
                result.Message = "当前数据已丢失或已删除!";
            }
            else
            {
                if (blog.IsPersonTop)
                {
                    result.Resultdata = "当前数据取消置顶成功!";
                    blog.PersonTop = "N";
                }
                else
                {
                    result.Resultdata = "当前数据置顶成功!";
                    blog.PersonTop = "Y";
                }
                this._blogService.Update(blog);
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult DeleteBlog(string id)
        {
            long blogId = Convert.ToInt64(Helper.FromHex(id));
            AjaxResult result = new AjaxResult();
            Blog blog = this._blogService.GetEntityByID(blogId);
            if (blog == null)
            {
                result.Success = false;
                result.Message = "当前数据已丢失或已删除!";
            }
            else
            {
                result.Resultdata = "数据删除成功!";
                this._blogService.LogicDelete(blog);
            }
            return Json(result);
        }
        #endregion

        #region 随笔分类
        /// <summary>
        /// 随笔分类
        /// </summary>
        [Route("{username}/tag/{hexId}")]
        public ActionResult EssayTag(string username, string hexId)
        {
            User user = GetUser(username);
            long tagId = Convert.ToInt64(Helper.FromHex(hexId));
            BlogTag tag = _blogTagService.GetEntity(t => t.Id == tagId && t.UserId == user.Id);
            ViewBag.TagName = tag == null ? "" : tag.Name;
            ViewBag.User = user;
            ViewBag.TagId = hexId;
            return View();
        }


        [Route("{username}/tagarticle/{hexId}")]
        public ActionResult LoadArticle(string username, string hexId)
        {
            User user = GetUser(username);
            string tagId = Helper.FromHex(hexId);
            IEnumerable<Blog> blogList = this._blogService.GetEntities(b => b.UserId == user.Id && b.BlogTags.Contains(tagId))
                .OrderByDescending(b => b.PersonTop)
                .ThenByDescending(b => b.PublishDate);
            ViewBag.User = user;
            return PartialView("article", blogList.ToList());
        }
        #endregion

        #region 加载全部文章
        [Route("~/{username}")]
        public ActionResult AllArticle(string username)
        {
            User user = GetUser(username);
            ViewBag.User = user;
            return View();
        }
        [Route("{username}/allarticle")]
        public ActionResult LoadArticle(string username)
        {
            User user = GetUser(username);
            IEnumerable<Blog> blogList = this._blogService.GetEntities(b => b.UserId == user.Id)
                .OrderByDescending(b => b.PersonTop)
                .ThenByDescending(b => b.PublishDate);
            ViewBag.User = user;
            return PartialView("article", blogList.ToList());
        }
        #endregion

        public User GetUser(string username)
        {
            //查询是否存在当前用户
            User user = null;
            if (!string.IsNullOrEmpty(username))
            {
                username = username.Trim();
                user = _userService.GetEntity(u => u.UserName == username);
            }
            if (user == null) throw new NotFoundException("找不到您访问的页面!");
            return user;
        }
    }
}