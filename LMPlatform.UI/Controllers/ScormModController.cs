using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Core.Data;
using Application.Core.UI.Controllers;
using Ionic.Zip;
using LMPlatform.Data.Repositories;
using LMPlatform.Models;

namespace LMPlatform.UI.Controllers
{
    using SCORMHost;

    public class ScormModController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public string ScoFilePath
        {
            get { return ConfigurationManager.AppSettings["ScoFilePath"]; }
        }

	    public ActionResult GetObjects()
	    {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return Json(repositoriesContainer.RepositoryFor<ScoObjects>().GetAll(new Query<ScoObjects>()).ToList(), JsonRequestBehavior.AllowGet);
	        }
	    }

        public ActionResult LoadObject(string name, HttpPostedFileBase file)
	    {
	        var guid = Guid.NewGuid().ToString();
            file.SaveAs(ScoFilePath + "\\" + guid + ".zip");

            using (ZipFile zip = ZipFile.Read(ScoFilePath + "\\" + guid + ".zip"))
            {
                Directory.CreateDirectory(ScoFilePath + "\\" + guid);
                zip.ExtractAll(ScoFilePath + "\\" + guid, ExtractExistingFileAction.OverwriteSilently);
            }

            System.IO.File.Delete(ScoFilePath + "\\" + guid + ".zip");

            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.RepositoryFor<ScoObjects>().Save(new ScoObjects()
                {
                    Name = name,
                    Path = guid
                });
                repositoriesContainer.ApplyChanges();
            }

	        return Json(null);
	    }

	    public ActionResult ViewSco(string path)
	    {
	        var fileImsmanifestPath = ScoFilePath + "\\" + path + "\\" + "imsmanifest.xml";
	        var scorm = new Scorm();
	        var fileXml = new FileInfo(fileImsmanifestPath);
            scorm.OpenImsManifest(fileXml);
            return Json(scorm.TreeActivity, JsonRequestBehavior.AllowGet);
	    }
    }
}
