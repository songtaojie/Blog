using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Model
{
    public class AjaxResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; } = true;//默认是成功

        /// <summary>
        /// 信息描述
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 获取 Ajax操作结果编码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 获取 返回数据
        /// </summary>
        public object Resultdata { get; set; }

        ///// <summary>
        ///// 回调地址
        ///// </summary>
        //public string ReturnUrl { get; set; }
    }
    /// <summary>
    /// 表示 ajax 操作结果类型的枚举
    /// </summary>
    public enum ResultType
    {
        /// <summary>
        /// 消息结果类型
        /// </summary>
        Info = 0,

        /// <summary>
        /// 成功结果类型
        /// </summary>
        Success = 1,

        /// <summary>
        /// 警告结果类型
        /// </summary>
        Warning = 2,

        /// <summary>
        /// 异常结果类型
        /// </summary>
        Error = 3
    }
}
