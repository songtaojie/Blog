using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Repository;
using log4net;
using System.IO;
using log4net.Config;
using System.Web;

namespace HxBlogs.Framework.Log
{
    public class Log4NetManager
    {
        private ILog logger;
        /// <summary>
        /// 初始化Log4Net管理类，并加根目录中的log4net.config配置文件
        /// </summary>
        public Log4NetManager(Type type)
        {
            FileInfo info;
            if (HttpContext.Current != null)
            {
                info = new FileInfo(HttpContext.Current.Server.MapPath("~/log4net.config"));
            }
            else
            {
                info = new FileInfo("log4net.config");
            }
            XmlConfigurator.ConfigureAndWatch(info);
            // logger = LoggerManager.GetLogger(type);
        }
        /// <summary>
        /// 适用指定的诶之文件路径初始化Log4Net管理类
        /// </summary>
        /// <param name="configPath"></param>
        public Log4NetManager(string configPath)
        {
            FileInfo info;
            if (string.IsNullOrEmpty(configPath))
            {
                if (HttpContext.Current != null)
                {
                    info = new FileInfo(HttpContext.Current.Server.MapPath("~/log4net.config"));
                }
                else
                {
                    info = new FileInfo("log4net.config");
                }
            }
            else
            {
                info = new FileInfo(configPath);
            }
            
            XmlConfigurator.ConfigureAndWatch(info);
        }

    }
}
