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
            InitProfilerSettings();
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
                MiniProfiler.StartNew();
            }
            //#if DEBUG
            //            
            //#endif
        }
        protected void Application_EndRequest()
        {
            MiniProfiler.Current?.Stop();
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
        private void InitProfilerSettings()
        {
            // A powerful feature of the MiniProfiler is the ability to share links to results with other developers.
            // by default, however, long-term result caching is done in HttpRuntime.Cache, which is very volatile.
            // 
            // Let's rig up serialization of our profiler results to a database, so they survive app restarts.
            MiniProfiler.Configure(new MiniProfilerOptions
            {
                // Sets up the route to use for MiniProfiler resources:
                // Here, ~/profiler is used for things like /profiler/mini-profiler-includes.js)
                RouteBasePath = "~/profiler",

               
                // Different RDBMS have different ways of declaring sql parameters - SQLite can understand inline sql parameters just fine.
                // By default, sql parameters will be displayed.
                //SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter(),

                // These settings are optional and all have defaults, any matching setting specified in .RenderIncludes() will
                // override the application-wide defaults specified here, for example if you had both:
                //    PopupRenderPosition = RenderPosition.Right;
                //    and in the page:
                //    @MiniProfiler.Current.RenderIncludes(position: RenderPosition.Left)
                // ...then the position would be on the left on that page, and on the right (the application default) for anywhere that doesn't
                // specified position in the .RenderIncludes() call.
                PopupRenderPosition = RenderPosition.BottomLeft,  // defaults to left
                PopupMaxTracesToShow = 10,                   // defaults to 15

                // ResultsAuthorize (optional - open to all by default):
                // because profiler results can contain sensitive data (e.g. sql queries with parameter values displayed), we
                // can define a function that will authorize clients to see the JSON or full page results.
                // we use it on http://stackoverflow.com to check that the request cookies belong to a valid developer.
                ResultsAuthorize = request =>
                {
                    // you may implement this if you need to restrict visibility of profiling on a per request basis

                    // for example, for this specific path, we'll only allow profiling if a query parameter is set
                    //if ("/Home/ResultsAuthorization".Equals(request.Url.LocalPath, StringComparison.OrdinalIgnoreCase))
                    //{
                    //    return (request.Url.Query).IndexOf("isauthorized", StringComparison.OrdinalIgnoreCase) >= 0;
                    //}

                    // all other paths can check our global switch
                    return !DisableProfilingResults;
                },

                // ResultsListAuthorize (optional - open to all by default)
                // the list of all sessions in the store is restricted by default, you must return true to allow it
                ResultsListAuthorize = request =>
                {
                    // you may implement this if you need to restrict visibility of profiling lists on a per request basis 
                    return true; // all requests are legit in our happy world
                },

                // Stack trace settings
                StackMaxLength = 256, // default is 120 characters

                // (Optional) You can disable "Connection Open()", "Connection Close()" (and async variant) tracking.
                // (defaults to true, and connection opening/closing is tracked)
                TrackConnectionOpenClose = true
            }
            // Optional settings to control the stack trace output in the details pane
            .ExcludeType("SessionFactory")  // Ignore any class with the name of SessionFactory)
            .ExcludeAssembly("NHibernate")  // Ignore any assembly named NHibernate
            .ExcludeMethod("Flush")         // Ignore any method with the name of Flush
            .AddViewProfiling()              // Add MVC view profiling
            );

            MiniProfilerEF6.Initialize();
        }
    }
}
