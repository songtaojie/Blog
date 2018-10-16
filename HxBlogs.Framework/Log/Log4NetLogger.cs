using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Framework.Log
{
    /// <summary>
    /// 使用log4net记录日志信息
    /// </summary>
    public class Log4NetLogger : ILogger
    {
        /// <summary>
        /// 组
        /// </summary>
        private string group;
        private ILog log;

        public void Debug(string message)
        {
            throw new NotImplementedException();
        }

        public void Debug(string message, Exception ex)
        {
            throw new NotImplementedException();
        }

        public void Error(string message)
        {
            throw new NotImplementedException();
        }

        public void Error(string message, Exception ex)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string message)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string message, Exception ex)
        {
            throw new NotImplementedException();
        }

        public void Info(string message)
        {
            throw new NotImplementedException();
        }

        public void Info(string message, Exception ex)
        {
            throw new NotImplementedException();
        }

        public void Warn(string message)
        {
            throw new NotImplementedException();
        }

        public void Warn(string message, Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
