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
                UserInfo userInfo = UserContext.ValidateSession();
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
            UserInfo userInfo = UserContext.LoginUser;
            if (userInfo != null)
            {
                model.CreatorId = userInfo.Id;
                model.CreatorName = userInfo.NickName;
            }
            return model;
        }
        protected T FillDeleteModel<T>(T model) where T : BaseEntity
        {
            UserInfo userInfo = UserContext.LoginUser;
            if (userInfo != null)
            {
                model.DeleteId = userInfo.Id;
                model.DeleteName = userInfo.NickName;
            }
            return model;
        }
        protected T FillModifyModel<T>(T model) where T : BaseEntity
        {
            UserInfo userInfo = UserContext.LoginUser;
            if (userInfo != null)
            {
                model.LastModifiyId = userInfo.Id;
                model.LastModifiyName = userInfo.NickName;
            }
            return model;
        }
        /// <summary>
        /// 获取客户端ip
        /// </summary>
        /// <returns></returns>
        protected string GetClientIp()
        {
            string userIP = "未获取用户IP";

            try
            {
                if (System.Web.HttpContext.Current == null
                 || System.Web.HttpContext.Current.Request == null
                 || System.Web.HttpContext.Current.Request.ServerVariables == null)
                {
                    return "";
                }

                string CustomerIP = "";

                //CDN加速后取到的IP simone 090805
                CustomerIP = System.Web.HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
                if (!string.IsNullOrEmpty(CustomerIP))
                {
                    return CustomerIP;
                }

                CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (!String.IsNullOrEmpty(CustomerIP))
                {
                    return CustomerIP;
                }

                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                    if (CustomerIP == null)
                    {
                        CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    }
                }
                else
                {
                    CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                if (string.Compare(CustomerIP, "unknown", true) == 0 || String.IsNullOrEmpty(CustomerIP))
                {
                    return System.Web.HttpContext.Current.Request.UserHostAddress;
                }
                return CustomerIP;
            }
            catch { }

            return userIP;

        }
    }
}