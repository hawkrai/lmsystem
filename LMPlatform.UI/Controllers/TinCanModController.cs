using Application.Core;
using Application.Infrastructure.TinCanManagement;
using Ionic.Zip;
using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace LMPlatform.UI.Controllers
{
	public class TinCanModController : Controller
    {
        private readonly LazyDependency<ITinCanManagementService> tinCanManagementService = new LazyDependency<ITinCanManagementService>();

        public ITinCanManagementService TinCanManagementService
        {
            get
            {
                return tinCanManagementService.Value;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

		public string TinCanFilePath
		{
			get { return ConfigurationManager.AppSettings["TinCanFilePath"]; }
		}
		public ActionResult GetObjects()
		{
            return Json(TinCanManagementService.GetAllTinCanObjects(), JsonRequestBehavior.AllowGet);
		}

		public ActionResult LoadObject(string name, HttpPostedFileBase file)
		{
			var guid = Guid.NewGuid().ToString();
			file.SaveAs(TinCanFilePath + "\\" + guid + ".zip");

			using (ZipFile zip = ZipFile.Read(TinCanFilePath + "\\" + guid + ".zip"))
			{
				Directory.CreateDirectory(TinCanFilePath + "\\" + guid);
				zip.ExtractAll(TinCanFilePath + "\\" + guid, ExtractExistingFileAction.OverwriteSilently);
			}

			if (!System.IO.File.Exists(TinCanFilePath + "\\" + guid + "\\tincan.xml"))
			{
				return Json(new
				{
					error = "Загруженный файл не является объектом TinCan"
				});
			}
			System.IO.File.Delete(TinCanFilePath + "\\" + guid + ".zip");

            TinCanManagementService.SaveTinCanObject(name, guid);

			return Json(name, JsonRequestBehavior.AllowGet);
			
		}

		public ActionResult DeleteTinCan(int id)
		{
            TinCanManagementService.DeleteTinCanObject(id);
			return Json(id, JsonRequestBehavior.AllowGet);
		}

		public ActionResult ViewTinCan(int id)
		{
            return Json(TinCanManagementService.ViewTinCanObject(TinCanFilePath, id), JsonRequestBehavior.AllowGet);
		}

		public ActionResult EditObject(string name, string path)
		{
            TinCanManagementService.EditTinCanObject(name, path);
			return Json(name, JsonRequestBehavior.AllowGet);
		}

		public ActionResult UpdateObjects(bool enable, int id)
		{
            TinCanManagementService.UpdateTinCanObject(enable, id);
			return Json(enable, JsonRequestBehavior.AllowGet);
		}
    }
}
