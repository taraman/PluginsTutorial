using System.Data.Entity;

namespace PluginsTutorial.Data
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly IContextFactory _contextFactory;

		DbContext _dataContext;
		public DbContext DataContext
		{
			get { return _dataContext ?? (_dataContext = _contextFactory.Get()); }
		}

		public UnitOfWork(IContextFactory contextFactory)
		{
			_contextFactory = contextFactory;

			DataContext.Configuration.LazyLoadingEnabled = false; //Stop lazy loading
			DataContext.Configuration.ProxyCreationEnabled = false; //Stop creating proxy for database entities
		}

		public void Commit()
		{
			DataContext.SaveChanges();
		}
	}
}
