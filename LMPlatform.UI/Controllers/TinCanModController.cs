using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Application.Core.Data;
using Ionic.Zip;
using LMPlatform.Data.Repositories;
using LMPlatform.Models;

namespace LMPlatform.UI.Controllers
{
	public class TinCanModController : Controller
    {
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

			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				return Json(repositoriesContainer.RepositoryFor<TinCanObjects>().GetAll(new Query<TinCanObjects>(e => !e.IsDeleted)).ToList(), JsonRequestBehavior.AllowGet);

			}
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

			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				repositoriesContainer.RepositoryFor<TinCanObjects>().Save(new TinCanObjects()
				{
					Name = name,
					Path = guid,
					Enabled = false,
					IsDeleted = false
				});
				repositoriesContainer.ApplyChanges();
			}

			return Json(name, JsonRequestBehavior.AllowGet);
			
		}

		public ActionResult DeleteTinCan(int id)
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var data = repositoriesContainer.RepositoryFor<TinCanObjects>().GetBy(new Query<TinCanObjects>(e => e.Id == id));
				data.IsDeleted = true;
				repositoriesContainer.RepositoryFor<TinCanObjects>().Save(data);
				repositoriesContainer.ApplyChanges();
			}
			return Json(id, JsonRequestBehavior.AllowGet);
		}

		public ActionResult ViewTinCan(int id)
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var data = repositoriesContainer.RepositoryFor<TinCanObjects>().GetBy(new Query<TinCanObjects>(e => e.Id == id));
				var tincanPath = TinCanFilePath + "//" + data.Path + "//" + "tincan.xml";
				var dirName = new FileInfo(TinCanFilePath).Directory.Name;
				XDocument xdoc = XDocument.Load(tincanPath);
				var name = dirName + "/" + data.Path + "/" + xdoc.Descendants(XName.Get("launch", @"http://projecttincan.com/tincan.xsd")).First().Value;
				return Json(name, JsonRequestBehavior.AllowGet);
			}
		}

		public ActionResult EditObject(string name, string path)
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var data = repositoriesContainer.RepositoryFor<TinCanObjects>().GetBy(new Query<TinCanObjects>(e => e.Path == path));
				data.Name = name;
				repositoriesContainer.RepositoryFor<TinCanObjects>().Save(data);
				repositoriesContainer.ApplyChanges();
			}
			return Json(name, JsonRequestBehavior.AllowGet);
		}

		public ActionResult UpdateObjects(bool enable, int id)
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var data = repositoriesContainer.RepositoryFor<TinCanObjects>().GetBy(new Query<TinCanObjects>(e => e.Id == id));
				data.Enabled = enable;
				repositoriesContainer.RepositoryFor<TinCanObjects>().Save(data);
				repositoriesContainer.ApplyChanges();
			}
			return Json(enable, JsonRequestBehavior.AllowGet);
		}
    }
}
