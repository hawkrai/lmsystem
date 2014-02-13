using Application.Core.Data;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories
{
    public class AttachmentRepository : RepositoryBase<LmPlatformModelsContext, Attachment>, IAttachmentRepository
    {
        public AttachmentRepository(LmPlatformModelsContext dataContext) : base(dataContext)
        {
        }
    }
}
