using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Features.ResolveAnything;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Com.Stone.HuLuBlog.Infrastructure.AutoFac;
using Com.Stone.HuLuBlog.Repositories.SqlSugar;

namespace Com.Stone.HuLuBlog.Web
{
    public class IOCConfig
    {
        /// <summary>
        /// RegisterAll
        /// </summary>
        public static void RegisterAll()
        {
            var iocBuilder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;

            //注册DbContext 单例模式
            iocBuilder.RegisterType<SqlSugarRepositoryContext>()
                .As<ISqlSugarRepositoryContext>().SingleInstance();

            //注册所有IDependency接口的实现
            var assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>()
                        .Where(
                            assembly =>
                                assembly.GetTypes()
                                .FirstOrDefault(type => type.GetInterfaces()
                                .Contains(typeof(IDependency))) != null
                        );
            iocBuilder.RegisterAssemblyTypes(assemblies.ToArray())
                        .Where(n => n.GetInterfaces().Contains(typeof(IDependency)))
                        .AsImplementedInterfaces()
                        .InstancePerDependency();

            //注册过滤器
            iocBuilder.RegisterFilterProvider();
            iocBuilder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            // OPTIONAL: Register the Autofac filter provider.
            iocBuilder.RegisterWebApiFilterProvider(config);
            // OPTIONAL: Register the Autofac model binder provider.
            iocBuilder.RegisterWebApiModelBinderProvider();



            //注册所有Controller
            iocBuilder.RegisterControllers(Assembly.GetExecutingAssembly());
            iocBuilder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Set the dependency resolver to be Autofac.
            IContainer iocContainer = iocBuilder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(iocContainer);

            //设置依赖注入解析器
            DependencyResolver.SetResolver(new AutofacDependencyResolver(iocContainer));

            ContainerManager.SetContainer(iocContainer);
        }
    }
}