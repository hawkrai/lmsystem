using System.Collections.Generic;
using Application.Core.Data;
using LMPlatform.Models;

namespace Application.Infrastructure.GroupManagement
{
    public interface IGroupManagementService
    {
        Group GetGroup(int groupId);

        List<Group> GetGroups(IQuery<Group> query = null);

        IPageableList<Group> GetGroupsPageable(string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null);

        Group AddGroup(Group group);

        Group UpdateGroup(Group lecturer);

        void DeleteGroup(int id);

	    List<string> GetLabsScheduleVisitings(int subjectId, int groupId, int subGorupId);

		List<List<string>> GetLabsScheduleMarks(int subjectId, int groupId, int subGroupOneId, int subGroupTwoId);

        List<string> GetCpScheduleVisitings(int subjectId, int groupId);

        List<List<string>> GetCpScheduleMarks(int subjectId, int groupId);

        List<string> GetCpPercentage(int subjectId, int groupId);

        List<List<string>> GetCpMarks(int subjectId, int groupId);

        Group GetGroupByName(string groupName);

        List<string> GetLabsNames(int subjectId, int groupId);

        List<List<string>> GetLabsMarks(int subjectId, int groupId);

		List<Group> GetLecturesGroups(int id);
    }
}