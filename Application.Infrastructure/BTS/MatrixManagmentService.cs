using Application.Core;
using Application.Core.Data;
using Application.Infrastructure.FilesManagement;
using Application.Infrastructure.ProjectManagement;
using LMPlatform.Data.Repositories;
using LMPlatform.Models.BTS;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;

namespace Application.Infrastructure.BTS
{
    public class MatrixManagmentService : IMatrixManagmentService
    {
        private const string RequirementNumberRegex = @"R\s?(\d(\.\d)*)\.?\s*([^\r\n]*)";
        private const string RequirementWordRegex = @"\s" + RequirementNumberRegex + @"\s";
        private const string RequirementExcelRegex = @"\s?" + RequirementNumberRegex + @"\s?";

        public void FillRequirements(int projectId, string requirementsFileName)
        {
            var path = GetFilePath(projectId, requirementsFileName);
            var requirementsText = ReadWordDocument(path);
            var requirements = GetRequirements(requirementsText);
            CreateRequirements(requirements, projectId);
        }

        public void ClearRequirements(int projectId)
        {
	        using var repositoriesContainer = new LmPlatformRepositoriesContainer();
	        repositoriesContainer.ProjectMatrixRequirementsRepository.DeleteAll(projectId);
        }

        public void FillRequirementsCoverage(int projectId, string testsFileName)
        {
            var path = GetFilePath(projectId, testsFileName);
            path = path.Replace("//", "\\");
            var readedSheets = ReadExcelDocument(path);
            var filteredSheets = FilterRequirements(readedSheets);
            CoverRequirements(filteredSheets);
        }

        public List<ProjectMatrixRequirement> GetRequirements(int projectId)
        {
	        using var repositoriesContainer = new LmPlatformRepositoriesContainer();
	        var query = new Query<ProjectMatrixRequirement>(e => e.ProjectId == projectId);
	        return repositoriesContainer.ProjectMatrixRequirementsRepository.GetAll(query).ToList();
        }

        private void CoverRequirements(Dictionary<string, List<string>> sheetsWithNumbers)
        {
            var uniqNumbers = SelectUniqNumbers(sheetsWithNumbers);

            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var query = new Query<ProjectMatrixRequirement>(e => uniqNumbers.Contains(e.Number));
                repositoriesContainer.ProjectMatrixRequirementsRepository.UpdateCoveredWhere(query);
            }
        }

        private List<string> SelectUniqNumbers(Dictionary<string, List<string>> sheetsWithNumbers)
        {
            var uniqueNumbers = new HashSet<string>();
            foreach (var sheet in sheetsWithNumbers)
            {
                foreach (var number in sheet.Value)
                {
                    uniqueNumbers.Add(number);
                }
            }
            return uniqueNumbers.ToList();
        }

        private Dictionary<string, List<string>> FilterRequirements(Dictionary<string, List<string>> sheets)
        {
            var filteredSheets = new Dictionary<string, List<string>>();
            var regex = new Regex(RequirementExcelRegex);

            foreach (var sheet in sheets)
            {
                foreach (var value in sheet.Value)
                {
                    var match = regex.Match(value);
                    if (match.Success)
                    {
                        if (!filteredSheets.ContainsKey(sheet.Key))
                        {
                            filteredSheets[sheet.Key] = new List<string>();
                        }

                        filteredSheets[sheet.Key].Add(match.Groups[1].Value);
                    }
                }
            }
            return filteredSheets;
        }

        private Dictionary<string, List<string>> ReadExcelDocument(string path)
        {
            var app = new Excel.Application();
            var workbooks = app.Workbooks;
            var document = workbooks.Open(path);
            var readedSheets = new Dictionary<string, List<string>>();

            var sheets = document.Sheets;
            foreach (var sheet in sheets)
            {
                var worksheet = (Excel.Worksheet)sheet;
                var name = worksheet.Name;
                var cells = ReadExcelCells(worksheet);
                readedSheets[name] = cells;
            }

            document.Close();
            workbooks.Close();
            app.Quit();
            Marshal.ReleaseComObject(sheets);
            Marshal.ReleaseComObject(document);
            Marshal.ReleaseComObject(workbooks);
            Marshal.ReleaseComObject(app);

            return readedSheets;
        }

        private List<string> ReadExcelCells(Excel.Worksheet worksheet)
        {
            Excel.Range excelRange = worksheet.UsedRange;
            var name = worksheet.Name;
            var columns = excelRange.Columns;
            Excel.Range column = columns[2];
            var cells = column.Cells;

            List<string> readedCells;
            System.Array array = cells.Value2;
            if (array == null)
            {
                readedCells = new List<string>();
            }
            else
            {
                readedCells = array.OfType<string>().ToList();
            }

            Marshal.ReleaseComObject(cells);
            Marshal.ReleaseComObject(column);
            Marshal.ReleaseComObject(columns);
            Marshal.ReleaseComObject(excelRange);
            Marshal.ReleaseComObject(worksheet);

            return readedCells;
        }

        private string ReadWordDocument(string path)
        {
            var app = new Word.Application();
            var document = app.Documents.Open(path);
            var range = document.Range();
            var text = range.Text;
            document.Save();
            document.Close();
            app.Quit();
            Marshal.ReleaseComObject(range);
            Marshal.ReleaseComObject(document);
            Marshal.ReleaseComObject(app);
            return text;
        }

        private string GetFilePath(int projectId, string fileName)
        {
            var attachment = ProjectManagementService.GetAttachment(projectId, fileName);
            return FilesManagementService.GetFullPath(attachment);
        }

        private Dictionary<string, string> GetRequirements(string text)
        {
            var regex = new Regex(RequirementWordRegex);
            var result = new Dictionary<string, string>();

            foreach (Match match in regex.Matches(text))
            {
                result[match.Groups[1].Value] = match.Groups[3].Value;
            }

            return result;
        }

        private void CreateRequirements(Dictionary<string, string> requirements, int projectId)
        {
	        var projectMatrixRequirements = requirements.Select(requirementPair => new ProjectMatrixRequirement
	        {
		        Number = requirementPair.Key,
		        Name = requirementPair.Value,
		        ProjectId = projectId
	        });
            foreach (var requirement in projectMatrixRequirements)
	        {
		        ProjectManagementService.CreateMatrixRequirement(requirement);
	        }
        }

        private IProjectManagementService ProjectManagementService
        {
            get { return _projectManagementService.Value; }
        }

        private IFilesManagementService FilesManagementService
        {
            get { return _filesManagementService.Value; }
        }

        private readonly LazyDependency<IProjectManagementService> _projectManagementService = new LazyDependency<IProjectManagementService>();
        private readonly LazyDependency<IFilesManagementService> _filesManagementService = new LazyDependency<IFilesManagementService>();
    }
}
