using Common.Cache;
using Common.Logs;
using HxBlogs.Framework;
using HxBlogs.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public UserInfo LoginUser
        {
            get;set;
        }
        /// <summary>
        /// 在执行控制器的方法前，先执行该方法，可以用来进行校验
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            bool isSuccess = false;
            string pageUrl = filterContext.HttpContext.Request.Url.LocalPath;
            if (Request.Cookies[CookieInfo.SessionID] != null)
            {
                string sessionId = Request.Cookies[CookieInfo.SessionID].Value;
                object value = MemcachedHelper.Get(sessionId);
                if (value != null)
                {
                    isSuccess = true;
                    string jsonData = value.ToString();
                    UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(jsonData);
                    UserContext.LoginUser = userInfo;
                    //模拟滑动过期时间，就像Session中默认20分钟那这样
                    MemcachedHelper.Set(sessionId, value, DateTime.Now.AddMinutes(20));
                }
            }
            if (!isSuccess)
            {
                filterContext.Result = Redirect("/login?ReturnUrl="+pageUrl);
            }
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