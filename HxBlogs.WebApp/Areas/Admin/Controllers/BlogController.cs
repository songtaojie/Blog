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
           List<Blog> blogList =  this._blogService.GetEntitiesNoTrack(b => true, false).ToList();
            return View(blogList);
        }
    }
}