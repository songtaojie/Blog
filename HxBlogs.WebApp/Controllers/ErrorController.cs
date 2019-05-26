using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HxBlogs.WebApp.Controllers
{
    public class ErrorController : BaseNoAuthController
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
            Models.ExceptionMessage message = new Models.ExceptionMessage();
            if (!string.IsNullOrEmpty(data))
            {
                try
                {
                    data = HttpUtility.UrlDecode(data);
                    message = JsonConvert.DeserializeObject<Models.ExceptionMessage>(data);
                }
                catch {

                }
            }
            return View(message);
        }
        public ActionResult Error401(string errorUrl)
        {
            ViewData["errorUrl"] = errorUrl;
            return View();
        }
    }
}