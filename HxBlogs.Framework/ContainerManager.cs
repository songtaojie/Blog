using Autofac;
using HxBlogs.Framework.Dependency;
using HxBlogs.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Compilation;

namespace HxBlogs.Framework
{
    public class ContainerManager
    {
        private static ContainerBuilder builder;
        static ContainerManager()
        {
            builder = new ContainerBuilder();
        }
        private static IContainer _container;
        public static void SetContainer(IContainer container)
        {
            _container = container;
        }
        /// <summary>
        /// 创建，连接依赖关系并管理一组组件的生命周期的容器
        /// </summary>
        public static IContainer GetContainer()
        {
            return _container;
        }
        /// <summary>
        /// 从上下文中检索服务。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
        /// <summary>
        /// 从上下文中检索服务。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object Resolve(Type type)
        {
            return _container.Resolve(type);
        }
        /// <summary>
        /// 一个委托在Build之前会执行当前委托
        /// </summary>
        public event Action<ContainerBuilder> BeforeRegister;
        public static ContainerBuilder Builder
        {
            get
            {
                return builder;
            }
        }
        /// <summary>
        /// 在应用程序启动时进行服务的注入
        /// </summary>
        public void Start()
        {
            Type baseType = typeof(ITransientDependency);
            // 获取所有相关类库的程序集
            Assembly[] assemblies = TypeHelper.GetAllAssembly().ToArray();
            builder.RegisterAssemblyTypes(assemblies).Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract)
                .AsImplementedInterfaces().InstancePerLifetimeScope();//每次解析获得新实例

            Type singletonType = typeof(ISingletonDependency);
            builder.RegisterAssemblyTypes(assemblies).Where(type => singletonType.IsAssignableFrom(type) && !type.IsAbstract)
                .AsImplementedInterfaces().SingleInstance();// 保证对象生命周期基于单例

            Type requestType = typeof(IRequestDependency);
            builder.RegisterAssemblyTypes(assemblies).Where(type => singletonType.IsAssignableFrom(type) && !type.IsAbstract)
                .AsImplementedInterfaces().InstancePerRequest();// 保证对象生命周期基于单例

            // builder.RegisterAssemblyModules(assemblies);//所有继承module中的类都会被注册
            BeforeRegister?.Invoke(builder);
            IContainer container = builder.Build();
            SetContainer(container);
        }
    }
}
