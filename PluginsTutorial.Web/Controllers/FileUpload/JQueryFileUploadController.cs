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

        string VirtualFolderPath
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

      
		[HttpPost]
		public ActionResult UploadFiles()
		{
			var fileInfoList = new List<dynamic>();
			
			foreach (string file in Request.Files)
			{
				var hpf = Request.Files[file];
				if (hpf.ContentLength == 0) continue;
				var savedFileName = Path.Combine(Server.MapPath("~/Resources"), Path.GetFileName(hpf.FileName));
				hpf.SaveAs(savedFileName);

				fileInfoList.Add(new 
				{
					Name = hpf.FileName,
                    Url = string.Concat(VirtualFolderPath, "/", hpf.FileName),
					Length = hpf.ContentLength,
					Type = hpf.ContentType,
				});
			}

			//var js = new JavaScriptSerializer();
			//var str = js.Serialize(fileInfoList);
            //return Json(fileInfoList);
            //return Content(str, "application/json");
            //return Content(str);
            
            var result = Json(fileInfoList);
            //for IE9 or less which does not accept application/json
            if (Request.Headers["Accept"] != null && !Request.Headers["Accept"].Contains("application/json"))
                result.ContentType = "text/plain";

            return result;
		}


		//public ActionResult JQueryUI()
		//{
		//	//return View("~/Views/FileUpload/JQueryFileUpload/JQueryUI.cshtml");
		//	//return View("~/Views/FileUpload/JQueryFileUpload/JQueryUI.aspx");
		//	return View("~/Views/FileUpload/JQueryFileUpload/UsingGenericHandler/Basic.cshtml");
		//}


		

		




		//[HttpPost]
		//public ContentResult JQueryUI_UploadFiles()
		//{
		//	Response.ContentType = "text/plain"; //"application/json";
		//	var r = new System.Collections.Generic.List<ViewDataUploadFilesResult>();
		//	JavaScriptSerializer js = new JavaScriptSerializer();

		//	foreach (string file in Request.Files)
		//	{
		//		HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;

		//		//HttpPostedFile hpf = Request.Files[file] as HttpPostedFile;
		//		string FileName = string.Empty;
		//		if (Request.Browser.Browser.ToUpper() == "IE")
		//		{
		//			string[] files = hpf.FileName.Split(new char[] { '\\' });
		//			FileName = files[files.Length - 1];
		//		}
		//		else
		//		{
		//			FileName = hpf.FileName;
		//		}
		//		if (hpf.ContentLength == 0)
		//			continue;

		//		string savedFileName = Path.Combine(Server.MapPath("~/App_Data"), Path.GetFileName(hpf.FileName));
		//		//string savedFileName = Path.Combine(context.Server.MapPath("~/App_Data"), Path.GetFileName(hpf.FileName));
		//		hpf.SaveAs(savedFileName);

		//		r.Add(new ViewDataUploadFilesResult()
		//		{
		//			//Thumbnail_url = savedFileName,
		//			Name = FileName,
		//			Length = hpf.ContentLength,
		//			Type = hpf.ContentType
		//		});

		//		var uploadedFiles = new
		//		{
		//			files = r.ToArray()
		//		};

		//		var jsonObj = js.Serialize(uploadedFiles);
				
		//		//context.Response.Write(jsonObj.ToString());
		//		//return Content("{\"name\":\"" + r[0].Name + "\",\"type\":\"" + r[0].Type + "\",\"size\":\"" + string.Format("{0} bytes", r[0].Length) + "\"}", "application/json");
		//	}


		//}






		//not used
		public FilePathResult Image()
		{
			string filename = Request.Url.AbsolutePath.Replace("/home/image", "");
			string contentType = "";
			var filePath = new FileInfo(Server.MapPath("~/App_Data") + filename);

			var index = filename.LastIndexOf(".") + 1;
			var extension = filename.Substring(index).ToUpperInvariant();

			// Fix for IE not handling jpg image types
			contentType = string.Compare(extension, "JPG") == 0 ? "image/jpeg" : string.Format("image/{0}", extension);

			return File(filePath.FullName, contentType);
		}




















    }
}
