namespace LMPlatform.Data.Repositories
{
    using Application.Core.Data;

    using LMPlatform.Data.Infrastructure;
    using LMPlatform.Data.Repositories.RepositoryContracts;
    using LMPlatform.Models;

    public class GroupsRepository : RepositoryBase<LmPlatformModelsContext, Group>, IGroupsRepository
    {
    }
}