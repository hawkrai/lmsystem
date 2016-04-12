using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Application.Infrastructure.Export;
using LMPlatform.Data.Infrastructure;
using Application.Infrastructure.CTO;
using System.Collections.Generic;
using Application.Infrastructure.CPManagement;
using System.Diagnostics.CodeAnalysis;
using Application.Core;

namespace LMPlatform.UI.Controllers
{
    public class CpController : Controller
    {
        //
        // GET: /YE/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Projects()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult Project()
        {
            return PartialView();
        }

        public ActionResult TaskSheet()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult Students()
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

        public ActionResult News()
        {
            return PartialView();
        }

        public void GetTasksSheetDocument(int courseProjectId)
        {
            
            var courseProject =
                new LmPlatformModelsContext().CourseProjects
                .Include(x => x.AssignedCourseProjects.Select(y => y.Student.Group.Secretary.CoursePercentagesGraphs))
                .Single(x => x.CourseProjectId == courseProjectId);

            string docName;
            if (courseProject.AssignedCourseProjects.Count == 1)
            {
                var stud = courseProject.AssignedCourseProjects.Single().Student;
                docName = string.Format("{0}_{1}", stud.LastName, stud.FirstName);
            }
            else
            {
                docName = string.Format("{0}", courseProject.Theme);
            }

             WordCourseProject.CourseProjectToWord(docName, courseProject, Response);
        }
        
        public string GetTasksSheetHtml(int courseProjectId)
        {
            //todo
            var courseProject =
                new LmPlatformModelsContext().CourseProjects
                .Include(x => x.AssignedCourseProjects.Select(y => y.Student.Group.Secretary.CoursePercentagesGraphs))
                .Single(x => x.CourseProjectId == courseProjectId);

           return courseProject.AssignedCourseProjects.Count == 1 ?
                WordCourseProject.CourseProjectToDocView(courseProject.AssignedCourseProjects.First()) :
                WordCourseProject.CourseProjectToDocView(courseProject);
        }

        public ActionResult TaskSheetEdit()
        {
            return PartialView();
        }

    }
}
