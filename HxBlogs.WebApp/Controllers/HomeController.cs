using Hx.Common.Helper;
using Hx.Framework;
using HxBlogs.IBLL;
using HxBlogs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HxBlogs.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private IBlogService _blogService;
        public HomeController(IBlogService blogService)
        {
            this._blogService = blogService;
        }
        public ActionResult Index()
        {
            IEnumerable<Blog> blogs = _blogService.QueryEntities(m => m.IsPublish == "Y" && m.IsPrivate == "N")
                 .OrderByDescending<Blog, decimal?>(m => m.OrderFactor);
                
            return View(blogs.ToList());
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