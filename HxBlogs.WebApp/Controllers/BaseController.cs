using Hx.Common.Cache;
using Hx.Common.Logs;
using Hx.Framework;
using HxBlogs.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace HxBlogs.WebApp.Controllers
{
    public class BaseController : Controller
    {
        public ILogger Logger
        {
            get
            {
                return ContainerManager.Resolve<ILogger>();
            }
        }
        /// <summary>
        /// 在执行控制器的方法前，先执行该方法，可以用来进行校验
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            bool isSuccess = AllowAnonymous(filterContext);
            if (!isSuccess)
            {
                User userInfo = UserContext.ValidateSession();
                isSuccess = userInfo != null;
                if (!isSuccess)
                {
                    userInfo = UserContext.ValidateCookie();
                    isSuccess = userInfo != null;
                }
            }
            if (!isSuccess)
            {
                string pageUrl = filterContext.HttpContext.Request.Url.LocalPath;
                filterContext.Result = Redirect(string.Format("/login?{0}={1}", ConstInfo.returnUrl, pageUrl));
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
        }


        private bool AllowAnonymous(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.Url == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            // 1、允许匿名访问 用于标记在授权期间要跳过 AuthorizeAttribute 的控制器和操作的特性 
            var actionAnonymous = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true) as IEnumerable<AllowAnonymousAttribute>;
            var controllerAnonymous = filterContext.Controller.GetType().GetCustomAttributes(typeof(AllowAnonymousAttribute), true) as IEnumerable<AllowAnonymousAttribute>;
            if ((actionAnonymous != null && actionAnonymous.Any()) || (controllerAnonymous != null && controllerAnonymous.Any()))
            {
                return true;
            }
            return false;
        }

        protected T FillAddModel<T>(T model) where T : BaseEntity
        {
            User userInfo = UserContext.LoginUser;
            if (userInfo != null)
            {
                model.UserId = userInfo.Id;
                model.UserName = userInfo.UserName;
            }
            return model;
        }
        protected T FillDeleteModel<T>(T model) where T : BaseEntity
        {
            User userInfo = UserContext.LoginUser;
            if (userInfo != null)
            {
                model.DeleteId = userInfo.Id;
            }
            return model;
        }
    }
}