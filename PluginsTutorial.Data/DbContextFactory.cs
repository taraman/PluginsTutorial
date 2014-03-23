using System;
using System.Data.Entity;
using PluginsTutorial.Data.Models;

namespace PluginsTutorial.Data
{
	public class DbContextFactory : IContextFactory, IDisposable
	{
		DbContext _dataContext;
		public DbContext Get()
		{
			return _dataContext ?? (_dataContext = new OrdersContext());
		}

		~DbContextFactory()
		{
			Dispose(false);
		}
		
		#region IDisposable Members
		bool _isDisposed;

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		void Dispose(bool disposing)
		{
			if (!_isDisposed && disposing && _dataContext != null)
			{
				_dataContext.Dispose();
				_dataContext = null;
			}
			_isDisposed = true;
		}
		#endregion
	}
}
