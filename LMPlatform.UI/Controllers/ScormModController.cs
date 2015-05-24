using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Core.UI.Controllers;

namespace LMPlatform.UI.Controllers
{
	public class ScormModController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        private string path_1 = @"\ScormObjects\Sco_1\index.html";
        private string path_2 = @"\ScormObjects\Sco_2\index.html";
        private string path_3 = @"\ScormObjects\Sco_3\index.html";

	    public ActionResult GetObjects()
	    {
	        var result = new List<dynamic>
	        {
	            new
	            {
	                name = "Первый объект",
	                url = path_1
	            },
	            new
	            {
	                name = "Второй объект",
	                url = path_2
	            },
	            new
	            {
	                name = "Третий объект",
	                url = path_3
	            }
	        };

            return Json(result, JsonRequestBehavior.AllowGet);
	    }
    }
}
