using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Framework.Log
{
    /// <summary>
    /// 写入日志的接口，实现可以使用log4net、NLog
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// 调试程序的记录信息
        /// </summary>
        /// <param name="message"></param>
        void Debug(string message);
        /// <summary>
        /// 调试程序的记录信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        void Debug(string message, Exception ex);
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="message"></param>
        void Error(string message);
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        void Error(string message, Exception ex);
        void Fatal(string message);
        /// <summary>
        /// 致命的信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        void Fatal(string message, Exception ex);
        /// <summary>
        /// 信息类型的信息
        /// </summary>
        /// <param name="message"></param>
        void Info(string message);
        /// <summary>
        /// 信息类型的信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        void Info(string message, Exception ex);
        /// <summary>
        /// 警告类型的额信息
        /// </summary>
        /// <param name="message"></param>
        void Warn(string message);
        /// <summary>
        /// 警告类型的信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        void Warn(string message, Exception ex);
    }
}
