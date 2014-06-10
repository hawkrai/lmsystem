using System;
using System.Data.Entity;
using System.Threading;
using System.Web.Security;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories;
using LMPlatform.Models;
using WebMatrix.WebData;

namespace Application.Infrastructure.AccountManagement
{
    public class SimpleMembershipInitializer
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public static void Initialize()
        {
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        public SimpleMembershipInitializer()
        {
            Database.SetInitializer<LmPlatformModelsContext>(new ProjectInitializer());

            try
            {
                bool firstLoad;

                using (var context = new LmPlatformModelsContext())
                {
                    firstLoad = DataBaseInitializer.InitializeDatabase(context);
                }

                WebSecurity.InitializeDatabaseConnection("DefaultConnection", "Users", "UserId", "UserName", autoCreateTables: true);

                if (firstLoad)
                {
                    WebSecurity.CreateUserAndAccount("admin", "123456");
                    Roles.AddUserToRole("admin", "admin");
                    WebSecurity.CreateUserAndAccount("popova", "123456");
                    Roles.AddUserToRole("popova", "lector");
                    WebSecurity.CreateUserAndAccount("hawk_1", "123456");
                    Roles.AddUserToRole("hawk_1", "student");
                    WebSecurity.CreateUserAndAccount("hawk_2", "123456");
                    Roles.AddUserToRole("hawk_2", "student");
                    WebSecurity.CreateUserAndAccount("hawk_3", "123456");
                    Roles.AddUserToRole("hawk_3", "student");
                    WebSecurity.CreateUserAndAccount("hawk_4", "123456");
                    Roles.AddUserToRole("hawk_4", "student");
                    WebSecurity.CreateUserAndAccount("hawk_5", "123456");
                    Roles.AddUserToRole("hawk_5", "student");
                    WebSecurity.CreateUserAndAccount("hawk_6", "123456");
                    Roles.AddUserToRole("hawk_6", "student");
                    WebSecurity.CreateUserAndAccount("hawk_7", "123456");
                    Roles.AddUserToRole("hawk_7", "student");
                    WebSecurity.CreateUserAndAccount("hawk_8", "123456");
                    Roles.AddUserToRole("hawk_8", "student");
                    WebSecurity.CreateUserAndAccount("hawk_9", "123456");
                    Roles.AddUserToRole("hawk_9", "student");
                    WebSecurity.CreateUserAndAccount("hawk_10", "123456");
                    Roles.AddUserToRole("hawk_10", "student");
                    using (var context = new LmPlatformModelsContext())
                    {
                        context.Lecturers.Add(new Lecturer
	                    {
                            Id = 2,
                            FirstName = "Юлия",
                            MiddleName = "Борисовна",
                            LastName = "Попова"
	                    });

                        context.Students.Add(new Student
                        {
                            Id = 3,
                            GroupId = 2,
                            FirstName = "Сергей",
                            MiddleName = "Викторович",
                            LastName = "Яцынович"
                        });

                        context.Students.Add(new Student
                        {
                            Id = 4,
                            GroupId = 2,
                            FirstName = "Киррил",
                            MiddleName = "Викторович",
                            LastName = "Андреевич"
                        });

                        context.Students.Add(new Student
                        {
                            Id = 5,
                            GroupId = 2,
                            FirstName = "Петр",
                            MiddleName = "Викторович",
                            LastName = "Петров"
                        });

                        context.Students.Add(new Student
                        {
                            Id = 6,
                            GroupId = 2,
                            FirstName = "Иван",
                            MiddleName = "Иванович",
                            LastName = "Иванов"
                        });

                        context.Students.Add(new Student
                        {
                            Id = 7,
                            GroupId = 2,
                            FirstName = "Виктор",
                            MiddleName = "Викторович",
                            LastName = "Иванченко"
                        });

                        context.Students.Add(new Student
                        {
                            Id = 8,
                            GroupId = 2,
                            FirstName = "Виктор",
                            MiddleName = "Васильевич",
                            LastName = "Шеремет"
                        });

                        context.Students.Add(new Student
                        {
                            Id = 9,
                            GroupId = 2,
                            FirstName = "Андрей",
                            MiddleName = "Петрович",
                            LastName = "Беляк"
                        });

                        context.Students.Add(new Student
                        {
                            Id = 10,
                            GroupId = 2,
                            FirstName = "Екатерина",
                            MiddleName = "Ивановна",
                            LastName = "Крузернштейн"
                        });

                        context.Students.Add(new Student
                        {
                            Id = 11,
                            GroupId = 2,
                            FirstName = "Анатолий",
                            MiddleName = "Васильевич",
                            LastName = "Шибека"
                        });

                        context.Students.Add(new Student
                        {
                            Id = 12,
                            GroupId = 2,
                            FirstName = "Николай",
                            MiddleName = "Николаевич",
                            LastName = "Гурский"
                        });

                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ошибка", ex);
            }
        }
    }
}