using System.Data.Entity;

namespace PluginsTutorial.Data
{
	public interface IUnitOfWork
	{
		DbContext DataContext { get; }
		void Commit();
	}
}
