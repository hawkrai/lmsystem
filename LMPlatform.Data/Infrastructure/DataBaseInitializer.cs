using System;
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
		    CreateProjectRoles(context);
		    CreateModules(context);
			context.SaveChanges();
		}

	    private static void CreateModules(LmPlatformModelsContext context)
	    {
	        context.Modules.Add(new Module { DisplayName = "Новости", Name = "News", ModuleType = ModuleType.News, Visible = true });
            context.Modules.Add(new Module { DisplayName = "Лекции", Name = "Lectures", ModuleType = ModuleType.Lectures, Visible = true });
            context.Modules.Add(new Module { DisplayName = "Лабораторные работы", Name = "Labs", ModuleType = ModuleType.Labs, Visible = true });
            context.Modules.Add(new Module { DisplayName = "Курсовые проекты/работы", Name = "YEManagment", ModuleType = ModuleType.YeManagment, Visible = true });
            context.Modules.Add(new Module { DisplayName = "Файлы", Name = "SubjectAttachments", ModuleType = ModuleType.SubjectAttachments, Visible = true });
            context.Modules.Add(new Module { DisplayName = "Архив", Name = "LabAttachments", ModuleType = ModuleType.LabAttachments, Visible = false });
            context.Modules.Add(new Module { DisplayName = "Проекты", Name = "Projects", ModuleType = ModuleType.Projects, Visible = false });
            context.Modules.Add(new Module { DisplayName = "Тестирование знаний", Name = "SmartTest", ModuleType = ModuleType.SmartTest, Visible = true });
            context.Modules.Add(new Module { DisplayName = "Методические материалы", Name = "DSM", ModuleType = ModuleType.Dsm, Visible = true });
            context.Modules.Add(new Module { DisplayName = "График защиты", Name = "ScheduleProtection", ModuleType = ModuleType.ScheduleProtection, Visible = false });
            context.Modules.Add(new Module { DisplayName = "Результаты", Name = "Results", ModuleType = ModuleType.Results, Visible = false });
            context.Modules.Add(new Module { DisplayName = "Статистика посещения", Name = "StatisticsVisits", ModuleType = ModuleType.StatisticsVisits, Visible = false });
            context.Modules.Add(new Module { DisplayName = "Практические занятия", Name = "Practical", ModuleType = ModuleType.Practical, Visible = true });
	    }

	    private static void CreateBugSymptoms(LmPlatformModelsContext context)
        {
            context.BugSymptoms.Add(new BugSymptom { Name = "Запрос на улучшение" });
            context.BugSymptoms.Add(new BugSymptom { Name = "Инсталяционная ошибка" });
            context.BugSymptoms.Add(new BugSymptom { Name = "Искажение данных" });
            context.BugSymptoms.Add(new BugSymptom { Name = "Косметическая ошибка" });
            context.BugSymptoms.Add(new BugSymptom { Name = "Локализационные проблемы" });
            context.BugSymptoms.Add(new BugSymptom { Name = "Неверное действие" });
            context.BugSymptoms.Add(new BugSymptom { Name = "Недружественное поведение" });
            context.BugSymptoms.Add(new BugSymptom { Name = "Неожиданное поведение" });
            context.BugSymptoms.Add(new BugSymptom { Name = "Низкая производительность" });
            context.BugSymptoms.Add(new BugSymptom { Name = "Отказ системы" });
            context.BugSymptoms.Add(new BugSymptom { Name = "Отсутствующий функционал" });
            context.BugSymptoms.Add(new BugSymptom { Name = "Ошибка документации" });
            context.BugSymptoms.Add(new BugSymptom { Name = "Потеря данных" });
            context.BugSymptoms.Add(new BugSymptom { Name = "Различие со спецификацией" });
        }

        private static void CreateBugSeverities(LmPlatformModelsContext context)
        {
            context.BugSeverities.Add(new BugSeverity { Name = "Низкая" });
            context.BugSeverities.Add(new BugSeverity { Name = "Средняя" });
            context.BugSeverities.Add(new BugSeverity { Name = "Высокая" });
            context.BugSeverities.Add(new BugSeverity { Name = "Критическая" });
            }

        private static void CreateBugStatuses(LmPlatformModelsContext context)
        {
            context.BugStatuses.Add(new BugStatus { Name = "Новая" });
            context.BugStatuses.Add(new BugStatus { Name = "Присвоена" });
            context.BugStatuses.Add(new BugStatus { Name = "Исправлена" });
            context.BugStatuses.Add(new BugStatus { Name = "Отложена" });
            context.BugStatuses.Add(new BugStatus { Name = "Не воспроизводится" });
            context.BugStatuses.Add(new BugStatus { Name = "Дубликат" });
            context.BugStatuses.Add(new BugStatus { Name = "Не ошибка" });
            context.BugStatuses.Add(new BugStatus { Name = "Закрыта" });
        }

	    private static void CreateProjectRoles(LmPlatformModelsContext context)
	    {
            context.ProjectRoles.Add(new ProjectRole { Name = "Разработчик" });
            context.ProjectRoles.Add(new ProjectRole { Name = "Тестировщик" });
            context.ProjectRoles.Add(new ProjectRole { Name = "Руководитель проекта" });
	    }

	    private static void CreateGroups(LmPlatformModelsContext context)
	    {
            context.Groups.Add(new Group
            {
                Name = "107219",
                StartYear = string.Format("2009"),
                GraduationYear = string.Format("2014"),
            });

            context.Groups.Add(new Group
            {
                Name = "107229",
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
				//((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
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