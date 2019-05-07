using Hx.Common.Logs;
using Hx.Framework;
using HxBlogs.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HxBlogs.WebApp.Filters
{
    public class HandleErrorAttribute : System.Web.Mvc.HandleErrorAttribute
    {
        public ILogger Logger
        {
            get
            {
                return ContainerManager.Resolve<ILogger>();
            }
        }
        public override void OnException(ExceptionContext context)
        {
            WriteLog(context);
            base.OnException(context);
            context.ExceptionHandled = true;
            if (context.Exception is UserFriendlyException)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Result = new ContentResult { Content = new AjaxResult { type = ResultType.error, message = context.Exception.Message }.ToJson() };
            }
            else if (context.Exception is NoAuthorizeException)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                if (!context.HttpContext.Request.IsAjaxRequest())
                {
                    context.HttpContext.Response.RedirectToRoute("Default", new { controller = "Error", action = "Error401", errorUrl = context.HttpContext.Request.RawUrl });
                }
                else
                {
                    context.Result = new ContentResult { Content = context.HttpContext.Request.RawUrl };
                }
            }
            else
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                ExceptionMessage error = new ExceptionMessage(context.Exception);
                var s = error.ToJson();
                if (!context.HttpContext.Request.IsAjaxRequest())
                {
                    context.HttpContext.Response.RedirectToRoute("Default", new { controller = "Error", action = "Error500", data = WebHelper.UrlEncode(s) });
                }
                else
                {
                    context.Result = new ContentResult { Content = WebHelper.UrlEncode(s) };
                }
            }
        }


        // <summary>
        /// 写入日志（log4net）
        /// </summary>
        /// <param name="context">提供使用</param>
        private void WriteLog(ExceptionContext context)
        {
            if (context == null)
                return;
            if (context.Exception is NoAuthorizeException || context.Exception is UserFriendlyException)
            {
                //友好错误提示,未授权错误提示，记录警告日志
                Logger.Warn(context.Exception.Message);
            }
            else
            {
                //异常错误，
                Logger.Error(context.Exception.Message);
                ////TODO :写入错误日志到数据库
            }
        }
    }
}