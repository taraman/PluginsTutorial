using System.Data.Entity;

namespace PluginsTutorial.Data
{
	public interface IContextFactory
	{
		DbContext Get();
	}
}
