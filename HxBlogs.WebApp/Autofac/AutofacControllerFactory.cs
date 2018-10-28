using HxBlogs.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HxBlogs.WebApp
{
    public class AutofacControllerFactory : DefaultControllerFactory
    {
        /// <summary>
        /// 由MVC系统调用并为给定的控制器类型创建控制器实例
        /// </summary>
        /// <param name="requestContext">Request context</param>
        /// <param name="controllerType">Controller type</param>
        /// <returns></returns>
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                return base.GetControllerInstance(requestContext, controllerType);
            }

            return ContainerManager.Resolve(controllerType) as IController;
        }
    }
}