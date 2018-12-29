using HxBlogs.Framework;
using Autofac;
using Autofac.Integration.Mvc;
using Common.Logs;
using System.Web.Mvc;

namespace HxBlogs.WebApp
{
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
                // builder.RegisterType<LoggerFactory>().As<ILoggerFactory>().SingleInstance();//注册日志对象
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
    public class AutoMapperConfig
    {
        public static void Register()
        {
            MapperManager manager = new MapperManager();
            manager.Start();
        }
    }
}