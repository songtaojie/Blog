using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HxBlogs.WebApp.Models
{
    /// <summary>
    /// 用户友好异常https://www.jb51.net/article/142961.htm
    /// </summary>
    public class UserFriendlyException : Exception
    {
        public UserFriendlyException(string message)
        : base(message)
        {
        }
    }

    /// <summary>
    /// 没有被授权的异常
    /// </summary>
    public class NoAuthorizeException : Exception
    {
        public NoAuthorizeException(string message)
        : base(message)
        {
        }
    }

    // <summary>
    /// 表示Ajax操作结果
    /// </summary>
    public class AjaxResult
    {
        /// <summary>
        /// 获取 Ajax操作结果类型
        /// </summary>
        public ResultType type { get; set; }

        /// <summary>
        /// 获取 Ajax操作结果编码
        /// </summary>
        public int errorcode { get; set; }

        /// <summary>
        /// 获取 消息内容
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 获取 返回数据
        /// </summary>
        public object resultdata { get; set; }
    }
    /// <summary>
    /// 表示 ajax 操作结果类型的枚举
    /// </summary>
    public enum ResultType
    {
        /// <summary>
        /// 消息结果类型
        /// </summary>
        info = 0,

        /// <summary>
        /// 成功结果类型
        /// </summary>
        success = 1,

        /// <summary>
        /// 警告结果类型
        /// </summary>
        warning = 2,

        /// <summary>
        /// 异常结果类型
        /// </summary>
        error = 3
    }
}