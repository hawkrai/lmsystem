using System;
using System.Data.Entity;
using System.Linq;

using LMPlatform.Data.Infrastructure;

namespace Application.Infrastructure.CPManagement
{
    public static class AuthorizationHelper
    {
        public static void ValidateLecturerAccess(ICpContext context, int userId)
        {
            if (!IsLecturer(context, userId))
            {
                throw new ApplicationException("Only lecturers have permissions for this operation!");
            }
        }

        public static bool IsLecturer(ICpContext context, int userId)
        {
            return context.Users.Include(x => x.Lecturer).Single(x => x.Id == userId).Lecturer != null;
        }

        public static bool IsStudent(ICpContext context, int userId)
        {
            return context.Users.Include(x => x.Student).Single(x => x.Id == userId).Student != null;
        }

    }
}
