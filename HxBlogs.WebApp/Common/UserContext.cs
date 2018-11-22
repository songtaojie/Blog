using HxBlogs.Model;
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
                return HttpContext.Current.Items[CookieInfo.LoginUser] as UserInfo;
            }
            set
            {
                HttpContext.Current.Items[CookieInfo.LoginUser] = value;
            }
        }
    }
}