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

namespace Application.Infrastructure.BTS
{
    public class MatrixManagmentService : IMatrixManagmentService
    {
        private const string RequirementRegex = @"\sR\s?(\d(\.\d)*)\.?\s*([^\r\n]*)\s";

        public void Fillrequirements(int projectId, string requirementsFileName)
        {
            var attachment = ProjectManagementService.GetAttachment(projectId, requirementsFileName);
            string path = FilesManagementService.GetFullPath(attachment);
            Word.Application app = new Word.Application();
            var document = app.Documents.Open(path);

            string requirementsText = document.Range().Text;
            Dictionary<string, string> requirements = GetRequirements(requirementsText);
            CreateRequirements(requirements, projectId);

            document.Save();
            document.Close();
            Marshal.ReleaseComObject(document);
            Marshal.ReleaseComObject(app);
        }

        public List<ProjectMatrixRequirement> GetRequirements(int projectId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var query = new Query<ProjectMatrixRequirement>(e => e.ProjectId == projectId);
                return repositoriesContainer.ProjectMatrixRequirementsRepository.GetAll(query).ToList();
            }
        }

        private Dictionary<string, string> GetRequirements(string text)
        {
            Regex regex = new Regex(RequirementRegex);

            var result = new Dictionary<string, string>();
            foreach (Match match in regex.Matches(text))
            {
                result[match.Groups[1].Value] = match.Groups[3].Value;
            }

            return result;
        }

        private void CreateRequirements(Dictionary<string, string> requirements, int projectId)
        {
            foreach(var requirementPair in requirements)
            {
                var requirement = new ProjectMatrixRequirement
                {
                    Number = requirementPair.Key,
                    Name = requirementPair.Value,
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
