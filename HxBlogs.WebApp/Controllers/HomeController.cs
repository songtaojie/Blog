using Hx.Common.Helper;
using Hx.Framework;
using HxBlogs.IBLL;
using HxBlogs.Model;
using HxBlogs.WebApp.Filters;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HxBlogs.WebApp.Controllers
{
    public class HomeController : BaseNoAuthController
    {
        private IBlogService _blogService;
        public HomeController(IBlogService blogService)
        {
            this._blogService = blogService;
        }
        public ActionResult Index()
        {
            var profiler = StackExchange.Profiling.MiniProfiler.Current;
            using (profiler.Step("主页"))
            {
                ICategoryService cateService = ContainerManager.Resolve<ICategoryService>();
                IEnumerable<Category> cateList = cateService.QueryEntities(c => true)
                    .OrderByDescending(c => c.Order);
                ViewBag.CategoryList = cateList;
            }
            return View();
        }

        public ActionResult LoadArticle()
        {
            var profiler = MiniProfiler.Current;
            using (profiler.Step("文章"))
            {
                IEnumerable<Blog> blogs = _blogService.QueryEntities(b => true)
                 .OrderByDescending<Blog, decimal?>(m => m.OrderFactor);
                return PartialView("article", blogs.ToList());
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}