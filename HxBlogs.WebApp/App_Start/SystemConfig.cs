using Autofac;
using Autofac.Integration.Mvc;
using Hx.Common.Logs;
using Hx.Framework;
using HxBlogs.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HxBlogs.WebApp
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public class SystemConfig
    {
        public static void Initialize()
        {
            var configs = Model.Context.DbFactory.GetDbContext().Database.SqlQuery<HxBlogs.Model.SystemConfig>("select * from SystemConfig");
            if (configs.Count() > 0)
            {
                Models.Config.SystemConfig = configs.First();
            }
            else
            {
                Models.Config.SystemConfig = new Model.SystemConfig(); ;
            }
        }
    }

    /// <summary>
    /// Autofac配置
    /// </summary>
    public class AutofacConfig
    {
        /// <summary>
        /// 负责调用autofac框架实现业务逻辑层和数据仓储层程序集中的类型对象的创建
        /// 负责创建MVC控制器类的对象(调用控制器中的有参构造函数),接管DefaultControllerFactory的工作
        /// </summary>
        public static void Register()
        {
            ContainerManager manager = new ContainerManager();
            manager.BeforeRegister += (builder) =>
            {
                builder.RegisterControllers(typeof(MvcApplication).Assembly); // 注册Mvc控制器实例
                ILoggerFactory factory = new LoggerFactory()
                {
                    UseConfig = true
                };
                builder.RegisterInstance(factory).As<ILoggerFactory>().SingleInstance();
                builder.RegisterInstance(factory.CreateLogger("Logger")).As<ILogger>().SingleInstance();
            };
            manager.Start();
            ////builder.RegisterFilterProvider();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(ContainerManager.GetContainer()));
            //将MVC的控制器对象实例 交由autofac来创建
            // ControllerBuilder.Current.SetControllerFactory(new AutofacControllerFactory());
        }
    }
    /// <summary>
    /// AutoMapper配置
    /// </summary>
    public class AutoMapperConfig
    {
        public static void Register()
        {
            MapperManager manager = new MapperManager();
            manager.Start();
        }
    }
}