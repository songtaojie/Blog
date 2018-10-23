using Common.Helper;
using HxBlogs.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HxBlogs.WebApp.Filters
{
    public class AuthFilterAttribute : AuthorizeAttribute
    {
        private bool isCheckAuth;

        public AuthFilterAttribute(bool isCheckAuth = true)
        {
            this.isCheckAuth = isCheckAuth;
        }
        //public override void OnAuthorization(AuthorizationContext filterContext)
        //{
        //    //在此可以验证权限，如验证Session和Cookie  
        //     if (filterContext == null)
        //    {
        //        throw new ArgumentNullException("filterContext");
        //    }
        //    if (AllowAnonymous(filterContext)) return;
        //    var request = filterContext.HttpContext.Request;
        //    var response = filterContext.HttpContext.Response;
        //    // 是否是Ajax请求
        //    var bAjax = request.IsAjaxRequest();
        //    // 判断是否登录或登录已超时 需要重新登录
        //    if (Infrastructure.OperateContext.Current.UserInfo == null)
        //    {
        //        if (bAjax)
        //        {
        //            BusinessResultBase result = new BusinessResultBase();
        //            result.Title = "未登录或登录已超时";
        //            result.Status = false;
        //            result.StatusCode = BusinessStatusCode.LoginTimeOut.ToString();
        //            result.StatusMessage = "请重新登录系统。";

        //            var jsonResult = new JsonResult();
        //            jsonResult.Data = result;
        //            filterContext.Result = jsonResult;
        //        }
        //        else
        //        {
        //            filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { Controller = "Account", action = "Login" }));
        //        }
        //    }
        //    else
        //    {
        //        // 2、拒绝某个账号登录当前系统
        //        if (OperateContext.Current.IsDenyVisit())
        //        {
        //            if (bAjax)
        //            {
        //                BusinessResultBase result = new BusinessResultBase();
        //                result.Title = "拒绝访问当前系统";
        //                result.Status = false;
        //                result.StatusCode = BusinessStatusCode.AccessDeny.ToString();
        //                result.StatusMessage = "您的账号不允许访问当前系统。";
        //                var jsonResult = new JsonResult();
        //                jsonResult.Data = result;
        //                filterContext.Result = jsonResult;
        //            }
        //            else
        //            {
        //                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { Controller = "Common", action = "DenyAccess", bAjaxReq = false, message = "没有获取您拥有的权限菜单，请尝试重新登录。" }));
        //            }
        //        }
        //        else
        //        {
        //            // 3、判断登录状态 Controller  Action 标签 某些功能只需判断是否登录 用户没登录 调到登录页面  
        //            // 判断Controller上是否有CheckLoginAttribute标签 只需要登录就可以访问
        //            var checkLoginControllerAttr = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(CheckLoginAttribute), true) as IEnumerable<CheckLoginAttribute>;
        //            if (checkLoginControllerAttr != null && checkLoginControllerAttr.Any())
        //            {
        //                return;
        //            }
        //            // 判断action上是否有CheckLoginAttribute标签 只需要登录就可以访问
        //            var checkLoginActionAttr = filterContext.ActionDescriptor.GetCustomAttributes(typeof(CheckLoginAttribute), true) as IEnumerable<CheckLoginAttribute>;
        //            if (checkLoginActionAttr != null && checkLoginActionAttr.Any())
        //            {
        //                return;
        //            }
        //            // 4、有些要判断是否有某个菜单 action的权限 具体判断某个用户是否有某个权限
        //            // 用于标记在授权期间需要CustomerResourceAttribute 的操作的特性
        //            var attNames = filterContext.ActionDescriptor.GetCustomAttributes(typeof(CustomerResourceAttribute), true) as IEnumerable<CustomerResourceAttribute>;
        //            // 用户具有的菜单
        //            var moduleList = OperateContext.Current.GetPermissionList();
        //            if (moduleList == null || !moduleList.Any())
        //            {
        //                if (bAjax)
        //                {
        //                    BusinessResultBase result = new BusinessResultBase();
        //                    result.Title = "没有访问权限";
        //                    result.Status = false;
        //                    result.StatusCode = BusinessStatusCode.AccessDeny.ToString();
        //                    result.StatusMessage = "没有获取您拥有的权限菜单，请尝试重新登录。";
        //                    var jsonResult = new JsonResult();
        //                    jsonResult.Data = result;
        //                    filterContext.Result = jsonResult;
        //                }
        //                else
        //                {
        //                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { Controller = "Common", action = "DenyAccess", bAjaxReq = false, message = "没有获取您拥有的权限菜单，请尝试重新登录。" }));
        //                }
        //            }
        //            else
        //            {
        //                // 判断用户的权限菜单中的code是否与控制器上标示的资源的code一致
        //                var joinResult = (from aclEntity in moduleList
        //                                  join attName in attNames on aclEntity.Code equals attName.ResourceName
        //                                  select attName).Any();
        //                if (!joinResult)
        //                {

        //                    if (bAjax)
        //                    {
        //                        BusinessResultBase result = new BusinessResultBase();
        //                        result.Title = "没有访问权限";
        //                        result.Status = false;
        //                        result.StatusCode = BusinessStatusCode.AccessDeny.ToString();
        //                        result.StatusMessage = "您没有访问权限：" + pageUrl;
        //                        var jsonResult = new JsonResult();
        //                        jsonResult.Data = result;
        //                        filterContext.Result = jsonResult;
        //                    }
        //                    else
        //                    {
        //                        filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { Controller = "Common", action = "DenyAccess", bAjaxReq = false, message = "您没有访问权限：" + pageUrl }));
        //                    }
        //                }
        //                else
        //                {
        //                    return;
        //                }
        //            }
        //        }
        //    }
        //    base.OnAuthorization(filterContext);
        //}
        /// <summary>
        /// 是否允许匿名访问
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        private bool AllowAnonymous(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.Url == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            string pageUrl = filterContext.HttpContext.Request.Url.AbsolutePath; //OperateContext.GetThisPageUrl(false);
            
            // 1、允许匿名访问 用于标记在授权期间要跳过 AuthorizeAttribute 的控制器和操作的特性 
            var actionAnonymous = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true) as IEnumerable<AllowAnonymousAttribute>;
            var controllerAnonymous = filterContext.Controller.GetType().GetCustomAttributes(typeof(AllowAnonymousAttribute), true) as IEnumerable<AllowAnonymousAttribute>;
            if ((actionAnonymous != null && actionAnonymous.Any()) || (controllerAnonymous != null && controllerAnonymous.Any()))
            {
                return true;
            }
            return false;
        }
        private bool VerifyCookie(HttpRequestBase request, HttpResponseBase response)
        {

            if (!request.Cookies.AllKeys.Contains(CookieInfo.CookieName)
                || !request.Cookies.AllKeys.Contains(CookieInfo.CacheKeyCookieName))
            {
                return false;
            }

            var cookie = request.Cookies[CookieInfo.CookieName];
            //string cacheKey = Base64Helper.Base64Decode(request.Cookies[CookieInfo.CacheKeyCookieName].Value);

            //ICacheManager cache = ContainerManager.Resolve<ICacheManager>();
            //string cacheCookieValue = cache.Get<string>(cacheKey);
            //if (cacheCookieValue != cookie.Value)
            //{
            //    return false;
            //}

            return true;
        }


    }
}