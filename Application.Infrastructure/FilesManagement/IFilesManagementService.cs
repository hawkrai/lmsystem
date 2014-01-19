using LMPlatform.Models;

namespace Application.Infrastructure.FilesManagement
{
	public interface IFilesManagementService
	{
        void DeleteFileAttachment(Attachment attachment);

        string GetFileDisplayName(string guid);
	}
}