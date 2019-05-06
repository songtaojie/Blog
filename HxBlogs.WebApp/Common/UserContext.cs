using Hx.Common.Cache;
using Hx.Common.Helper;
using Hx.Framework;
using HxBlogs.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HxBlogs.WebApp
{
    public static class UserContext
    {
        private static HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }
        /// <summary>
        /// 当前登录用户
        /// </summary>
        public static UserInfo LoginUser
        {
            get
            {
                UserInfo userInfo = HttpContext.Current.Items[ConstInfo.LoginUser] as UserInfo;
                if (userInfo == null)
                {
                    userInfo = ValidateSession();
                    if (userInfo == null)
                    {
                        userInfo = ValidateCookie();
                    }
                }
                return userInfo;
            }
            set
            {
                HttpContext.Current.Items[ConstInfo.LoginUser] = value;
            }
        }
        /// <summary>
        /// 验证Session中(即Memcached中)是否有数据
        /// </summary>
        /// <returns></returns>
        public static UserInfo ValidateSession()
        {
            UserInfo userInfo = null;
            string sessionId = WebHelper.GetCookieValue(ConstInfo.SessionID);
            if (!string.IsNullOrEmpty(sessionId))
            {
                object value = MemcachedHelper.Get(sessionId);
                if (value != null)
                {
                    string jsonData = value.ToString();
                    userInfo = JsonConvert.DeserializeObject<UserInfo>(jsonData);
                    UserContext.LoginUser = userInfo;
                    //模拟滑动过期时间，就像Session中默认20分钟那这样
                    MemcachedHelper.Set(sessionId, value, DateTime.Now.AddHours(2));
                }
            }
            return userInfo;
        }
        /// <summary>
        /// 验证Cookie中是否有数据(正常保存7天)
        /// </summary>
        /// <returns></returns>
        public static UserInfo ValidateCookie()
        {
            UserInfo userInfo = null;
            string cookieName = WebHelper.GetCookieValue(ConstInfo.CookieName);
            if (!string.IsNullOrEmpty(cookieName))
            {
                string jsonUser = Hx.Common.Security.SafeHelper.DESDecrypt(cookieName);
                Dictionary<string, string> user = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonUser);
                if (user.ContainsKey(nameof(UserInfo.UserName)) && user.ContainsKey(nameof(UserInfo.PassWord)))
                {
                    IBLL.IUserInfoService userService = ContainerManager.Resolve<IBLL.IUserInfoService>();
                    string userName = user[nameof(UserInfo.UserName)];
                    string pwd = user[nameof(UserInfo.PassWord)];
                    userInfo = userService.QueryEntity(u => u.UserName == userName && u.PassWord == pwd);
                    if (userInfo != null)
                    {
                        UserContext.LoginUser = userInfo;
                        UserContext.CacheUserInfo(userInfo);
                    }
                }
            }
            return userInfo;
        }

        /// <summary>
        /// 把用户信息存储在Memcached缓存中
        /// </summary>
        public static void CacheUserInfo(UserInfo userInfo, bool isRemember = false)
        {
            if (userInfo == null) return;
            //模拟Session
            string sessionId = Guid.NewGuid().ToString();
            WebHelper.SetCookieValue(ConstInfo.SessionID, sessionId.ToString(), DateTime.Now.AddHours(2));
            string jsonData = JsonConvert.SerializeObject(userInfo);
            MemcachedHelper.Set(sessionId, jsonData, DateTime.Now.AddHours(2));
            //记住用户名,添加7天的缓存
            if (isRemember)
            {
                Dictionary<string, string> user = new Dictionary<string, string>();
                user.Add(nameof(userInfo.UserName), userInfo.UserName);
                user.Add(nameof(userInfo.PassWord), userInfo.PassWord);
                string jsonUser = JsonConvert.SerializeObject(user);
                string desUser = Hx.Common.Security.SafeHelper.DESEncrypt(jsonUser);
                WebHelper.SetCookieValue(ConstInfo.CookieName, desUser, DateTime.Now.AddDays(7));
            }
        }
        public static void ClearUserInfo()
        {
            UserContext.LoginUser = null;
            if (Request.Cookies[ConstInfo.SessionID] != null)
            {
                string sessionId = Request.Cookies[ConstInfo.SessionID].Value;
                MemcachedHelper.Delete(sessionId);
            }
            WebHelper.RemoveCookie(ConstInfo.SessionID);
            WebHelper.RemoveCookie(ConstInfo.CookieName);
        }
    }
}