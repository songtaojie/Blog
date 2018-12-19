using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HxBlogs.WebApp.Controllers
{
    public class FileController : BaseController
    {
        // GET: File
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Upload()
        {
            HttpPostedFileBase file = Request.Files["upload"];
            string rootPath = WebHelper.GetAppSetValue("uploadRootPath");

            return Json(new
            {
                uploaded = true,
                url = "/upload/360wallpaper2015110162333.jpg"
            });
        }
    }
}