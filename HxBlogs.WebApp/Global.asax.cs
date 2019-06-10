#define DEBUG
using Hx.Common.Logs;
using Hx.Framework;
using StackExchange.Profiling;
using StackExchange.Profiling.EntityFramework6;
using StackExchange.Profiling.Mvc;
using StackExchange.Profiling.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HxBlogs.WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public ILogger Logger
        {
            get
            {
                return ContainerManager.Resolve<ILogger>();
            }
        }
        protected void Application_Start()
        {
            AutofacConfig.Register();
            AutoMapperConfig.Register();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            GlobalFilters.Filters.Add(new ProfilingActionFilter());
            MiniProfilerEF6.Initialize();
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            HttpException httpEx = new HttpException(ex.Message, ex);
            if (httpEx.GetHttpCode() == 404)
            {
                string msg ="错误消息："+ httpEx.Message;
                Logger.Error(msg, httpEx);
                bool isAjax = new HttpRequestWrapper(Context.Request).IsAjaxRequest();
                if (isAjax)
                {
                    Response.Clear();
                    Response.Write(Context.Request.RawUrl);
                }
                else
                {
                    //Response.RedirectToRoute("Default", new { controller = "error", action = "error404", errorUrl = Context.Request.RawUrl });
                    Response.Redirect("/error/error404?errorUrl=" + HttpUtility.UrlEncode(Context.Request.RawUrl));
                }
            }
        }
        protected void Application_BeginRequest(Object source, EventArgs e)
        {
            if (Request.IsLocal)
            {
                MiniProfiler.Start();
            }
            //#if DEBUG
            //            
            //#endif
        }
        protected void Application_EndRequest()
        {
            MiniProfiler.Stop();
            //#if DEBUG
            //            
            //#endif
        }
        /// <summary>
        /// Gets or sets a value indicating whether disable profiling results.
        /// </summary>
        public static bool DisableProfilingResults { get; set; } = false;
        /// <summary>
        /// Customize aspects of the MiniProfiler.
        /// </summary>
    }
}
