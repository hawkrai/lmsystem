using System.Collections.Generic;
using Application.Core.Data;
using Application.Infrastructure.DTO;
using LMPlatform.Models.DP;
using LMPlatform.Models;
using System;

namespace Application.Infrastructure.DPManagement
{
    public interface IDpManagementService
    {
        PagedList<DiplomProjectData> GetProjects(int userId, GetPagedListParams parms);

        DiplomProjectData GetProject(int id);

        void SaveProject(DiplomProjectData projectData);

        TaskSheetData GetTaskSheet(int diplomProjectId);

        void SaveTaskSheet(int userId, TaskSheetData taskSheet);

        void DeleteProject(int userId, int id);

        void AssignProject(int userId, int projectId, int studentId);

        void SetStudentDiplomMark(int lecturerId, int assignedProjectId, int mark);

        void DeleteAssignment(int userId, int id);

        PagedList<StudentData> GetStudentsByDiplomProjectId(GetPagedListParams parms);

        PagedList<StudentData> GetGraduateStudentsForUser(int userId, GetPagedListParams parms, bool getBySecretaryForStudent = true);

        bool ShowDpSectionForUser(int userId);

        DiplomProjectTaskSheetTemplate GetTaskSheetTemplate(int id);
        
        void SaveTaskSheetTemplate(DiplomProjectTaskSheetTemplate template);

        List<NewsData> GetNewses(int userId);

        DiplomProjectNews GetNews(int id);

        void DeleteNews(DiplomProjectNews news);

        DiplomProjectNews SaveNews(DiplomProjectNews news, IList<Attachment> attachments, Int32 userId);

        void DisableNews(int lecturerId, bool disable);

    }
}
