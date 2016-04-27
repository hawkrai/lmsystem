﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using LMPlatform.Models.BTS;
using LMPlatform.Models.DP;
using LMPlatform.Models.CP;
using LMPlatform.Models.KnowledgeTesting;

namespace LMPlatform.Data.Infrastructure
{
    using LMPlatform.Models;

    public class LmPlatformModelsContext : DbContext, IDpContext, ICpContext
    {
        #region Constructors

        public LmPlatformModelsContext()
            : base("DefaultConnection")
        {
	        
        }

        #endregion Constructors

        #region DataContext Members

        public DbSet<Membership> Membership { get; set; }

        public DbSet<OAuthMembership> OAuthMembership { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<ScoObjects> ScoObjects { get; set; }

        public IQueryable<Student> GetGraduateStudents()
        {
            return Students.Where(StudentIsGraduate);
        }

        public Expression<Func<Student, bool>> StudentIsGraduate
        {
            get
            {
                var currentYearStr = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                var nextYearStr = DateTime.Now.AddYears(1).Year.ToString(CultureInfo.InvariantCulture);

                return x =>
                    (x.Group.GraduationYear == currentYearStr && DateTime.Now.Month <= 9) ||
                    (x.Group.GraduationYear == nextYearStr && DateTime.Now.Month >= 9);
            }
        }

        public IQueryable<Group> GetGraduateGroups()
        {
            var currentYearStr = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
            var nextYearStr = DateTime.Now.AddYears(1).Year.ToString(CultureInfo.InvariantCulture);

            return Groups.Where(x =>
                (x.GraduationYear == currentYearStr && DateTime.Now.Month <= 9) ||
                (x.GraduationYear == nextYearStr && DateTime.Now.Month >= 9));
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

        public DbSet<Materials> Materials
        {
            get;
            set;
        }

        public DbSet<Concept> Concept
        {
            get;
            set;
        }

        public DbSet<Folders> Folders
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

        public DbSet<SubjectNews> SubjectNewses
        {
            get;
            set;
        }

        public DbSet<Attachment> Attachments
        {
            get;
            set;
        }

        public DbSet<Message> Messages
        {
            get;
            set;
        }

        public DbSet<UserMessages> UserMessages
        {
            get;
            set;
        }

        public DbSet<SubGroup> SubGroups
        {
            get;
            set;
        }

        public DbSet<SubjectStudent> SubjectStudents
        {
            get;
            set;
        }

        public DbSet<Lectures> Lectures
        {
            get;
            set;
        }

        public DbSet<TestPassResult> TestPassResults
        {
            get;
            set;
        }

        public DbSet<Labs> Labs
        {
            get;
            set;
        }

        public DbSet<Practical> Practicals
        {
            get;
            set;
        }

        public DbSet<ScheduleProtectionLabs> ScheduleProtectionLabs
        {
            get;
            set;
        }

        public DbSet<LecturesVisitMark> LecturesVisitMarks { get; set; }

        public DbSet<ScheduleProtectionLabMark> ScheduleProtectionLabMarks { get; set; }

        public DbSet<StudentLabMark> StudentLabMarks { get; set; }

        public DbSet<ScheduleProtectionPractical> ScheduleProtectionPracticals { get; set; }

        public DbSet<ScheduleProtectionPracticalMark> ScheduleProtectionPracticalMarks { get; set; }

        public DbSet<StudentPracticalMark> StudentPracticalMarks { get; set; }

        public DbSet<UserLabFiles> UserLabFiles { get; set; }

		public DbSet<AccessCode> AccessCode { get; set; }

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
            modelBuilder.Entity<ScoObjects>().Map(m => m.ToTable("ScoObjects"));
            modelBuilder.Entity<Subject>().Map(m => m.ToTable("Subjects"));
            modelBuilder.Entity<Module>().Map(m => m.ToTable("Modules"));
            modelBuilder.Entity<SubjectGroup>().Map(m => m.ToTable("SubjectGroups"));
            modelBuilder.Entity<SubjectModule>().Map(m => m.ToTable("SubjectModules"));
            modelBuilder.Entity<SubjectNews>().Map(m => m.ToTable("SubjectNewses"));
            modelBuilder.Entity<Attachment>().Map(m => m.ToTable("Attachments"));
            modelBuilder.Entity<Message>().Map(m => m.ToTable("Messages"));
            modelBuilder.Entity<SubjectStudent>().Map(m => m.ToTable("SubjectStudents"));
            modelBuilder.Entity<UserMessages>()
                .HasRequired(u => u.Author)
                .WithMany()
                .WillCascadeOnDelete(true);

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
                .WithRequiredPrincipal(e => e.User)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<User>()
                .HasRequired<Lecturer>(e => e.Lecturer)
                .WithRequiredPrincipal(e => e.User)
                .WillCascadeOnDelete(true);

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

            modelBuilder.Entity<Concept>().Map(m => m.ToTable("Concept"));
            modelBuilder.Entity<Concept>().HasMany<Concept>(d => d.Children);

            modelBuilder.Entity<Subject>()
                .HasMany<Concept>(e => e.Concept)
                .WithRequired(e => e.Subject)
                .HasForeignKey(e => e.SubjectId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany<Concept>(e => e.Concept)
                .WithRequired(e => e.Author)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SubjectModule>()
                 .HasMany<Folders>(e => e.Folders)
                 .WithRequired(e => e.SubjectModule)
                 .HasForeignKey(e => e.SubjectModuleId)
                 .WillCascadeOnDelete(false);

            modelBuilder.Entity<Module>()
               .HasMany<SubjectModule>(e => e.SubjectModules)
               .WithRequired(e => e.Module)
               .HasForeignKey(e => e.ModuleId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<SubGroup>()
               .HasMany<SubjectStudent>(e => e.SubjectStudents)
               .WithRequired(e => e.SubGroup)
               .HasForeignKey(e => e.SubGroupId)
               .WillCascadeOnDelete(true);

            modelBuilder.Entity<SubjectGroup>()
               .HasMany<SubGroup>(e => e.SubGroups)
               .WithRequired(e => e.SubjectGroup)
               .HasForeignKey(e => e.SubjectGroupId)
			   .WillCascadeOnDelete(true);

            modelBuilder.Entity<Student>()
               .HasMany<SubjectStudent>(e => e.SubjectStudents)
               .WithRequired(e => e.Student)
               .HasForeignKey(e => e.StudentId)
               .WillCascadeOnDelete(true);

            modelBuilder.Entity<SubjectGroup>()
               .HasMany<SubjectStudent>(e => e.SubjectStudents)
               .WithRequired(e => e.SubjectGroup)
               .HasForeignKey(e => e.SubjectGroupId)
			   .WillCascadeOnDelete(true);

            modelBuilder.Entity<Subject>()
                .HasMany<Lectures>(e => e.Lectures)
                .WithRequired(e => e.Subject)
                .HasForeignKey(e => e.SubjectId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Subject>()
                .HasMany<Labs>(e => e.Labs)
                .WithRequired(e => e.Subject)
                .HasForeignKey(e => e.SubjectId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Subject>()
                .HasMany<Practical>(e => e.Practicals)
                .WithRequired(e => e.Subject)
                .HasForeignKey(e => e.SubjectId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SubGroup>()
               .HasMany<ScheduleProtectionLabs>(e => e.ScheduleProtectionLabs)
               .WithRequired(e => e.SubGroup)
               .HasForeignKey(e => e.SuGroupId)
			   .WillCascadeOnDelete(true);

            modelBuilder.Entity<Subject>()
                .HasMany<LecturesScheduleVisiting>(e => e.LecturesScheduleVisitings)
                .WithRequired(e => e.Subject)
                .HasForeignKey(e => e.SubjectId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LecturesScheduleVisiting>()
                .HasMany<LecturesVisitMark>(e => e.LecturesVisitMarks)
                .WithRequired(e => e.LecturesScheduleVisiting)
                .HasForeignKey(e => e.LecturesScheduleVisitingId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .HasMany<LecturesVisitMark>(e => e.LecturesVisitMarks)
                .WithRequired(e => e.Student)
                .HasForeignKey(e => e.StudentId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Student>()
                .HasMany<ScheduleProtectionLabMark>(e => e.ScheduleProtectionLabMarks)
                .WithRequired(e => e.Student)
                .HasForeignKey(e => e.StudentId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ScheduleProtectionLabs>()
                .HasMany<ScheduleProtectionLabMark>(e => e.ScheduleProtectionLabMarks)
                .WithRequired(e => e.ScheduleProtectionLab)
                .HasForeignKey(e => e.ScheduleProtectionLabId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Student>()
                .HasMany<StudentLabMark>(e => e.StudentLabMarks)
                .WithRequired(e => e.Student)
                .HasForeignKey(e => e.StudentId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Labs>()
                .HasMany<StudentLabMark>(e => e.StudentLabMarks)
                .WithRequired(e => e.Lab)
                .HasForeignKey(e => e.LabId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Group>()
               .HasMany<ScheduleProtectionPractical>(e => e.ScheduleProtectionPracticals)
               .WithRequired(e => e.Group)
               .HasForeignKey(e => e.GroupId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Subject>()
               .HasMany<ScheduleProtectionPractical>(e => e.ScheduleProtectionPracticals)
               .WithRequired(e => e.Subject)
               .HasForeignKey(e => e.SubjectId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .HasMany<ScheduleProtectionPracticalMark>(e => e.ScheduleProtectionPracticalMarks)
                .WithRequired(e => e.Student)
                .HasForeignKey(e => e.StudentId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ScheduleProtectionPractical>()
                .HasMany<ScheduleProtectionPracticalMark>(e => e.ScheduleProtectionPracticalMarks)
                .WithRequired(e => e.ScheduleProtectionPractical)
                .HasForeignKey(e => e.ScheduleProtectionPracticalId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .HasMany<StudentPracticalMark>(e => e.StudentPracticalMarks)
                .WithRequired(e => e.Student)
                .HasForeignKey(e => e.StudentId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Practical>()
                .HasMany<StudentPracticalMark>(e => e.StudentPracticalMarks)
                .WithRequired(e => e.Practical)
                .HasForeignKey(e => e.PracticalId)
                .WillCascadeOnDelete(false);

            MapKnowledgeTestingEntities(modelBuilder);
            MapBTSEntities(modelBuilder);
            MapDpEntities(modelBuilder);
        }

        private void MapKnowledgeTestingEntities(DbModelBuilder modelBuilder)
        {
            var testEntity = modelBuilder.Entity<Test>();
            testEntity.Property(test => test.Title).IsRequired();
            testEntity.HasRequired(test => test.Subject)
                .WithMany(subject => subject.SubjectTests)
                .HasForeignKey(test => test.SubjectId);
            testEntity.Ignore(test => test.Unlocked);

            var questionEntity = modelBuilder.Entity<Question>();
            questionEntity.Property(question => question.Title).IsRequired();
            questionEntity.HasRequired(question => question.Test)
                .WithMany(test => test.Questions)
                .HasForeignKey(question => question.TestId);

            var answerEntity = modelBuilder.Entity<Answer>();
            answerEntity.Property(answer => answer.Content).IsRequired();
            answerEntity.HasRequired(answer => answer.Question)
                .WithMany(question => question.Answers)
                .HasForeignKey(answer => answer.QuestionId);

            var testUnlockEntity = modelBuilder.Entity<TestUnlock>();
            testUnlockEntity.HasRequired(testunlock => testunlock.Test)
                .WithMany(test => test.TestUnlocks)
                .HasForeignKey(testunlock => testunlock.TestId);
            testUnlockEntity.HasRequired(testunlock => testunlock.Student)
                .WithMany(student => student.TestUnlocks)
                .HasForeignKey(testunlock => testunlock.StudentId);

            var testPassResultEntity = modelBuilder.Entity<TestPassResult>();
            testPassResultEntity.HasRequired(passResult => passResult.User)
                .WithMany(user => user.TestPassResults)
                .HasForeignKey(passResult => passResult.StudentId);

            var studentAnswerOnTestQuestionEntity = modelBuilder.Entity<AnswerOnTestQuestion>();
            studentAnswerOnTestQuestionEntity.HasRequired(answer => answer.User)
                .WithMany(user => user.UserAnswersOnTestQuestions)
                .HasForeignKey(answer => answer.UserId);
            studentAnswerOnTestQuestionEntity.HasOptional(answer => answer.Answer)
                .WithMany()
                .HasForeignKey(answer => answer.AnswerId);
        }

        #endregion Protected Members

        #region DataContext BTS

        public DbSet<Project> Projects
        {
            get;
            set;
        }

        public DbSet<ProjectUser> ProjectUsers
        {
            get;
            set;
        }

        public DbSet<ProjectRole> ProjectRoles
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
            modelBuilder.Entity<ProjectUser>().Map(m => m.ToTable("ProjectUsers"));
            modelBuilder.Entity<ProjectRole>().Map(m => m.ToTable("ProjectRoles"));
            modelBuilder.Entity<Bug>().Map(m => m.ToTable("Bugs"));
            modelBuilder.Entity<BugStatus>().Map(m => m.ToTable("BugStatuses"));
            modelBuilder.Entity<BugSeverity>().Map(m => m.ToTable("BugSeverities"));
            modelBuilder.Entity<BugSymptom>().Map(m => m.ToTable("BugSymptoms"));

            modelBuilder.Entity<Project>()
                 .HasRequired<User>(e => e.Creator)
                 .WithMany(e => e.Projects)
                 .HasForeignKey(e => e.CreatorId)
                 .WillCascadeOnDelete(false);

            modelBuilder.Entity<Project>()
                .HasMany<ProjectComment>(e => e.ProjectComments)
                .WithRequired(e => e.Project)
                .HasForeignKey(e => e.ProjectId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<User>()
                .HasMany<ProjectComment>(e => e.ProjectComments)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Project>()
                .HasMany<ProjectUser>(e => e.ProjectUsers)
                .WithRequired(e => e.Project)
                .HasForeignKey(e => e.ProjectId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<User>()
                .HasMany<ProjectUser>(e => e.ProjectUsers)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProjectRole>()
                .HasMany<ProjectUser>(e => e.ProjectUser)
                .WithRequired(e => e.ProjectRole)
                .HasForeignKey(e => e.ProjectRoleId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Project>()
               .HasMany<Bug>(e => e.Bugs)
               .WithRequired(e => e.Project)
               .HasForeignKey(e => e.ProjectId)
               .WillCascadeOnDelete(true);

            modelBuilder.Entity<BugStatus>()
                .HasMany<Bug>(e => e.Bug)
                .WithRequired(e => e.Status)
                .HasForeignKey(e => e.StatusId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BugSeverity>()
                .HasMany<Bug>(e => e.Bug)
                .WithRequired(e => e.Severity)
                .HasForeignKey(e => e.SeverityId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BugSymptom>()
                .HasMany<Bug>(e => e.Bug)
                .WithRequired(e => e.Symptom)
                .HasForeignKey(e => e.SymptomId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Bug>()
                .HasRequired<User>(e => e.Reporter)
                .WithMany(e => e.Bugs)
                .HasForeignKey(e => e.ReporterId)
                .WillCascadeOnDelete(false);
        }

        #endregion Protected BTS

        #region DP

        protected void MapDpEntities(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DiplomProjectConsultationDate>()
                .HasMany(e => e.DiplomProjectConsultationMarks)
                .WithRequired(e => e.DiplomProjectConsultationDate)
                .HasForeignKey(e => e.ConsultationDateId);

            modelBuilder.Entity<CourseProjectConsultationDate>()
               .HasMany(e => e.CourseProjectConsultationMarks)
               .WithRequired(e => e.CourseProjectConsultationDate)
               .HasForeignKey(e => e.ConsultationDateId);

            modelBuilder.Entity<Student>()
                .HasMany(e => e.AssignedDiplomProjects)
                .WithRequired(e => e.Student)
                .HasForeignKey(e => e.StudentId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .HasMany(e => e.PercentagesResults)
                .WithRequired(e => e.Student)
                .HasForeignKey(e => e.StudentId)
                .WillCascadeOnDelete(false); //TODO: get rid of multiple cascade paths

            modelBuilder.Entity<Student>()
                .HasMany(e => e.DiplomProjectConsultationMarks)
                .WithRequired(e => e.Student)
                .HasForeignKey(e => e.StudentId)
                .WillCascadeOnDelete(false); //TODO: get rid of multiple cascade paths

            modelBuilder.Entity<DiplomProjectConsultationMark>()
                .Property(e => e.Mark)
                .IsFixedLength()
                .IsUnicode(false);

            //            modelBuilder.Entity<DiplomProject>()
            //                .HasMany(x => x.Groups)
            //                .WithMany(x => x.DiplomProjects)
            //                .Map(x =>
            //            {
            //                x.ToTable("DiplomProjectGroups");
            //                x.MapLeftKey("DiplomProjectId");
            //                x.MapRightKey("GroupId");
            //            });
        }

        public virtual DbSet<AssignedDiplomProject> AssignedDiplomProjects { get; set; }

        public virtual DbSet<DiplomPercentagesGraph> DiplomPercentagesGraphs { get; set; }

        public virtual DbSet<DiplomPercentagesGraphToGroup> DiplomPercentagesGraphToGroup { get; set; }

        public virtual DbSet<DiplomPercentagesResult> DiplomPercentagesResults { get; set; }

        public virtual DbSet<DiplomProjectConsultationDate> DiplomProjectConsultationDates { get; set; }

        public virtual DbSet<DiplomProjectConsultationMark> DiplomProjectConsultationMarks { get; set; }

        public virtual DbSet<DiplomProjectGroup> DiplomProjectGroups { get; set; }

        public virtual DbSet<DiplomProject> DiplomProjects { get; set; }

        public virtual DbSet<DiplomProjectTaskSheetTemplate> DiplomProjectTaskSheetTemplates { get; set; }

        #endregion

        #region CP

        public virtual DbSet<CourseProjectNews> CourseProjectNewses { get; set; }
        public virtual DbSet<CourseProject> CourseProjects { get; set; }
        public virtual DbSet<AssignedCourseProject> AssignedCourseProjects { get; set; }
        public virtual DbSet<CourseProjectGroup> CourseProjectGroups { get; set; }
        public virtual DbSet<CoursePercentagesGraph> CoursePercentagesGraphs { get; set; }

        public virtual DbSet<CoursePercentagesGraphToGroup> CoursePercentagesGraphToGroup { get; set; }

        public virtual DbSet<CoursePercentagesResult> CoursePercentagesResults { get; set; }

        public virtual DbSet<CourseProjectConsultationDate> CourseProjectConsultationDates { get; set; }

        public virtual DbSet<CourseProjectConsultationMark> CourseProjectConsultationMarks { get; set; }

        public virtual DbSet<CourseProjectTaskSheetTemplate> CourseProjectTaskSheetTemplates { get; set; }

        #endregion
    }
}
