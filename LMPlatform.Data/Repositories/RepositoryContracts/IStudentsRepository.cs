using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace LMPlatform.Data.Repositories.RepositoryContracts
{
    using Application.Core.Data;

    using LMPlatform.Models;

    public interface IStudentsRepository : IRepositoryBase<Student>
    {
        Student GetStudent(int id);

        List<Student> GetStudents(int groupId);

        List<Student> GetStudents();
    }
}