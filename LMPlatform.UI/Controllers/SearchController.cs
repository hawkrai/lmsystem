using System.Web.Mvc;
using Application.Core;
using Application.Core.UI.Controllers;
using Application.Infrastructure.GroupManagement;
using Application.Infrastructure.LecturerManagement;
using Application.Infrastructure.ProjectManagement;
using Application.Infrastructure.StudentManagement;
using Application.SearchEngine.SearchMethods;
using LMPlatform.UI.ViewModels.SearchViewModel;

namespace LMPlatform.UI.Controllers
{
    public class SearchController : BasicController
    {
        private readonly LazyDependency<IStudentManagementService> _studentRepository = new LazyDependency<IStudentManagementService>();
        private readonly LazyDependency<IGroupManagementService> _groupRepository = new LazyDependency<IGroupManagementService>();
        private readonly LazyDependency<ILecturerManagementService> _lecturerRepository = new LazyDependency<ILecturerManagementService>();
        private readonly LazyDependency<IProjectManagementService> _projectRepository = new LazyDependency<IProjectManagementService>();

        public ActionResult Index(string query)
        {
            if (string.IsNullOrEmpty(query))
                return RedirectToAction("Index", "Lms");

            var model = new SearchViewModel();

            var ssm = new StudentSearchMethod();
            if (!ssm.IsIndexExist())
                ssm.AddToIndex(_studentRepository.Value.GetStudents());
            model.Students = ssm.Search(query);

            var gSearchMethod = new GroupSearchMethod();
            if (!gSearchMethod.IsIndexExist())
                gSearchMethod.AddToIndex(_groupRepository.Value.GetGroups());
            model.Groups = gSearchMethod.Search(query);

            var psm = new ProjectSearchMethod();
            if (!psm.IsIndexExist())
                psm.AddToIndex(_projectRepository.Value.GetProjects());
            model.Projects = psm.Search(query);

            var lsm = new LecturerSearchMethod();
            if (!lsm.IsIndexExist())
                lsm.AddToIndex(_lecturerRepository.Value.GetLecturers());
            model.Lecturers = lsm.Search(query);

            return View(model);
        }

    }
}
