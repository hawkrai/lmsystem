using System.Collections.Generic;
using System.ServiceModel;
using LMPlatform.UI.Services.Modules.Lectures;
using System.ServiceModel.Web;

namespace LMPlatform.UI.Services.Lectures
{
	using Modules;
 
	[ServiceContract]
    public interface ILecturesService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetLectures/{subjectId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
        LecturesResult GetLectures(string subjectId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetCalendar/{subjectId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
        CalendarResult GetCalendar(string subjectId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Save")]
        ResultViewData Save(int subjectId, int id, string theme, int duration, int order, string pathFile, string attachments);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Delete")]
        ResultViewData Delete(int id, int subjectId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/SaveDateLectures")]
        ResultViewData SaveDateLectures(int subjectId, string date);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetMarksCalendarData")]
        StudentMarkForDateResult GetMarksCalendarData(int dateId, int subjectId, int groupId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/SaveMarksCalendarData")]
        ResultViewData SaveMarksCalendarData(List<LecturesMarkVisitingViewData> lecturesMarks);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/DeleteVisitingDates")]
		ResultViewData DeleteVisitingDates(List<int> dateIds);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/SaveMarksCalendarDataSingle")]
		ResultViewData SaveMarksCalendarDataSingle(int markId, string mark, int lecuresVisitId, int studentId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/DeleteVisitingDate")]
        ResultViewData DeleteVisitingDate(int id);
    }
}
