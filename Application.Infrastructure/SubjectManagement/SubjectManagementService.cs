using System.Collections.Generic;
using Application.Core;
using Application.Infrastructure.StudentManagement;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;

namespace Application.Infrastructure.SubjectManagement
{
    public class SubjectManagementService : ISubjectManagementService
    {
        private readonly LazyDependency<ISubjectRepository> _subjectRepository = new LazyDependency<ISubjectRepository>();

        public ISubjectRepository SubjectRepository
        {
            get
            {
                return _subjectRepository.Value;
            }
        }

        private readonly LazyDependency<IStudentManagementService> _studentManagementService = new LazyDependency<IStudentManagementService>();

        public IStudentManagementService StudentManagementService
        {
            get
            {
                return _studentManagementService.Value;
            }
        }
        
        public List<Subject> GetUserSubjects(int userId)
        {
            var student = StudentManagementService.GetStudent(userId);
            return SubjectRepository.GetSubjects(student.Group.Id);
        }

        public Subject GetSubject(int id)
        {
            return SubjectRepository.GetSingle(new Subject
            {
                Id = id
            });
        }
    }
}