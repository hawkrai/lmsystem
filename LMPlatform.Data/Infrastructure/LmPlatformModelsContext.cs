using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using LMPlatform.Models.BTS;
using LMPlatform.Models.KnowledgeTesting;

namespace LMPlatform.Data.Infrastructure
{
    using LMPlatform.Models;

    public class LmPlatformModelsContext : DbContext
    {
        #region Constructors

        public LmPlatformModelsContext()
            : base("DefaultConnection")
        {
        }

        #endregion Constructors

        #region DataContext Members

        public DbSet<Membership> Membership
        {
            get;
            set;
        }

        public DbSet<OAuthMembership> OAuthMembership
        {
            get;
            set;
        }

        public DbSet<Role> Roles
        {
            get;
            set;
        }

        public DbSet<User> Users
        {
            get;
            set;
        }

        public DbSet<Student> Students
        {
            get;
            set;
        }

        public DbSet<Group> Groups
        {
            get;
            set;
        }

        public DbSet<Subject> Subjects
        {
            get; 
            set;
        }

        public DbSet<Module> Modules
        {
            get; 
            set;
        }

        public DbSet<SubjectGroup> SubjectGroups
        {
            get; 
            set;
        }

        public DbSet<Lecturer> Lecturers
        {
            get; 
            set;
        }

        public DbSet<SubjectModule> SubjectModules
        {
            get;
            set;
        } 

        #endregion DataContext Members

        #region Protected Members

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {   
            modelBuilder.Entity<Membership>().Map(m => m.ToTable("webpages_Membership"))
                .Property(m => m.Id)
                .HasColumnName("UserId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<OAuthMembership>().Map(m => m.ToTable("webpages_OAuthMembership"));
            modelBuilder.Entity<Role>().Map(m => m.ToTable("webpages_Roles"))
                .Property(m => m.Id)
                .HasColumnName("RoleId");
            modelBuilder.Entity<User>().Map(m => m.ToTable("Users"))
                .Property(m => m.Id)
                .HasColumnName("UserId");
            modelBuilder.Entity<Student>().Map(m => m.ToTable("Students"))
                .Property(m => m.Id)
                .HasColumnName("UserId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<Group>().Map(m => m.ToTable("Groups"));
            modelBuilder.Entity<Subject>().Map(m => m.ToTable("Subjects"));
            modelBuilder.Entity<Module>().Map(m => m.ToTable("Modules"));
            modelBuilder.Entity<SubjectGroup>().Map(m => m.ToTable("SubjectGroups"));
            modelBuilder.Entity<SubjectModule>().Map(m => m.ToTable("SubjectModules"));

            modelBuilder.Entity<Membership>()
              .HasMany<Role>(r => r.Roles)
              .WithMany(u => u.Members)
              .Map(m =>
              {
                  m.ToTable("webpages_UsersInRoles");
                  m.MapLeftKey("UserId");
                  m.MapRightKey("RoleId");
              });

            modelBuilder.Entity<User>()
                .HasRequired<Membership>(e => e.Membership)
                .WithRequiredPrincipal(e => e.User);

            modelBuilder.Entity<Group>()
                .HasMany<Student>(e => e.Students)
                .WithRequired(e => e.Group)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasRequired<Student>(e => e.Student)
                .WithRequiredPrincipal(e => e.User);

            modelBuilder.Entity<User>()
                .HasRequired<Lecturer>(e => e.Lecturer)
                .WithRequiredPrincipal(e => e.User);

            modelBuilder.Entity<Subject>()
                .HasMany<SubjectGroup>(e => e.SubjectGroups)
                .WithRequired(e => e.Subject)
                .HasForeignKey(e => e.SubjectId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Group>()
               .HasMany<SubjectGroup>(e => e.SubjectGroups)
               .WithRequired(e => e.Group)
               .HasForeignKey(e => e.GroupId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Subject>()
                .HasMany<SubjectLecturer>(e => e.SubjectLecturers)
                .WithRequired(e => e.Subject)
                .HasForeignKey(e => e.SubjectId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Lecturer>()
               .HasMany<SubjectLecturer>(e => e.SubjectLecturers)
               .WithRequired(e => e.Lecturer)
               .HasForeignKey(e => e.LecturerId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Subject>()
                .HasMany<SubjectModule>(e => e.SubjectModules)
                .WithRequired(e => e.Subject)
                .HasForeignKey(e => e.SubjectId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Module>()
               .HasMany<SubjectModule>(e => e.SubjectModules)
               .WithRequired(e => e.Module)
               .HasForeignKey(e => e.ModuleId)
               .WillCascadeOnDelete(false);
            
            MapKnowledgeTestingEntities(modelBuilder);
            MapBTSEntities(modelBuilder);
        }

        private void MapKnowledgeTestingEntities(DbModelBuilder modelBuilder)
        {
            var testEntity = modelBuilder.Entity<Test>();
            
            testEntity.Property(test => test.Title).IsRequired();
            testEntity.HasRequired(test => test.Subject)
                .WithMany(subject => subject.SubjectTests)
                .HasForeignKey(test => test.SubjectId);
        }

        #endregion Protected Members

        #region DataContext BTS

        public DbSet<Project> Projects
        {
            get;
            set;
        }

        public DbSet<ProjectStudent> ProjectStudents
        {
            get; 
            set;
        }

        public DbSet<Bug> Bugs
        {
            get;
            set;
        }

        public DbSet<BugStatus> BugStatuses
        {
            get;
            set;
        }

        public DbSet<BugSeverity> BugSeverities
        {
            get;
            set;
        }

        public DbSet<BugSymptom> BugSymptoms
        {
            get;
            set;
        }

        #endregion DataContext BTS

        #region Protected BTS

        private void MapBTSEntities(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().Map(m => m.ToTable("Projects"));
            modelBuilder.Entity<Bug>().Map(m => m.ToTable("Bugs"));
            modelBuilder.Entity<ProjectStudent>().Map(m => m.ToTable("ProjectStudents"));
            modelBuilder.Entity<BugStatus>().Map(m => m.ToTable("BugStatuses"));
            modelBuilder.Entity<BugSeverity>().Map(m => m.ToTable("BugSeverities"));
            modelBuilder.Entity<BugSymptom>().Map(m => m.ToTable("BugSymptoms"));

            modelBuilder.Entity<Project>()
                .HasMany<ProjectStudent>(e => e.ProjectStudents)
                .WithRequired(e => e.Project)
                .HasForeignKey(e => e.ProjectId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .HasMany<ProjectStudent>(e => e.ProjectStudents)
                .WithRequired(e => e.Student)
                .HasForeignKey(e => e.StudentId)
                .WillCascadeOnDelete(false);

           modelBuilder.Entity<Project>()
                .HasRequired<User>(e => e.Creator)
                .WithMany(e => e.Projects)
                .HasForeignKey(e => e.CreatorId)
                .WillCascadeOnDelete(false);

           modelBuilder.Entity<Project>()
               .HasMany<Bug>(e => e.Bugs)
               .WithRequired(e => e.Project)
               .HasForeignKey(e => e.ProjectId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<BugStatus>()
                .HasRequired<Bug>(e => e.Bug)
                .WithRequiredPrincipal(e => e.Status);
            modelBuilder.Entity<BugSeverity>()
                .HasRequired<Bug>(e => e.Bug)
                .WithRequiredPrincipal(e => e.Severity);
            modelBuilder.Entity<BugSymptom>()
                .HasRequired<Bug>(e => e.Bug)
                .WithRequiredPrincipal(e => e.Symptom);

            modelBuilder.Entity<Bug>()
                .HasRequired<Student>(e => e.Student)
                .WithMany(e => e.Bugs)
                .HasForeignKey(e => e.StudentId)
                .WillCascadeOnDelete(false);
        }

        #endregion Protected BTS
    }
}
