using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

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
        }

        #endregion Protected Members
    }
}
