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

		public void ExportPlagiarism(int subjectId, int type, int threshold)
        {
			var path = Guid.NewGuid().ToString("N");

			var subjectName = this.SubjectManagementService.GetSubject(subjectId).ShortName;

			Directory.CreateDirectory(this.PlagiarismTempPath + path);

			var usersFiles = this.SubjectManagementService.GetUserLabFiles(0, subjectId).Where(e => e.IsReceived);

			var filesPaths = usersFiles.Select(e => e.Attachments);

			foreach (var filesPath in filesPaths)
			{
				foreach (var srcPath in Directory.GetFiles(this.FileUploadPath + filesPath))
				{
					System.IO.File.Copy(srcPath, srcPath.Replace(this.FileUploadPath + filesPath, this.PlagiarismTempPath + path), true);
				}
			}

			var plagiarismController = new PlagiarismController();
			var result = plagiarismController.CheckByDirectory(new[] { PlagiarismTempPath + path }.ToList(), threshold, 10, type);

			var data = new ResultPlagSubjectClu
			{
				clusters = new ResultPlagSubject[result.Count]
			};

			for (int j = 0; j < result.Count; ++j)
			{
				data.clusters[j] = new ResultPlagSubject
				{
					correctDocs = new List<ResultPlag>()
				};
				foreach (var doc in result[j].Docs)
				{
					var resultS = new ResultPlag();
					var fileName = Path.GetFileName(doc);
					var name = this.FilesManagementService.GetFileDisplayName(fileName);
					resultS.subjectName = subjectName;
					resultS.doc = name;
					var pathName = this.FilesManagementService.GetPathName(fileName);

					var userFileT = this.SubjectManagementService.GetUserLabFile(pathName);

					var userId = userFileT.UserId;

					var user = this.StudentManagementService.GetStudent(userId);

					resultS.author = user.FullName;

					resultS.groupName = user.Group.Name;
					data.clusters[j].correctDocs.Add(resultS);
				}
			}

			Directory.Delete(this.PlagiarismTempPath + path, true);

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

		public void ExportPlagiarismStudent(string userFileId, string subjectId)
        {
			var path = Guid.NewGuid().ToString("N");

			var subjectName = this.SubjectManagementService.GetSubject(Int32.Parse(subjectId)).ShortName;

			Directory.CreateDirectory(this.PlagiarismTempPath + path);

			var userFile = this.SubjectManagementService.GetUserLabFile(Int32.Parse(userFileId));

			var usersFiles = this.SubjectManagementService.GetUserLabFiles(0, Int32.Parse(subjectId)).Where(e => e.IsReceived && e.Id != userFile.Id);

			var filesPaths = usersFiles.Select(e => e.Attachments);

			foreach (var filesPath in filesPaths)
			{
				foreach (var srcPath in Directory.GetFiles(this.FileUploadPath + filesPath))
				{
					System.IO.File.Copy(srcPath, srcPath.Replace(this.FileUploadPath + filesPath, this.PlagiarismTempPath + path), true);
				}
			}

			string firstFileName =
				Directory.GetFiles(this.FileUploadPath + userFile.Attachments)
				.Select(fi => fi)
				.FirstOrDefault();

			var plagiarismController = new PlagiarismController();
			var result = plagiarismController.CheckBySingleDoc(firstFileName, new[] { PlagiarismTempPath + path }.ToList(), 10, 10);

			var data = new List<ResultPlag>();

			foreach (var res in result)
			{
				var resPlag = new ResultPlag();

				var fileName = Path.GetFileName(res.Doc);

				var name = FilesManagementService.GetFileDisplayName(fileName);

				resPlag.doc = name;

				resPlag.subjectName = subjectName;

				resPlag.coeff = res.Coeff.ToString();

				var pathName = FilesManagementService.GetPathName(fileName);

				var userFileT = SubjectManagementService.GetUserLabFile(pathName);

				var userId = userFileT.UserId;

				var user = StudentManagementService.GetStudent(userId);

				resPlag.author = user.FullName;

				resPlag.groupName = user.Group.Name;

				data.Add(resPlag);
			}

			
			Directory.Delete(this.PlagiarismTempPath + path, true);

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