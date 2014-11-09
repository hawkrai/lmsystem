using Application.Infrastructure.DTO;

namespace Application.Infrastructure.DPManagement
{
    public interface IUserService
    {
        UserData GetUserInfo(int userId);
    }
}
