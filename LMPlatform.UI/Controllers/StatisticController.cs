using System.Text;
using System.Web.Mvc;
using Application.Core;
using Application.Core.SLExcel;
using Application.Infrastructure.GroupManagement;
using Application.Infrastructure.LecturerManagement;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.UI.Services.Labs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

using Application.Infrastructure.FilesManagement;
using Application.Infrastructure.StudentManagement;
using LMPlatform.PlagiarismNet.Controllers;
using LMPlatform.UI.PlagiateReference;
using LMPlatform.UI.Services.Modules;

namespace LMPlatform.UI.Controllers
{
	public class StatisticController : Controller
	{
		private readonly LazyDependency<IGroupManagementService> groupManagementService = new LazyDependency<IGroupManagementService>();

		public IGroupManagementService GroupManagementService
		{
			get
			{
				return groupManagementService.Value;
			}
		}

		private readonly LazyDependency<ILecturerManagementService> lecturerManagementService = new LazyDependency<ILecturerManagementService>();

		public ILecturerManagementService LecturerManagementService
		{
			get
			{
				return lecturerManagementService.Value;
			}
		}

		public void GetVisitLecture(int subjectId, int groupId)
		{
			var data = new SLExcelData();

			var headerData = LecturerManagementService.GetLecturesScheduleVisitings(subjectId);
			var rowsData = LecturerManagementService.GetLecturesScheduleMarks(subjectId, groupId);

			data.Headers.Add("Студент");
			data.Headers.AddRange(headerData);
			data.DataRows.AddRange(rowsData);
			
			var file = (new SLExcelWriter()).GenerateExcel(data);

			Response.Clear();
			Response.Charset = "ru-ru";
			Response.HeaderEncoding = Encoding.UTF8;
			Response.ContentEncoding = Encoding.UTF8;
			Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
			Response.AddHeader("Content-Disposition", "attachment; filename=LectureVisiting.xlsx");
			Response.BinaryWrite(file);
			Response.Flush();
			Response.End();
		}

        public void GetVisitCP(int subjectId, int groupId)
        {
            var data = new SLExcelData();

            var headerData = GroupManagementService.GetCpScheduleVisitings(subjectId, groupId);
            var rowsData = GroupManagementService.GetCpScheduleMarks(subjectId, groupId);

            data.Headers.Add("Студент");
            data.Headers.AddRange(headerData);
            data.DataRows.AddRange(rowsData);

            var file = (new SLExcelWriter()).GenerateExcel(data);

            Response.Clear();
            Response.Charset = "ru-ru";
            Response.HeaderEncoding = Encoding.UTF8;
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=CPVisiting.xlsx");
            Response.BinaryWrite(file);
            Response.Flush();
            Response.End();
        }

        public void GetPercentageCP(int subjectId, int groupId)
        {
            var data = new SLExcelData();

            var headerData = GroupManagementService.GetCpPercentage(subjectId, groupId);
            var rowsData = GroupManagementService.GetCpMarks(subjectId, groupId);

            data.Headers.Add("Студент");
            data.Headers.AddRange(headerData);
            data.Headers.Add("Оценка");
            data.DataRows.AddRange(rowsData);

            var file = (new SLExcelWriter()).GenerateExcel(data);

            Response.Clear();
            Response.Charset = "ru-ru";
            Response.HeaderEncoding = Encoding.UTF8;
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=PercentageCP.xlsx");
            Response.BinaryWrite(file);
            Response.Flush();
            Response.End();
        }

        public void GetVisitLabs(int subjectId, int groupId, int subGroupOneId, int subGroupTwoId)
        {
            var data = new SLExcelData();

            var rowsDataOne = GroupManagementService.GetLabsScheduleMarks(subjectId, groupId, subGroupOneId, subGroupTwoId);

            data.DataRows.AddRange(rowsDataOne);

            var file = (new SLExcelWriter()).GenerateExcel(data);

            Response.Clear();
            Response.Charset = "ru-ru";
            Response.HeaderEncoding = Encoding.UTF8;
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=LabVisiting.xlsx");
            Response.BinaryWrite(file);
            Response.Flush();
            Response.End();
        }

        public void GetLabsMarks(int subjectId, int groupId)
        {
            var data = new SLExcelData();

            var headerData = GroupManagementService.GetLabsNames(subjectId, groupId);
            var rowsData = GroupManagementService.GetLabsMarks(subjectId, groupId);

            data.Headers.Add("Студент");
            data.Headers.AddRange(headerData);
            data.DataRows.AddRange(rowsData);

            var file = (new SLExcelWriter()).GenerateExcel(data);

            Response.Clear();
            Response.Charset = "ru-ru";
            Response.HeaderEncoding = Encoding.UTF8;
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=LabMarks.xlsx");
            Response.BinaryWrite(file);
            Response.Flush();
            Response.End();
        }

		private readonly LazyDependency<ISubjectManagementService> subjectManagementService = new LazyDependency<ISubjectManagementService>();

		public ISubjectManagementService SubjectManagementService
		{
			get
			{
				return subjectManagementService.Value;
			}
		}

		public string PlagiarismUrl
		{
			get { return ConfigurationManager.AppSettings["PlagiarismUrl"]; }
		}

		public string PlagiarismTempPath
		{
			get { return ConfigurationManager.AppSettings["PlagiarismTempPath"]; }
		}

		public string FileUploadPath
		{
			get { return ConfigurationManager.AppSettings["FileUploadPath"]; }
		}

		private readonly LazyDependency<IFilesManagementService> filesManagementService = new LazyDependency<IFilesManagementService>();

		public IFilesManagementService FilesManagementService
		{
			get
			{
				return filesManagementService.Value;
			}
		}

		private readonly LazyDependency<IStudentManagementService> studentManagementService = new LazyDependency<IStudentManagementService>();

		public IStudentManagementService StudentManagementService
		{
			get
			{
				return studentManagementService.Value;
			}
		}

		public void ExportPlagiarism(int subjectId, bool isCp = false)
        {
			
			var usersFiles = this.SubjectManagementService.GetUserLabFiles(0, subjectId).Where(e => e.IsReceived && e.IsCoursProject == isCp);

			var filesPaths = usersFiles.Select(e => e.Attachments);

			var key = 0;

			foreach (var filesPath in filesPaths)
			{
				key += filesPath.GetHashCode();
			}

			var sessionData = Session[key.ToString()] as IEnumerable<ResultPlagSubject>;
			
			var data = new ResultPlagSubjectClu
			{
				clusters = sessionData.ToArray()
			};

			var dataE = new SLExcelData();
			dataE.Headers.Add("");
			dataE.Headers.Add("Автор");
			dataE.Headers.Add("Группа");
			dataE.Headers.Add("Предмет");
			dataE.Headers.Add("Файл");
			var i = 1;
			foreach (var resultPlagSubject in data.clusters.ToList())
			{
				dataE.DataRows.Add(new List<string>() { "Кластер " + i });

				var listRows = new List<List<string>>();

				foreach (var correctDoc in resultPlagSubject.correctDocs)
				{
					var list = new List<string> { "", correctDoc.author, correctDoc.groupName, correctDoc.subjectName, correctDoc.doc };
					listRows.Add(list);
				}

				dataE.DataRows.AddRange(listRows);

				i += 1;
			}

			var file = (new SLExcelWriter()).GenerateExcel(dataE);

            Response.Clear();
            Response.Charset = "ru-ru";
            Response.HeaderEncoding = Encoding.UTF8;
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
			Response.AddHeader("Content-Disposition", "attachment; filename=PlagiarismResults.xlsx");
            Response.BinaryWrite(file);
            Response.Flush();
            Response.End();
        }

		public void ExportPlagiarismStudent(string userFileId, string subjectId, bool isCp = false)
        {
			var userFile = this.SubjectManagementService.GetUserLabFile(Int32.Parse(userFileId));

			var usersFiles = this.SubjectManagementService.GetUserLabFiles(0, Int32.Parse(subjectId)).Where(e => e.IsReceived && e.Id != userFile.Id && e.IsCoursProject == isCp);

			var filesPaths = usersFiles.Select(e => e.Attachments);

			var key = 0;

			foreach (var filesPath in filesPaths)
			{
				key += filesPath.GetHashCode();
			}			

			var data = Session[key.ToString()] as IEnumerable<ResultPlag>;

			var dataE = new SLExcelData();
			dataE.Headers.Add("");
			dataE.Headers.Add("Процент схожести, %");
			dataE.Headers.Add("Автор");
			dataE.Headers.Add("Группа");
			dataE.Headers.Add("Предмет");
			dataE.Headers.Add("Файл");
			var listRows = new List<List<string>>();
			foreach (var resultPlagSubject in data)
			{
				var listRow = new List<string>()
									{
										"",
										resultPlagSubject.coeff,
										resultPlagSubject.author,
										resultPlagSubject.groupName,
										resultPlagSubject.subjectName,
										resultPlagSubject.doc
									};
				listRows.Add(listRow);
			}

			dataE.DataRows.AddRange(listRows);

			var file = (new SLExcelWriter()).GenerateExcel(dataE);

            Response.Clear();
            Response.Charset = "ru-ru";
            Response.HeaderEncoding = Encoding.UTF8;
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
			Response.AddHeader("Content-Disposition", "attachment; filename=PlagiarismResults.xlsx");
            Response.BinaryWrite(file);
            Response.Flush();
            Response.End();
        }
	}
}