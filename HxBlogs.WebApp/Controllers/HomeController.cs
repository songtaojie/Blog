using Hx.Common.Helper;
using Hx.Framework;
using HxBlogs.IBLL;
using HxBlogs.Model;
using HxBlogs.WebApp.Filters;
using HxBlogs.WebApp.Models;
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
            List<BlogViewModel> blogs = _blogService.GetEntitiesNoTrack(b => b.Carousel == "Y")
                     .Select(b => new BlogViewModel()
                     {
                         Id = b.Id,
                         Title = b.Title,
                         ImgUrl = b.ImgUrl,
                     }).ToList();
            foreach (BlogViewModel item in blogs)
            {
                item.ImgUrl = WebHelper.GetRandomImgUrl(item.ImgUrl);
            }
            blogs.Add(new BlogViewModel { ImgUrl = WebHelper.GetCarousel("v/1.jpg") });
            blogs.Add(new BlogViewModel { ImgUrl = WebHelper.GetCarousel("v/2.jpg") });
            blogs.Add(new BlogViewModel { ImgUrl = WebHelper.GetCarousel("v/3.jpg") });
            blogs.Add(new BlogViewModel { ImgUrl = WebHelper.GetCarousel("v/4.jpg") });

            blogs.Add(new BlogViewModel { ImgUrl = WebHelper.GetCarousel("h/h1.jpg") });
            blogs.Add(new BlogViewModel { ImgUrl = WebHelper.GetCarousel("h/h2.jpg") });
            return View(blogs);
        }
        /// <summary>
        /// 加载文章
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadArticle()
        {
            var profiler = MiniProfiler.Current;
            using (profiler.Step("文章"))
            {
                IEnumerable<BlogViewModel> blogs = _blogService.GetEntitiesNoTrack(b => true)
                    .Select(b => new BlogViewModel()
                    {
                        Id = b.Id,
                        Title = b.Title,
                        ContentHtml = b.ContentHtml,
                        CmtCount = b.CmtCount,
                        ReadCount = b.ReadCount,
                        PublishDate = b.PublishDate,
                        UserId = b.UserId,
                        User = b.User,
                        UserName = b.UserName
                    });
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