using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMPlatform.UI.Controllers
{
    public class MainController : Controller
    {
        //
        // GET: /Main/

        public ActionResult Index()
        {
            return View();
        }

		public FileResult DownloadApk()
		{
			var filePath = Server.MapPath("/lms_1_0.apk");
			byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
			var response = new FileContentResult(fileBytes, "application/vnd.android.package-archive");
			response.FileDownloadName = "lms_1_0.apk";

			return response;
		}

    }
}
