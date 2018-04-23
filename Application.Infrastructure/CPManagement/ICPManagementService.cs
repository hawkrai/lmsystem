using Application.Core.Data;
using Application.Infrastructure.CTO;
using LMPlatform.Models;
using LMPlatform.Models.CP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Infrastructure.CPManagement
{
    public interface ICPManagementService
    {
        PagedList<CourseProjectData> GetProjects(int userId, GetPagedListParams parms);

        CourseProjectData GetProject(int id);

        List<NewsData> GetNewses(int userId, int subjectId);

        CourseProjectNews GetNews(int id);

        void DeleteNews(CourseProjectNews news);

        CourseProjectNews SaveNews(CourseProjectNews news, IList<Attachment> attachments, Int32 userId);

        List<Correlation> GetGroups(int subjectId);

        SubjectData GetSubject(int id);

        void DeleteProject(int userId, int id);

        void AssignProject(int userId, int projectId, int studentId);

        void DeleteAssignment(int userId, int id);

        PagedList<StudentData> GetStudentsByCourseProjectId(GetPagedListParams parms);

        PagedList<StudentData> GetGraduateStudentsForUser(int userId, int subjectId, GetPagedListParams parms, bool getBySecretaryForStudent = true);

        PagedList<StudentData> GetGraduateStudentsForGroup(int userId, int groupId, int subjectId, GetPagedListParams parms, bool getBySecretaryForStudent = true);

        void DisableNews(int subjectId, bool disable);

        void SaveProject(CourseProjectData projectData);

        void SetStudentDiplomMark(int lecturerId, int assignedProjectId, int mark);

        CourseProjectTaskSheetTemplate GetTaskSheetTemplate(int id);

        void SaveTaskSheetTemplate(CourseProjectTaskSheetTemplate template);

        void DeleteUserFromAcpProject(int id, int projectId);

        void DeletePercenageAndVisitStatsForUser(int id);

        TaskSheetData GetTaskSheet(int courseProjectId);

        void SaveTaskSheet(int userId, TaskSheetData taskSheet);

        void SetSelectedGroupsToCourseProjects(int subjectId, List<int> groupIds);

    }
}
