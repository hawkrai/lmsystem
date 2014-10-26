using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LMPlatform.UI.Services.Lectures
{
    using System.ServiceModel.Web;

    using LMPlatform.UI.Services.Modules;
    using LMPlatform.UI.Services.Modules.Lectures;
    using LMPlatform.UI.Services.Modules.News;

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
        ResultViewData Save(string subjectId, string id, string theme, string duration, string order, string pathFile, string attachments);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Delete")]
        ResultViewData Delete(string id, string subjectId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/SaveDateLectures")]
        ResultViewData SaveDateLectures(string subjectId, string date);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetMarksCalendarData")]
        StudentMarkForDateResult GetMarksCalendarData(string dateId, string subjectId, string groupId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/SaveMarksCalendarData")]
        ResultViewData SaveMarksCalendarData(List<LecturesMarkVisitingViewData> lecturesMarks);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/DeleteVisitingDate")]
        ResultViewData DeleteVisitingDate(string id);
    }
}
