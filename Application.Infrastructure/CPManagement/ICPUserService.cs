using Application.Infrastructure.CTO;

namespace Application.Infrastructure.CPManagement
{
    public interface ICPUserService
    {
        UserData GetUserInfo(int userId);
    }
}
