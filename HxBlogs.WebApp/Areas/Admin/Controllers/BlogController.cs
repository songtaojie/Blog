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
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList()
        {
            int draw = Convert.ToInt32(Request["draw"]);
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string search = Request["search[value]"];
            //没有过滤的记录数
            long recordsTotal = 0;

            IQueryable<Blog> blogs;
            if (string.IsNullOrEmpty(search))
            {
                recordsTotal = this._blogService.LongCount(b => true, false);
                blogs = this._blogService.GetEntitiesNoTrack(b => true, false)
                .OrderByDescending(b => b.PublishDate);
            }
            else
            {
                recordsTotal = this._blogService.LongCount(b => b.UserName.Contains(search) || b.Title.Contains(search), false);
                blogs = this._blogService.GetEntitiesNoTrack(b => b.UserName.Contains(search) || b.Title.Contains(search), false)
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
                    b.CreateTime
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
            //过滤后的记录数
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}