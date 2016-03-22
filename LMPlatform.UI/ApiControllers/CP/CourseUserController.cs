using Application.Core;
using Application.Infrastructure.CPManagement;
using Application.Infrastructure.CTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace LMPlatform.UI.ApiControllers.CP
{
    public class CourseUserController : ApiController
    {
        public UserData Get()
        {
            return UserService.GetUserInfo(WebSecurity.CurrentUserId);
        }

        private readonly LazyDependency<ICPUserService> userService = new LazyDependency<ICPUserService>();

        private ICPUserService UserService
        {
            get { return userService.Value; }
        }

    }
}
