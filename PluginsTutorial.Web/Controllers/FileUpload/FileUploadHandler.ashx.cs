using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using PluginsTutorial.Web.Models;

namespace PluginsTutorial.Web
{
	/// <summary>
	/// Summary description for FileUploadHandler
	/// </summary>
	public class FileUploadHandler : IHttpHandler
	{

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/plain";//"application/json";
			var r = new System.Collections.Generic.List<ViewDataUploadFilesResult>();
			JavaScriptSerializer js = new JavaScriptSerializer();
			foreach (string file in context.Request.Files)
			{
				HttpPostedFile hpf = context.Request.Files[file] as HttpPostedFile;
				//HttpPostedFileBase hpf = context.Request.Files[file] as HttpPostedFileBase;

				string FileName = string.Empty;
				if (HttpContext.Current.Request.Browser.Browser.ToUpper() == "IE")
				{
					string[] files = hpf.FileName.Split(new char[] { '\\' });
					FileName = files[files.Length - 1];
				}
				else
				{
					FileName = hpf.FileName;
				}
				if (hpf.ContentLength == 0)
					continue;

				var savedFileName = Path.Combine(context.Server.MapPath("~/App_Data"), Path.GetFileName(hpf.FileName));
				hpf.SaveAs(savedFileName);

				r.Add(new ViewDataUploadFilesResult()
				{
					//Thumbnail_url = savedFileName,
					Name = FileName,
					Length = hpf.ContentLength,
					Type = hpf.ContentType
				});
				var uploadedFiles = new
				{
					files = r.ToArray()
				};
				var jsonObj = js.Serialize(uploadedFiles);
				//jsonObj.ContentEncoding = System.Text.Encoding.UTF8;
				//jsonObj.ContentType = "application/json;";
				context.Response.Write(jsonObj.ToString());
			}
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