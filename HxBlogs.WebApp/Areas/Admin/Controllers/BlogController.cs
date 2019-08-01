using Hx.Common.Helper;
using HxBlogs.IBLL;
using HxBlogs.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HxBlogs.WebApp.Areas.Admin.Controllers
{
    [AllowAdmin]
    public class BlogController : Controller
    {
        private IBlogService _blogService;
        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }
        #region 博客加载
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadList()
        {
            string result = LoadBlog(false);
            //过滤后的记录数
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 博客操作
        [HttpPost]
        public ActionResult DoDelete(string hexId)
        {
            AjaxResult result = new AjaxResult();
            long id = Convert.ToInt64(Helper.FromHex(hexId));
            Blog blog = this._blogService.GetEntityByID(id);
            if (blog == null)
            {
                result.Success = false;
                result.Message = "当前博客不存在!";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            this._blogService.LogicDelete(blog);
            result.Resultdata = "博客删除成功!";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DoTop(string hexId)
        {
            AjaxResult result = new AjaxResult();
            long id = Convert.ToInt64(Helper.FromHex(hexId));
            Blog blog = this._blogService.GetEntityByID(id);
            if (blog == null)
            {
                result.Success = false;
                result.Message = "当前博客不存在!";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            blog.Top = blog.IsTop ? "N" : "Y";
            this._blogService.UpdateEntityFields(blog, "Top");
            result.Resultdata = "博客"+ (blog.IsTop?"":"取消")+ "置顶成功!";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DoCarousel(string hexId)
        {
            AjaxResult result = new AjaxResult();
            long id = Convert.ToInt64(Helper.FromHex(hexId));
            Blog blog = this._blogService.GetEntityByID(id);
            if (blog == null)
            {
                result.Success = false;
                result.Message = "当前博客不存在!";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            blog.Carousel = blog.IsCarousel ? "N" : "Y";
            this._blogService.UpdateEntityFields(blog, "Carousel");
            result.Resultdata = "博客"+(blog.IsCarousel ? "" : "取消")+"设置轮播图成功!";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 回收站
        public ActionResult Recycle()
        {
            return View();
        }
        public ActionResult LoadRecycle()
        {
            string result = LoadBlog(true);
            //过滤后的记录数
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 回收站操作
        [HttpPost]
        public ActionResult DoRemove(string hexId)
        {
            AjaxResult result = new AjaxResult();
            long id = Convert.ToInt64(Helper.FromHex(hexId));
            Blog blog = this._blogService.GetEntityByID(id, false);
            if (blog == null)
            {
                result.Success = false;
                result.Message = "当前博客不存在!";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            this._blogService.PhysicalDelete(blog);
            result.Resultdata = "博客删除成功!";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DoRevert(string hexId)
        {
            AjaxResult result = new AjaxResult();
            long id = Convert.ToInt64(Helper.FromHex(hexId));
            Blog blog = this._blogService.GetEntityByID(id,false);
            if (blog == null)
            {
                result.Success = false;
                result.Message = "当前博客不存在!";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            blog.Delete = "N";
            this._blogService.UpdateEntityFields(blog, "Delete");
            result.Resultdata = "博客还原成功!";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        /// <summary>
        /// 加载博客
        /// </summary>
        /// <param name="delete">是否删除</param>
        /// <returns></returns>
        private string LoadBlog(bool delete)
        {
            string isDel = delete ? "Y" : "N";
            int draw = Convert.ToInt32(Request["draw"]);
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string search = Request["search[value]"];
            //没有过滤的记录数
            long recordsTotal = 0;

            IQueryable<Blog> blogs;
            if (string.IsNullOrEmpty(search))
            {
                recordsTotal = this._blogService.LongCount(b => b.Delete == isDel, false);
                blogs = this._blogService.GetEntitiesNoTrack(b => b.Delete == isDel, false)
                .OrderByDescending(b => b.PublishDate);
            }
            else
            {
                recordsTotal = this._blogService.LongCount(b =>
                    b.Delete == isDel &&
                    (b.UserName.Contains(search) ||
                    b.Title.Contains(search)), false);
                blogs = this._blogService.GetEntitiesNoTrack(b =>
                    b.Delete == isDel &&
                    (b.UserName.Contains(search) ||
                    b.Title.Contains(search)), false)
                .OrderByDescending(b => b.PublishDate);
            }
            var data = blogs.Skip(start * length)
                .Take(length)
                .Select(b => new
                {
                    b.Id,
                    b.Title,
                    b.Publish,
                    b.PublishDate,
                    b.UserName,
                    b.Top,
                    b.BlogType,
                    b.Category,
                    b.Carousel,
                    b.DeleteTime,
                })
                .ToList();
            int recordsFiltered = data.Count;
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
            timeConverter.DateTimeFormat = "yyyy-MM-dd HH:ss";
            string result = JsonConvert.SerializeObject(new
            {
                draw,
                recordsTotal,
                recordsFiltered,
                data
            }, Formatting.Indented, timeConverter);
            return result;
        }
    }
}