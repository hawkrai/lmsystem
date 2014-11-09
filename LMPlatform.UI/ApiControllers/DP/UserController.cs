using System.Collections.Generic;
using System.Web.Http;
using Application.Core;
using Application.Infrastructure.DPManagement;
using Application.Infrastructure.DTO;
using WebMatrix.WebData;

namespace LMPlatform.UI.ApiControllers.DP
{
    public class UserController : ApiController
    {
        public UserData Get()
        {
            return UserService.GetUserInfo(WebSecurity.CurrentUserId);
        }

        private readonly LazyDependency<IUserService> userService = new LazyDependency<IUserService>();

        private IUserService UserService
        {
            get { return userService.Value; }
        }
    }
}
