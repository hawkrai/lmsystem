using System;
using System.Collections.Generic;
using Application.Core.Data;
using Application.Infrastructure.DTO;

namespace Application.Infrastructure.DPManagement
{
    public interface IPercentageGraphService
    {
        PagedList<PercentageGraphData> GetPercentageGraphs(int userId, GetPagedListParams parms);

        PagedList<PercentageGraphData> GetPercentageGraphsForLecturer(int lecturerId, GetPagedListParams parms, int secretaryId);

        List<PercentageGraphData> GetPercentageGraphsForLecturerAll(int userId, GetPagedListParams parms);

        List<DiplomProjectConsultationDateData> GetConsultationDatesForUser(int userId);

        void SavePercentage(int userId, PercentageGraphData percentageData);

        PercentageGraphData GetPercentageGraph(int id);

        void DeletePercentage(int userId, int id);

        void SavePercentageResult(int userId, PercentageResultData percentageResultData);

        void SaveConsultationMark(int userId, DipomProjectConsultationMarkData consultationMarkData);

        void SaveConsultationDate(int userId, DateTime date);

        void DeleteConsultationDate(int userId, int id);
    }
}
