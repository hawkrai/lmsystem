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
                    
                    using (var context = new LmPlatformModelsContext())
                    {
                        context.Lecturers.Add(new Lecturer
	                    {
                            Id = 2,
                            FirstName = "Юлия",
                            MiddleName = "Борисовна",
                            LastName = "Попова"
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