namespace PluginsTutorial.Web.Models
{
	public class FileInfo
	{
		public string name { get; set; }
		public string type { get; set; }
		public long size { get; set; }
		public string progress { get; set; }
		public string url { get; set; }
		public string thumbnail_url { get; set; }
		public string delete_url { get; set; }
		public string delete_type { get; set; }
	}
}