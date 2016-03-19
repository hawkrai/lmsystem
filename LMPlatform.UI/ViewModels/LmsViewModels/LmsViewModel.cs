using System.Collections.Generic;
using System.Linq;
using Application.Core;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.UI.ViewModels.SubjectViewModels;

namespace LMPlatform.UI.ViewModels.LmsViewModels
{
    using Application.Infrastructure.StudentManagement;

    using LMPlatform.UI.ViewModels.AdministrationViewModels;
    using Application.Infrastructure.ConceptManagement;
    using LMPlatform.UI.ViewModels.ComplexMaterialsViewModel;
    using LMPlatform.Models;

    public class LmsViewModel
    {
        private readonly LazyDependency<ISubjectManagementService> _subjectManagementService = new LazyDependency<ISubjectManagementService>();
        private readonly LazyDependency<IStudentManagementService> _studentManagementService = new LazyDependency<IStudentManagementService>();
        private readonly LazyDependency<IModulesManagementService> _modulesManagementService = new LazyDependency<IModulesManagementService>();


        public ISubjectManagementService SubjectManagementService
        {
            get
            {
                return _subjectManagementService.Value;
            }
        }

        public IStudentManagementService StudentManagementService
        {
            get
            {
                return _studentManagementService.Value;
            }
        }

        public IModulesManagementService ModulesManagementService
        {
            get
            {
                return _modulesManagementService.Value;
            }
        }


        public List<SubjectViewModel> Subjects
        {
            get;
            set;
        }

        public List<SubjectViewModel> ComplexSubjects { get; set; }

        public int TotalSubject { get; set; }

        public int CurrentSubjects { get; set; }

        public int TotalStudents { get; set; }

        public int CurrentStudents { get; set; }

        public UserActivityViewModel UserActivity { get; set; }

        public LmsViewModel(int userId, bool isLector)
        {
            var s = SubjectManagementService.GetUserSubjects(userId).Where(e => !e.IsArchive);
            Subjects = s.Select(e => new SubjectViewModel(e)).ToList();
            CurrentSubjects = Subjects.Count();
            TotalSubject = SubjectManagementService.GetSubjects().Count();
            ComplexSubjects = s
                .Where(cs => ModulesManagementService.GetModules(cs.Id).Any(m=>m.ModuleType == ModuleType.ComplexMaterial))
                .Select(e => new SubjectViewModel(e)).ToList();

            var modelStudents = new List<int>();
            CurrentStudents = 0;

            if (isLector)
            {

                TotalStudents = StudentManagementService.GetStudents().Count();

                foreach (var subjects in SubjectManagementService.GetUserSubjects(userId))
                {
                    if (subjects.SubjectGroups != null)
                    {
                        foreach (var group in subjects.SubjectGroups)
                        {
                            foreach (var student in group.SubjectStudents)
                            {
                                if (modelStudents.All(e => e != student.StudentId))
                                {
                                    modelStudents.Add(student.StudentId);
                                    CurrentStudents += 1;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}