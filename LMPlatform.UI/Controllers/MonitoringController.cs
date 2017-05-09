using Application.Core;
using Application.Core.UI.Controllers;
using Application.Infrastructure.ConceptManagement;
using LMPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace LMPlatform.UI.Controllers
{
    public class MonitoringController : Controller
    {
        //
        // GET: /Monitoring/

        public ActionResult Index()
        {
            ViewBag.NgApp = "monitoringApp";
            return View();
        }
    }
}
