using Autofac;
using HxBlogs.Framework.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Compilation;

namespace HxBlogs.Framework
{

    public class DefaultContainer
    {
        private static ContainerBuilder builder;
        static DefaultContainer()
        {
            builder = new ContainerBuilder();
        }
        public static ContainerBuilder CurrentContainer
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
            Assembly[] assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToArray();
            builder.RegisterAssemblyTypes(assemblies).Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract)
                .AsImplementedInterfaces().InstancePerLifetimeScope();//每次解析获得新实例

            Type singletonType = typeof(ISingletonDependency);
            builder.RegisterAssemblyTypes(assemblies).Where(type => singletonType.IsAssignableFrom(type) && !type.IsAbstract)
                .AsImplementedInterfaces().SingleInstance();// 保证对象生命周期基于单例

            Type requestType = typeof(IRequestDependency);
            builder.RegisterAssemblyTypes(assemblies).Where(type => singletonType.IsAssignableFrom(type) && !type.IsAbstract)
                .AsImplementedInterfaces().InstancePerRequest();// 保证对象生命周期基于单例

            builder.RegisterAssemblyModules(assemblies);//所有继承module中的类都会被注册

            IContainer container = builder.Build();
            ContainerManager.SetContainer(container);
        }
    }
}
