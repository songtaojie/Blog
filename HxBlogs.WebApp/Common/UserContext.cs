using Common.Cache;
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
                    if (HttpContext.Current.Request.Cookies[ConstInfo.SessionID] != null)
                    {
                        string sessionId = HttpContext.Current.Request.Cookies[ConstInfo.SessionID].Value;
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
    }
}