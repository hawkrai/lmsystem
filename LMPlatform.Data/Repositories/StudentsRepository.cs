using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LMPlatform.Data.Repositories
{
    using Application.Core.Data;

    using LMPlatform.Data.Infrastructure;
    using LMPlatform.Data.Repositories.RepositoryContracts;
    using LMPlatform.Models;

    public class StudentsRepository : RepositoryBase<LmPlatformModelsContext, Student>, IStudentsRepository
    {
        public StudentsRepository(LmPlatformModelsContext dataContext) : base(dataContext)
        {
        }

        public Student GetStudent(int id)
        {
            using (var context = new LmPlatformModelsContext())
            {
                var student = context.Set<Student>().Include(e => e.Group).FirstOrDefault(e => e.Id == id);
                return student;
            }
        }

        public List<Student> GetStudents(int groupId)
        {
            using (var context = new LmPlatformModelsContext())
            {
                var students = context.Set<Student>().Include(e => e.Group).Where(e => e.GroupId == groupId).ToList();
                return students;
            } 
        }
    }
}