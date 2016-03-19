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

    public class LmsViewModel
    {
        private readonly LazyDependency<ISubjectManagementService> _subjectManagementService = new LazyDependency<ISubjectManagementService>();
        private readonly LazyDependency<IStudentManagementService> _studentManagementService = new LazyDependency<IStudentManagementService>();
        private readonly LazyDependency<IConceptManagementService> _conceptManagementService = new LazyDependency<IConceptManagementService>();

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

        public IConceptManagementService ConceptManagementService
        {
            get
            {
                return _conceptManagementService.Value;
            }
        }

        public List<SubjectViewModel> Subjects
        {
            get;
            set;
        }

        public List<AddOrEditRootConceptViewModel> ComplexMaterials { get; set; }

        public int TotalSubject { get; set; }

        public int CurrentSubjects { get; set; }

        public int TotalStudents { get; set; }

        public int CurrentStudents { get; set; }

        public UserActivityViewModel UserActivity { get; set; }

        public LmsViewModel(int userId, bool isLector)
        {
            Subjects = SubjectManagementService.GetUserSubjects(userId).Where(e => !e.IsArchive).Select(e => new SubjectViewModel(e)).ToList();
            CurrentSubjects = Subjects.Count();
            TotalSubject = SubjectManagementService.GetSubjects().Count();
            ComplexMaterials = new List<AddOrEditRootConceptViewModel>();
            
            var modelStudents = new List<int>();
            CurrentStudents = 0;
            
            if (isLector)
            {
                TotalStudents = StudentManagementService.GetStudents().Count();
                var complexMaterials = ConceptManagementService.GetRootElements(userId).Select(e=>new AddOrEditRootConceptViewModel(userId, e.Id));
                ComplexMaterials.AddRange(complexMaterials);
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