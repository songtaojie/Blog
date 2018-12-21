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
            get; set;
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
            isSuccess = ValidateSession();
            if (!isSuccess)
            {
                isSuccess = ValidateCookie();
            }
            if (!isSuccess)
            {
                filterContext.Result = Redirect("/login?ReturnUrl=" + pageUrl);
            }
        }
        /// <summary>
        /// 验证Session中(即Memcached中)是否有数据
        /// </summary>
        /// <returns></returns>
        private bool ValidateSession()
        {
            bool isSuccess = false;
            string sessionId = WebHelper.GetCookieValue(ConstInfo.SessionID);
            if (!string.IsNullOrEmpty(sessionId))
            {
                object value = MemcachedHelper.Get(sessionId);
                if (value != null)
                {
                    isSuccess = true;
                    string jsonData = value.ToString();
                    UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(jsonData);
                    UserContext.LoginUser = userInfo;
                    //模拟滑动过期时间，就像Session中默认20分钟那这样
                    MemcachedHelper.Set(sessionId, value, DateTime.Now.AddHours(2));
                }
            }
            return isSuccess;
        }
        /// <summary>
        /// 验证Cookie中是否有数据(正常保存7天)
        /// </summary>
        /// <returns></returns>
        private bool ValidateCookie()
        {
            bool isSuccess = false;
            string cookieName = WebHelper.GetCookieValue(ConstInfo.CookieName);
            if (!string.IsNullOrEmpty(cookieName))
            {
                string jsonUser = Common.Security.SafeHelper.DESDecrypt(cookieName);
                Dictionary<string, string> user = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonUser);
                if (user.ContainsKey(nameof(UserInfo.UserName)) && user.ContainsKey(nameof(UserInfo.PassWord)))
                {
                    IBLL.IUserInfoService userService = ContainerManager.Resolve<IBLL.IUserInfoService>();
                    string userName = user[nameof(UserInfo.UserName)];
                    string pwd = user[nameof(UserInfo.PassWord)];
                    UserInfo userInfo = userService.QueryEntity(u => u.UserName == userName && u.PassWord == pwd);
                    if (userInfo != null)
                    {
                        isSuccess = true;
                        UserContext.LoginUser = userInfo;
                        UserContext.CacheUserInfo(userInfo);
                    }
                }
            }
            return isSuccess;
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