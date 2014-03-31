using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PluginsTutorial.Web.Models
{
	public class FilesStatus
	{
		public string name { get; set; }
		public string type { get; set; }
		public int size { get; set; }
		public string progress { get; set; }
		public string url { get; set; }
		public string thumbnail_url { get; set; }
		public string delete_url { get; set; }
		public string delete_type { get; set; }


		string VirtualUploadPath = string.Empty;
		string UploadPath = string.Empty;

		public FilesStatus(System.IO.FileInfo fileInfo, string virtualUploadPath, string uploadPath)
		{
			this.VirtualUploadPath = virtualUploadPath;
			this.UploadPath = uploadPath;
			SetValues(fileInfo.Name, (int)fileInfo.Length);
		}

		public FilesStatus(HttpPostedFile file, string virtualUploadPath, string uploadPath)
		{
			this.VirtualUploadPath = virtualUploadPath;
			this.UploadPath = uploadPath;
			SetValues(file.FileName, (int)file.ContentLength);
		}


		void SetValues(string fileName, int fileLength)
		{
			name = fileName;
			type = "image/png";  //type = hpf.ContentType;
			size = fileLength;
			progress = "1.0";
			url = "/Controllers/FileUpload/AdvancedFileUploadHandler.ashx?UploadPath=" + UploadPath + "&f=" + fileName;
			thumbnail_url = this.VirtualUploadPath + fileName;
			delete_url = "/Controllers/FileUpload/AdvancedFileUploadHandler.ashx?Action=Delete&UploadPath=" + UploadPath + "&f=" + fileName;
			delete_type = "POST";
		}

	}
}