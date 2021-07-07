namespace LMPlatform.Data.Repositories.RepositoryContracts
{
    using Application.Core.Data;

    using LMPlatform.Models;

    public interface IUsersRepository : IRepositoryBase<User>
    {
    }

    public interface IMembershipRepository : IRepositoryBase<Membership>
    {
    }
}