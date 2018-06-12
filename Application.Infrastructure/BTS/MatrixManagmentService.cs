using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core;
using Application.Infrastructure.ProjectManagement;
using Application.Infrastructure.FilesManagement;
using Word = Microsoft.Office.Interop.Word;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using LMPlatform.Models.BTS;

namespace Application.Infrastructure.BTS
{
    public class MatrixManagmentService : IMatrixManagmentService
    {
        private const string RequirementRegex = @"\s(R\s?\d[^\r\n]*)\s";

        public void Fillrequirements(int projectId, string requirementsFileName)
        {
            var attachment = ProjectManagementService.GetAttachment(projectId, requirementsFileName);
            string path = FilesManagementService.GetFullPath(attachment);
            Word.Application app = new Word.Application();
            var document = app.Documents.Open(path);

            string requirementsText = document.Range().Text;
            List<string> requirements = GetRequirements(requirementsText);
            CreateRequirements(requirements, projectId);

            document.Save();
            document.Close();
            Marshal.ReleaseComObject(document);
            Marshal.ReleaseComObject(app);
        }

        private List<string> GetRequirements(string text)
        {
            Regex regex = new Regex(RequirementRegex);

            var result = new List<string>();
            foreach (Match match in regex.Matches(text))
            {
                result.Add(match.Groups[1].Value);
            }

            return result;
        }

        private void CreateRequirements(List<string> requirementNames, int projectId)
        {
            foreach(var name in requirementNames)
            {
                var requirement = new ProjectMatrixRequirement
                {
                    Name = name,
                    ProjectId = projectId
                };
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
