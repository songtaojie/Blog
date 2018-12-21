using Common.Cache;
using Common.Helper;
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
                    if (Request.Cookies[ConstInfo.SessionID] != null)
                    {
                        string sessionId = Request.Cookies[ConstInfo.SessionID].Value;
                        object value = MemcachedHelper.Get(sessionId);
                        if (value != null)
                        {
                            string jsonData = value.ToString();
                            userInfo = JsonConvert.DeserializeObject<UserInfo>(jsonData);
                            HttpContext.Current.Items[ConstInfo.LoginUser] = userInfo;
                        }
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
                string desUser = Common.Security.SafeHelper.DESEncrypt(jsonUser);
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