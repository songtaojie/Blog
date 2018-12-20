using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HxBlogs.WebApp
{
    public class ConstInfo
    {
        public const string CookieName = "mUSQAcsBu";

        public const string CacheKeyCookieName = "0uM9eFqf";

        public const string DomainName = "tonyblogs.top";
        /// <summary>
        /// 用来模拟session的标志
        /// </summary>
        public const string SessionID = "sessionId";
        /// <summary>
        /// 验证码存储在Session中的标志
        /// </summary>
        public const string VCode = "validateCode";
        public const string LoginUser = "loginUser";
        /// <summary>
        /// 文件上传时根目录
        /// </summary>
        public const string UploadPath = "uploadRootPath";
    }
}