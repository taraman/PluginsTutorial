using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PluginsTutorial.Services;

namespace PluginsTutorial.Web.Controllers
{
    public class HomeController : Controller
    {
		private readonly IService _service;

		public HomeController(IService service)
		{
			_service = service;
		}

        public ActionResult Index()
        {
            return View();
        }


		//public ActionResult Test()
		//{
		//	return View();
		//}


		//public ActionResult FileUpload()
		//{
		//	return View("~/Views/FileUpload/Index.cshtml");
		//}

		
		public JsonResult GetAllProductsWithCategoriesOrdredByProductName()
		{
			var products = _service.GetAllProductsWithCategoriesOrdredByProductName();
			var data = products.Select(x => new
			{
				x.ProductId,
				x.ProductName,
				Category = new
				{
					x.Category.CategoryId,
					x.Category.CategoryName
				}
			}).ToList();
			return Json(data, JsonRequestBehavior.AllowGet);
		}

    }
}
