using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Application.Infrastructure.Export;
using LMPlatform.Data.Infrastructure;

namespace LMPlatform.UI.Controllers
{
    public class DpController : Controller
    {
        // GET: /Dp/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Projects()
        {
            return PartialView();
        }

        public ActionResult Project()
        {
            return PartialView();
        }

        public ActionResult Students()
        {
            return PartialView();
        }

        public ActionResult TaskSheet()
        {
            return PartialView();
        }

        public ActionResult Percentages()
        {
            return PartialView();
        }

        public ActionResult Percentage()
        {
            return PartialView();
        }

        public ActionResult PercentageResults()
        {
            return PartialView();
        }

        public ActionResult VisitStats()
        {
            return PartialView();
        }

        public ActionResult ConsultationDate()
        {
            return PartialView();
        }

        public void GetTasksSheetDocument(int diplomProjectId)
        {
            //todo
            var diplomProject =
                new LmPlatformModelsContext().DiplomProjects
                .Include(x => x.AssignedDiplomProjects.Select(y => y.Student.Group))
                .Single(x => x.DiplomProjectId == diplomProjectId);

            string docName;
            if (diplomProject.AssignedDiplomProjects.Count == 1)
            {
                var stud = diplomProject.AssignedDiplomProjects.Single().Student;
                docName = string.Format("{0}_{1}", stud.LastName, stud.FirstName);
            }
            else
            {
                docName = string.Format("{0}", diplomProject.Theme);
            }

            Word.DiplomProjectToWord(docName, diplomProject, Response);
        }
        
        public string GetTasksSheetHtml(int diplomProjectId)
        {
            //todo
            var diplomProject =
                new LmPlatformModelsContext().DiplomProjects
                .Include(x => x.AssignedDiplomProjects.Select(y => y.Student.Group))
                .Single(x => x.DiplomProjectId == diplomProjectId);

            return diplomProject.AssignedDiplomProjects.Count == 1 ? 
                Word.DiplomProjectToDocView(diplomProject.AssignedDiplomProjects.First()) : 
                Word.DiplomProjectToDocView(diplomProject);
        }

        public ActionResult TaskSheetEdit()
        {
            return PartialView();
        }
    }
}
