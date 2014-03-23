using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using PluginsTutorial.Data;
using PluginsTutorial.Services;

namespace PluginsTutorial.Web.App_Start
{
	public class IocConfig
	{
		public static void RegisterDependencies()
		{
			var builder = new ContainerBuilder();
			builder.RegisterControllers(typeof(MvcApplication).Assembly);

			builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerHttpRequest();
			builder.RegisterType<DbContextFactory>().As<IContextFactory>().InstancePerHttpRequest();
			builder.RegisterType<GenericRepository>().As<IGenericRepository>().InstancePerHttpRequest();
			builder.RegisterType<Service>().As<IService>().InstancePerHttpRequest();

			IContainer container = builder.Build();
			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
		}
	}
}