using HxBlogs.Model;
using HxBlogs.WebApp.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HxBlogs.WebApp.Controllers
{
    [AllowAnonymous]
    public class BaseNoAuthController : Controller
    {
    }
}