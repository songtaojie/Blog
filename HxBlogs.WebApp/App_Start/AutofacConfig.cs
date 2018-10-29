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
            DefaultContainer container = new DefaultContainer();
            container.BeforeRegister += (builder) =>
            {
                builder.RegisterControllers(typeof(MvcApplication).Assembly); // 注册Mvc控制器实例
                builder.RegisterType<LoggerFactory>().As<ILoggerFactory>().SingleInstance();//注册日志对象
            };
            container.Start();
            ////builder.RegisterFilterProvider();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(ContainerManager.Container));
            //将MVC的控制器对象实例 交由autofac来创建
            // ControllerBuilder.Current.SetControllerFactory(new AutofacControllerFactory());
        }
    }
}