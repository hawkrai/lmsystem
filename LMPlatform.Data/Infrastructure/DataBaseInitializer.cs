using System.Data.Entity.Infrastructure;
using LMPlatform.Models;

namespace LMPlatform.Data.Infrastructure
{
	public class DataBaseInitializer
	{
		private static void FillInitData(LmPlatformModelsContext context)
		{
			FillRoles(context);

            CreateGroups(context);

			context.SaveChanges();
		}

	    private static void CreateGroups(LmPlatformModelsContext context)
	    {
            context.Groups.Add(new Group
            {
                Name = "107229",
                StartYear = string.Format("2009"),
                GraduationYear = string.Format("2014"),
            });

            context.Groups.Add(new Group
            {
                Name = "107219",
                StartYear = string.Format("2009"),
                GraduationYear = string.Format("2014"),
            });
	    }

	    private static void FillRoles(LmPlatformModelsContext context)
		{
			context.Roles.Add(new Role
			{
				RoleName = "admin",
				RoleDisplayName = "Администратор"
			});
			context.Roles.Add(new Role
			{
				RoleName = "student",
				RoleDisplayName = "Студент"
			});
			context.Roles.Add(new Role
			{
				RoleName = "lector",
				RoleDisplayName = "Лектор"
			});
	    }

        public static bool InitializeDatabase(LmPlatformModelsContext context)
		{
			if (!context.Database.Exists())
			{
				((IObjectContextAdapter)context).ObjectContext.CreateDatabase();

				FillInitData(context);

				return true;
			}
			else
			{
				return false;
			}
		}
	}
}