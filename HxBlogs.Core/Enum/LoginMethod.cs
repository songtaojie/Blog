using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Core.Enum
{
    /// <summary>
    /// 登录系统的方式
    /// </summary>
    public enum LoginMethod
    {
        /// <summary>
        /// 使用用户名密码的方式登录
        /// </summary>
        Password = 0,
        /// <summary>
        /// 使用Session的方式登录
        /// </summary>
        Session = 1,
        /// <summary>
        /// 使用第三方应用登录QQ
        /// </summary>
        QQ = 2,
        /// <summary>
        /// 使用第三方应用登录新浪
        /// </summary>
        Sina = 3
    }
}
