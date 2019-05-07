using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HxBlogs.WebApp.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public ActionResult Error404(string errorUrl)
        {
            ViewData["errorUrl"] = errorUrl;
            return View();
        }
        public ActionResult Error500(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                try
                {
                    data = HttpUtility.UrlDecode(data);
                    Models.ExceptionMessage message = JsonConvert.DeserializeObject<Models.ExceptionMessage>(data);
                    ViewData["errorUrl"] = message.Path;
                }
                catch {

                }
            }
            return View();
        }
        public ActionResult Error401(string errorUrl)
        {
            ViewData["errorUrl"] = errorUrl;
            return View();
        }
    }
}