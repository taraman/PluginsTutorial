using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using PluginsTutorial.Web.Models;
using FileInfo = System.IO.FileInfo;


namespace PluginsTutorial.Web.Controllers.FileUpload
{
    public class JQueryFileUploadController : Controller
    {
		const string AbsoluteRootPath = "/Resources/";

        string VirtualRootPath
        {
            get
            {
                var virtualFolderPath = Path.Combine(HttpRuntime.AppDomainAppVirtualPath ?? "", "/", AbsoluteRootPath);
                return virtualFolderPath;
            }
        }

		public ActionResult GenericHandlerBasic()
		{
			return View("~/Views/FileUpload/JQueryFileUpload/UsingGenericHandler/Basic.aspx");
		}
		
		public ActionResult GenericHandlerAdvanced()
		{
			return View("~/Views/FileUpload/JQueryFileUpload/UsingGenericHandler/Advanced.aspx");
		}
		
        public ActionResult Basic()
        {
            return View("~/Views/FileUpload/JQueryFileUpload/Demo/Basic.cshtml");
        }

        public ActionResult BasicPlus()
        {
            return View("~/Views/FileUpload/JQueryFileUpload/Demo/BasicPlus.cshtml");
        }

		public ActionResult BasicPlusUI()
		{
			return View("~/Views/FileUpload/JQueryFileUpload/Demo/BasicPlusUI.cshtml");
		}

      
		[HttpPost]
		public ActionResult UploadFiles(string folderPath)
		{
			var virtualFolderPath = Path.Combine(VirtualRootPath, folderPath + "/");
			var physicalFolderPath = Server.MapPath(virtualFolderPath);

			if (!Directory.Exists(physicalFolderPath)) 
				Directory.CreateDirectory(physicalFolderPath);

			var files = new List<dynamic>();
			foreach (string file in Request.Files)
			{
				var hpf = Request.Files[file];
				if (hpf.ContentLength == 0) continue;
				
				var fileUrl = Path.Combine(virtualFolderPath, Path.GetFileName(hpf.FileName));
				hpf.SaveAs(Server.MapPath(fileUrl));
				files.Add(new 
				{
					name = hpf.FileName,
					size = hpf.ContentLength,
					url = fileUrl,
					thumbnailUrl = fileUrl,
					deleteUrl = "/JQueryFileUpload/DeleteFile?fileUrl=" + fileUrl,
					type = hpf.ContentType,
				});
			}

			var result = Json(new { files = files }, JsonRequestBehavior.AllowGet);
			//for IE9 or less which does not accept application/json
			if (Request.Headers["Accept"] != null && !Request.Headers["Accept"].Contains("application/json"))
				result.ContentType = "text/plain";
			return result;
		}


		public ActionResult GetFiles(string folderPath)
		{
			var virtualFolderPath = Path.Combine(VirtualRootPath, folderPath + "/");
			var physicalFolderPath = Server.MapPath(virtualFolderPath);

			var files = new List<dynamic>();

			if (Directory.Exists(physicalFolderPath))
			{
				var existingFiles = new DirectoryInfo(physicalFolderPath)
					.GetFiles("*", SearchOption.TopDirectoryOnly)
					.Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden)).ToList();
				
				foreach (var f in existingFiles)
				{
					var fileUrl = Path.Combine(virtualFolderPath, f.Name);
					files.Add(new
					{
						name = f.Name,
						size = f.Length,
						url = fileUrl,
						thumbnailUrl = fileUrl,
						deleteUrl = "/JQueryFileUpload/DeleteFile?fileUrl=" + fileUrl
						//type = "image/png", //type = hpf.ContentType;
						//progress = "1.0",
						//deleteType = "POST"
					});
				}
			}

			var result = Json(new { files = files }, JsonRequestBehavior.AllowGet);
			//for IE9 or less which does not accept application/json
			if (Request.Headers["Accept"] != null && !Request.Headers["Accept"].Contains("application/json"))
				result.ContentType = "text/plain";
			return result;
		}


		[HttpPost]
		public ActionResult DeleteFile(string fileUrl)
		{
			var filePath = Server.MapPath(fileUrl);
			System.IO.File.Delete(filePath);

			var result = Json(new { error = String.Empty });
			//for IE9 which does not accept application/json
			if (Request.Headers["Accept"] != null && !Request.Headers["Accept"].Contains("application/json"))
				result.ContentType = "text/plain";
			return result;
		}

	

		
		//public FilePathResult Image()
		//{
		//	string filename = Request.Url.AbsolutePath.Replace("/home/image", "");
		//	string contentType = "";
		//	var filePath = new FileInfo(Server.MapPath("~/App_Data") + filename);

		//	var index = filename.LastIndexOf(".") + 1;
		//	var extension = filename.Substring(index).ToUpperInvariant();

		//	// Fix for IE not handling jpg image types
		//	contentType = string.Compare(extension, "JPG") == 0 ? "image/jpeg" : string.Format("image/{0}", extension);

		//	return File(filePath.FullName, contentType);
		//}


    }
}
