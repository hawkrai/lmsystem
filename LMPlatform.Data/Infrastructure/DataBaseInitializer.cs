using System.Data.Entity.Infrastructure;
using LMPlatform.Models;
using LMPlatform.Models.BTS;

namespace LMPlatform.Data.Infrastructure
{
	public class DataBaseInitializer
	{
		private static void FillInitData(LmPlatformModelsContext context)
		{
			FillRoles(context);

            CreateGroups(context);

            CreateBugSeverities(context);
            CreateBugSymptoms(context);
		    CreateBugStatuses(context);

			context.SaveChanges();
		}

        private static void CreateBugSymptoms(LmPlatformModelsContext context)
        {
            context.BugSymptoms.Add(new BugSymptom { Name = "Косметический дефект" });
            context.BugSymptoms.Add(new BugSymptom { Name = "Повреждение/потеря данных" });
            context.BugSymptoms.Add(new BugSymptom { Name = "Проблема в документации" });
            context.BugSymptoms.Add(new BugSymptom { Name = "Некорректная операция" });
            context.BugSymptoms.Add(new BugSymptom { Name = "Проблема инсталляции" });
            context.BugSymptoms.Add(new BugSymptom { Name = "Ошибка локализации" });
            context.BugSymptoms.Add(new BugSymptom { Name = "Нереализованная функциональность" });
            context.BugSymptoms.Add(new BugSymptom { Name = "Низкая производительность" });
            context.BugSymptoms.Add(new BugSymptom { Name = "Крах системы" });
            context.BugSymptoms.Add(new BugSymptom { Name = "Неожиданное поведение" });
            context.BugSymptoms.Add(new BugSymptom { Name = "Недружественное поведение" });
            context.BugSymptoms.Add(new BugSymptom { Name = "Расхождение с требованиям" });
            context.BugSymptoms.Add(new BugSymptom { Name = "Предложение по улучшению" });
        }

        private static void CreateBugSeverities(LmPlatformModelsContext context)
        {
            context.BugSeverities.Add(new BugSeverity { Name = "Критическая" });
            context.BugSeverities.Add(new BugSeverity { Name = "Высокая" });
            context.BugSeverities.Add(new BugSeverity { Name = "Средняя" });
            context.BugSeverities.Add(new BugSeverity { Name = "Низкая" });
            }

        private static void CreateBugStatuses(LmPlatformModelsContext context)
        {
            context.BugStatuses.Add(new BugStatus { Name = "Открыта" });
            context.BugStatuses.Add(new BugStatus { Name = "Исправлена" });
            context.BugStatuses.Add(new BugStatus { Name = "Проверена" });
            context.BugStatuses.Add(new BugStatus { Name = "Отклонена" });
            context.BugStatuses.Add(new BugStatus { Name = "Отложена" });
            context.BugStatuses.Add(new BugStatus { Name = "Закрыта" });
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