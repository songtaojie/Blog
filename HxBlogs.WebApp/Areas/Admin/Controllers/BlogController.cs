using HxBlogs.IBLL;
using HxBlogs.Model;
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
            string search = Request["search"];
            //没有过滤的记录数
            long recordsTotal = this._blogService.LongCount(b => true, false);
            if (string.IsNullOrEmpty(search)) {

            }
            var data = this._blogService.GetEntitiesNoTrack(b => true, false)
                .OrderByDescending(b=>b.PublishDate)
                .Skip(start * length)
                .Take(length)
                .ToList();
            //过滤后的记录数
            int recordsFiltered = data.Count;
            return Json(new
            {
                draw,
                recordsTotal,
                recordsFiltered,
                data
            }, JsonRequestBehavior.AllowGet);
        }
    }
}