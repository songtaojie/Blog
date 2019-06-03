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
    /// <summary>
    /// 用户信息相关的一些方法
    /// </summary>
    public static class UserContext
    {
        private static HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }
        /// <summary>
        /// 当前登录用户
        /// </summary>
        public static User LoginUser
        {
            get
            {
                User userInfo = HttpContext.Current.Items[ConstInfo.LoginUser] as User;
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
        public static User ValidateSession()
        {
            User userInfo = null;
            string sessionId = WebHelper.GetCookieValue(ConstInfo.SessionID);
            if (!string.IsNullOrEmpty(sessionId))
            {
                object value = MemcachedHelper.Get(sessionId);
                if (value != null)
                {
                    string jsonData = value.ToString();
                    userInfo = JsonConvert.DeserializeObject<User>(jsonData);
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
        public static User ValidateCookie()
        {
            User userInfo = null;
            string cookieName = WebHelper.GetCookieValue(ConstInfo.CookieName);
            if (!string.IsNullOrEmpty(cookieName))
            {
                string jsonUser = Hx.Common.Security.SafeHelper.DESDecrypt(cookieName);
                Dictionary<string, string> user = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonUser);
                if (user.ContainsKey(nameof(User.UserName)) && user.ContainsKey(nameof(User.PassWord)))
                {
                    IBLL.IUserService userService = ContainerManager.Resolve<IBLL.IUserService>();
                    string userName = user[nameof(User.UserName)];
                    string pwd = user[nameof(User.PassWord)];
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
        public static void CacheUserInfo(User userInfo, bool isRemember = false)
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
        /// <summary>
        /// 判断给定的用户是否是登录用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsLoginUser(User user)
        {
            User loginUser = LoginUser;
            if (user == null || loginUser == null) return false;
            return user.Id == loginUser.Id;
        }
        /// <summary>
        /// 判断给定的用户是否是登录用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsLoginUser(int userId)
        {
            User loginUser = LoginUser;
            if (userId<0 || loginUser == null) return false;
            return userId == loginUser.Id;
        }
        /// <summary>
        /// 获取用户用于展示的用户名
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetDisplayName(User user)
        {
            if (user == null) return "";
            if (!string.IsNullOrWhiteSpace(user.NickName)) return user.NickName;
            return user.UserName;
        }
    }
}