using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PluginsTutorial.Web.Controllers
{
	public class JQueryFileUploadController : Controller
	{

		public ActionResult Basic()
		{
			return View("~/Views/FileUpload/jQueryFileUpload/Basic.cshtml");
		}




	}
}
