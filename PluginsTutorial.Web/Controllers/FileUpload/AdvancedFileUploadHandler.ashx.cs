using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using PluginsTutorial.Web.Models;
using FileInfo = PluginsTutorial.Web.Models.FileInfo;

namespace PluginsTutorial.Web.Controllers.FileUpload
{

	//Absolute Path & Relative Path  & Virtual Path

	public class AdvancedFileUploadHandler : IHttpHandler
	{
		HttpContext Context { get; set; }

		readonly JavaScriptSerializer js = new JavaScriptSerializer();

		const string AbsoluteRootPath = "/Resources/";
		
		string FolderPath
		{
			get
			{
				var folderPath = Context.Request["FolderPath"];
				return folderPath;
			}
		}

		string VirtualFolderPath
		{
			get
			{
				var virtualFolderPath = Path.Combine(HttpRuntime.AppDomainAppVirtualPath ?? "", "/", AbsoluteRootPath, FolderPath);
				return virtualFolderPath;
			}
		}

		string PhysicalFolderPath
		{
			get
			{
				var physicalFolderPath = Context.Server.MapPath(VirtualFolderPath);
				return physicalFolderPath;
			}
		}

		string FileName
		{
			get
			{
				return Context.Request["f"];
			}
		}
		

		public void ProcessRequest(HttpContext context)
		{
			Context = context;
			Context.Response.AddHeader("Pragma", "no-cache");
			Context.Response.AddHeader("Cache-Control", "private, no-cache");

			switch (Context.Request.HttpMethod)
			{
				case "HEAD":
				case "GET":
					if (string.IsNullOrEmpty(FileName))
						ListFiles();
					else
						GetFile();
					break;

				case "POST":
				case "PUT":
					if (string.IsNullOrEmpty(Context.Request["Action"]))
						UploadFile();
					else
						DeleteFile();
					break;

				case "OPTIONS":
					Context.Response.AddHeader("Allow", "DELETE,GET,HEAD,POST,PUT,OPTIONS");
					Context.Response.StatusCode = 200;
					break;

				default:
					Context.Response.ClearHeaders();
					Context.Response.StatusCode = 405;
					break;
			}
		}




		void ListFiles()
		{
			if (!Directory.Exists(PhysicalFolderPath)) return;

			var existingFiles = new DirectoryInfo(PhysicalFolderPath)
				.GetFiles("*", SearchOption.TopDirectoryOnly)
				.Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden)).ToList();

			var files = existingFiles.Select(f => new FileInfo
				{
					name = f.Name,
					size = f.Length,
					thumbnail_url = string.Concat(VirtualFolderPath, "/", f.Name),
					url = "/Controllers/FileUpload/AdvancedFileUploadHandler.ashx?Action=View&FolderPath=" + FolderPath + "&f=" + f.Name,
					type = "image/png",  //type = hpf.ContentType;
					progress = "1.0",
					delete_url = "/Controllers/FileUpload/AdvancedFileUploadHandler.ashx?Action=Delete&FolderPath=" + FolderPath + "&f=" + f.Name,
					delete_type = "POST"
				}).ToList();

			Context.Response.Write(js.Serialize(files));
			Context.Response.AddHeader("Content-Disposition", "inline; filename=\"files.json\"");
			Context.Response.ContentType = "application/json";
		}
		
		void GetFile()
		{
			var filePath = Path.Combine(PhysicalFolderPath, Path.GetFileName(FileName));

			if (File.Exists(filePath))
			{
				Context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + FileName + "\"");
				Context.Response.ContentType = "application/octet-stream";
				Context.Response.ClearContent();
				Context.Response.WriteFile(filePath);
			}
			else
			{
				Context.Response.StatusCode = 404;
			}
		}
		
		void DeleteFile()
		{
			var filePath = Path.Combine(PhysicalFolderPath, Path.GetFileName(FileName));
			File.Delete(filePath);
		}	


		
		private void UploadFile()
		{
			var statuses = new List<FileInfo>();
			var headers = Context.Request.Headers;

			if (string.IsNullOrEmpty(headers["X-File-Name"]))
			{
				UploadEntireFile(statuses);
			}
			else
			{
				//UploadPartialFile(headers["X-File-Name"], statuses);
			}

			WriteJsonIframeSafe(statuses);
		}


		void UploadEntireFile(List<FileInfo> statuses)
		{
			if (!Directory.Exists(PhysicalFolderPath))
				Directory.CreateDirectory(PhysicalFolderPath);

			for (var i = 0; i < Context.Request.Files.Count; i++)
			{
				var hpf = Context.Request.Files[i];
				var fileName = hpf.FileName;
				var filePath = Path.Combine(PhysicalFolderPath, Path.GetFileName(fileName));
				hpf.SaveAs(filePath);

				statuses.Add(new FileInfo()
					{
						name = fileName,
						size = hpf.ContentLength,
						thumbnail_url = string.Concat(VirtualFolderPath, "/", hpf.FileName),
						url = "/Controllers/FileUpload/AdvancedFileUploadHandler.ashx?Action=View&FolderPath=" + FolderPath + "&f=" + fileName,
						type = "image/png",  //type = hpf.ContentType;
						progress = "1.0",
						delete_url = "/Controllers/FileUpload/AdvancedFileUploadHandler.ashx?Action=Delete&FolderPath=" + FolderPath + "&f=" + fileName,
						delete_type = "POST"
					});
			}
		}





		// Upload partial file
		private void UploadPartialFile(string fileName, List<FilesStatus> statuses)
		{
			if (Context.Request.Files.Count != 1) 
				throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");
			var inputStream = Context.Request.Files[0].InputStream;
			
			
			//var fullName = PhysicalPath + Path.GetFileName(fileName);
			var fullName = PhysicalFolderPath + Path.GetFileName(fileName);
			

			using (var fs = new FileStream(fullName, FileMode.Append, FileAccess.Write))
			{
				var buffer = new byte[1024];

				var l = inputStream.Read(buffer, 0, 1024);
				while (l > 0)
				{
					fs.Write(buffer, 0, l);
					l = inputStream.Read(buffer, 0, 1024);
				}
				fs.Flush();
				fs.Close();
			}
			statuses.Add(new FilesStatus(new System.IO.FileInfo(fullName), this.VirtualFolderPath, FolderPath));
		}

		

		void WriteJsonIframeSafe(List<FileInfo> statuses)
		{
			Context.Response.AddHeader("Vary", "Accept");
			try
			{
				Context.Response.ContentType = Context.Request["HTTP_ACCEPT"].Contains("application/json") ? "application/json" : "text/plain";
			}
			catch
			{
				Context.Response.ContentType = "text/plain";
			}

			var uploadedFiles = new
			{
				files = statuses.ToArray()
			};

			var jsonObj = js.Serialize(uploadedFiles);
			Context.Response.Write(jsonObj);
		}




		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

	}
}