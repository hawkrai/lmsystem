namespace LMPlatform.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WatchingTime",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ConceptId = c.Int(nullable: false),
                        Time = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TestQuestionPassResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.Int(nullable: false),
                        TestId = c.Int(nullable: false),
                        QuestionNumber = c.Int(nullable: false),
                        Result = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.webpages_Membership",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        CreateDate = c.DateTime(),
                        ConfirmationToken = c.String(maxLength: 128),
                        IsConfirmed = c.Boolean(),
                        LastPasswordFailureDate = c.DateTime(),
                        PasswordFailuresSinceLastSuccess = c.Int(nullable: false),
                        Password = c.String(nullable: false, maxLength: 128),
                        PasswordChangedDate = c.DateTime(),
                        PasswordSalt = c.String(nullable: false, maxLength: 128),
                        PasswordVerificationToken = c.String(maxLength: 128),
                        PasswordVerificationTokenExpirationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.webpages_Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(maxLength: 256),
                        RoleDisplayName = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        IsServiced = c.Boolean(),
                        LastLogin = c.DateTime(),
                        Attendance = c.String(),
                        Avatar = c.String(),
                        SkypeContact = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        About = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Concept",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Container = c.String(),
                        ParentId = c.Int(),
                        IsGroup = c.Boolean(nullable: false),
                        ReadOnly = c.Boolean(nullable: false),
                        NextConcept = c.Int(),
                        PrevConcept = c.Int(),
                        Published = c.Boolean(nullable: false),
                        SubjectId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        LectureId = c.Int(),
                        PracticalId = c.Int(),
                        LabId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subjects", t => t.SubjectId)
                .ForeignKey("dbo.Concept", t => t.ParentId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.SubjectId)
                .Index(t => t.ParentId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ConceptQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConceptId = c.Int(nullable: false),
                        QuestionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .ForeignKey("dbo.Concept", t => t.ConceptId, cascadeDelete: true)
                .Index(t => t.QuestionId)
                .Index(t => t.ConceptId);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionNumber = c.Int(),
                        TestId = c.Int(nullable: false),
                        Title = c.String(nullable: false),
                        Description = c.String(),
                        ComlexityLevel = c.Int(nullable: false),
                        ConceptId = c.Int(),
                        QuestionType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tests", t => t.TestId, cascadeDelete: true)
                .Index(t => t.TestId);
            
            CreateTable(
                "dbo.Tests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TestNumber = c.Int(),
                        Title = c.String(nullable: false),
                        Description = c.String(),
                        TimeForCompleting = c.Int(nullable: false),
                        SetTimeForAllTest = c.Boolean(nullable: false),
                        SubjectId = c.Int(nullable: false),
                        CountOfQuestions = c.Int(nullable: false),
                        IsNecessary = c.Boolean(nullable: false),
                        ForSelfStudy = c.Boolean(nullable: false),
                        BeforeEUMK = c.Boolean(nullable: false),
                        ForEUMK = c.Boolean(nullable: false),
                        ForNN = c.Boolean(nullable: false),
                        Data = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.SubjectId);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ShortName = c.String(),
                        Color = c.String(),
                        IsArchive = c.Boolean(nullable: false),
                        IsNeededCopyToBts = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                    "dbo.SubjectGroups",
                    c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupId = c.Int(nullable: false),
                        SubjectId = c.Int(nullable: false),
                        IsActiveOnCurrentGroup = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.GroupId)
                .ForeignKey("dbo.Subjects", t => t.SubjectId)
                .Index(t => t.GroupId)
                .Index(t => t.SubjectId);
                

            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StartYear = c.String(),
                        GraduationYear = c.String(),
                        Secretary_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lecturers", t => t.Secretary_Id)
                .Index(t => t.Secretary_Id);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        Email = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        MiddleName = c.String(),
                        Confirmed = c.Boolean(),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Groups", t => t.GroupId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.GroupId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.SubjectStudents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.Int(nullable: false),
                        SubGroupId = c.Int(nullable: false),
                        SubjectGroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SubGroups", t => t.SubGroupId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .ForeignKey("dbo.SubjectGroups", t => t.SubjectGroupId, cascadeDelete: false)
                .Index(t => t.SubGroupId)
                .Index(t => t.StudentId)
                .Index(t => t.SubjectGroupId);
            
            CreateTable(
                "dbo.SubGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SubjectGroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SubjectGroups", t => t.SubjectGroupId, cascadeDelete: true)
                .Index(t => t.SubjectGroupId);
            
            CreateTable(
                "dbo.ScheduleProtectionLabs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        SuGroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SubGroups", t => t.SuGroupId, cascadeDelete: true)
                .Index(t => t.SuGroupId);
            
            CreateTable(
                "dbo.ScheduleProtectionLabMarks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ScheduleProtectionLabId = c.Int(nullable: false),
                        StudentId = c.Int(nullable: false),
                        Comment = c.String(),
                        Mark = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ScheduleProtectionLabs", t => t.ScheduleProtectionLabId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.ScheduleProtectionLabId)
                .Index(t => t.StudentId);
            
            CreateTable(
                "dbo.TestUnlocks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TestId = c.Int(nullable: false),
                        StudentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .ForeignKey("dbo.Tests", t => t.TestId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.TestId);
            
            CreateTable(
                "dbo.AssignedDiplomProjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.Int(nullable: false),
                        DiplomProjectId = c.Int(nullable: false),
                        ApproveDate = c.DateTime(),
                        Mark = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DiplomProjects", t => t.DiplomProjectId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .Index(t => t.DiplomProjectId)
                .Index(t => t.StudentId);
            
            CreateTable(
                "dbo.DiplomProjects",
                c => new
                    {
                        DiplomProjectId = c.Int(nullable: false, identity: true),
                        Theme = c.String(nullable: false, maxLength: 2048),
                        LecturerId = c.Int(),
                        InputData = c.String(),
                        RpzContent = c.String(),
                        DrawMaterials = c.String(),
                        Consultants = c.String(),
                        Workflow = c.String(),
                        Univer = c.String(),
                        Faculty = c.String(),
                        HeadCathedra = c.String(),
                        DateEnd = c.DateTime(),
                        DateStart = c.DateTime(),
                    })
                .PrimaryKey(t => t.DiplomProjectId)
                .ForeignKey("dbo.Lecturers", t => t.LecturerId)
                .Index(t => t.LecturerId);
            
            CreateTable(
                "dbo.DiplomProjectGroups",
                c => new
                    {
                        DiplomProjectGroupId = c.Int(nullable: false, identity: true),
                        DiplomProjectId = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DiplomProjectGroupId)
                .ForeignKey("dbo.DiplomProjects", t => t.DiplomProjectId, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.DiplomProjectId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.Lecturers",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        MiddleName = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        Skill = c.String(),
                        IsSecretary = c.Boolean(nullable: false),
                        IsLecturerHasGraduateStudents = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Id, cascadeDelete: true)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.SubjectLecturers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LecturerId = c.Int(nullable: false),
                        SubjectId = c.Int(nullable: false),
                        Owner = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lecturers", t => t.LecturerId)
                .ForeignKey("dbo.Subjects", t => t.SubjectId)
                .Index(t => t.LecturerId)
                .Index(t => t.SubjectId);
            
            CreateTable(
                "dbo.DiplomPercentagesGraph",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LecturerId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Percentage = c.Double(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lecturers", t => t.LecturerId, cascadeDelete: true)
                .Index(t => t.LecturerId);
            
            CreateTable(
                "dbo.DiplomPercentagesResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DiplomPercentagesGraphId = c.Int(nullable: false),
                        StudentId = c.Int(nullable: false),
                        Mark = c.Int(),
                        Comments = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DiplomPercentagesGraph", t => t.DiplomPercentagesGraphId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .Index(t => t.DiplomPercentagesGraphId)
                .Index(t => t.StudentId);
            
            CreateTable(
                "dbo.DiplomPercentagesGraphToGroups",
                c => new
                    {
                        DiplomPercentagesGraphToGroupId = c.Int(nullable: false, identity: true),
                        DiplomPercentagesGraphId = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DiplomPercentagesGraphToGroupId)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.DiplomPercentagesGraph", t => t.DiplomPercentagesGraphId, cascadeDelete: true)
                .Index(t => t.GroupId)
                .Index(t => t.DiplomPercentagesGraphId);
            
            CreateTable(
                "dbo.DiplomProjectConsultationDates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LecturerId = c.Int(nullable: false),
                        Day = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lecturers", t => t.LecturerId, cascadeDelete: true)
                .Index(t => t.LecturerId);
            
            CreateTable(
                "dbo.DiplomProjectConsultationMarks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConsultationDateId = c.Int(nullable: false),
                        StudentId = c.Int(nullable: false),
                        Mark = c.String(maxLength: 2, fixedLength: true, unicode: false),
                        Comments = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DiplomProjectConsultationDates", t => t.ConsultationDateId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .Index(t => t.ConsultationDateId)
                .Index(t => t.StudentId);
            
            CreateTable(
                "dbo.CourseProjects",
                c => new
                    {
                        CourseProjectId = c.Int(nullable: false, identity: true),
                        Theme = c.String(nullable: false, maxLength: 2048),
                        LecturerId = c.Int(),
                        InputData = c.String(),
                        Univer = c.String(),
                        Faculty = c.String(),
                        HeadCathedra = c.String(),
                        RpzContent = c.String(),
                        DrawMaterials = c.String(),
                        Consultants = c.String(),
                        Workflow = c.String(),
                        DateEnd = c.DateTime(),
                        DateStart = c.DateTime(),
                        SubjectId = c.Int(),
                    })
                .PrimaryKey(t => t.CourseProjectId)
                .ForeignKey("dbo.Subjects", t => t.SubjectId)
                .ForeignKey("dbo.Lecturers", t => t.LecturerId)
                .Index(t => t.SubjectId)
                .Index(t => t.LecturerId);
            
            CreateTable(
                "dbo.AssignedCourseProjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.Int(nullable: false),
                        CourseProjectId = c.Int(nullable: false),
                        ApproveDate = c.DateTime(),
                        Mark = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CourseProjects", t => t.CourseProjectId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.CourseProjectId)
                .Index(t => t.StudentId);
            
            CreateTable(
                "dbo.CourseProjectGroups",
                c => new
                    {
                        CourseProjectGroupId = c.Int(nullable: false, identity: true),
                        CourseProjectId = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CourseProjectGroupId)
                .ForeignKey("dbo.CourseProjects", t => t.CourseProjectId, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.CourseProjectId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.CoursePercentagesGraph",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LecturerId = c.Int(nullable: false),
                        SubjectId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Percentage = c.Double(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lecturers", t => t.LecturerId, cascadeDelete: true)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.LecturerId)
                .Index(t => t.SubjectId);
            
            CreateTable(
                "dbo.CoursePercentagesResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CoursePercentagesGraphId = c.Int(nullable: false),
                        StudentId = c.Int(nullable: false),
                        Mark = c.Int(),
                        Comments = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CoursePercentagesGraph", t => t.CoursePercentagesGraphId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: false)
                .Index(t => t.CoursePercentagesGraphId)
                .Index(t => t.StudentId);
            
            CreateTable(
                "dbo.CoursePercentagesGraphToGroups",
                c => new
                    {
                        CoursePercentagesGraphToGroupId = c.Int(nullable: false, identity: true),
                        CoursePercentagesGraphId = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CoursePercentagesGraphToGroupId)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.CoursePercentagesGraph", t => t.CoursePercentagesGraphId, cascadeDelete: true)
                .Index(t => t.GroupId)
                .Index(t => t.CoursePercentagesGraphId);
            
            CreateTable(
                "dbo.CourseProjectConsultationDates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LecturerId = c.Int(nullable: false),
                        SubjectId = c.Int(),
                        Day = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subjects", t => t.SubjectId)
                .ForeignKey("dbo.Lecturers", t => t.LecturerId, cascadeDelete: true)
                .Index(t => t.SubjectId)
                .Index(t => t.LecturerId);
            
            CreateTable(
                "dbo.CourseProjectConsultationMarks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConsultationDateId = c.Int(nullable: false),
                        StudentId = c.Int(nullable: false),
                        Mark = c.String(maxLength: 2),
                        Comments = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .ForeignKey("dbo.CourseProjectConsultationDates", t => t.ConsultationDateId, cascadeDelete: false)
                .Index(t => t.StudentId)
                .Index(t => t.ConsultationDateId);
            
            CreateTable(
                "dbo.LecturesVisitMarks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.Int(nullable: false),
                        Mark = c.String(),
                        LecturesScheduleVisitingId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LecturesScheduleVisitings", t => t.LecturesScheduleVisitingId)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.LecturesScheduleVisitingId)
                .Index(t => t.StudentId);
            
            CreateTable(
                "dbo.LecturesScheduleVisitings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        SubjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subjects", t => t.SubjectId)
                .Index(t => t.SubjectId);
            
            CreateTable(
                "dbo.StudentLabMarks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LabId = c.Int(nullable: false),
                        StudentId = c.Int(nullable: false),
                        Mark = c.String(),
                        Comment = c.String(),
                        Date = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Labs", t => t.LabId)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.LabId)
                .Index(t => t.StudentId);
            
            CreateTable(
                "dbo.Labs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Theme = c.String(),
                        Duration = c.Int(nullable: false),
                        SubjectId = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        ShortName = c.String(),
                        Attachments = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subjects", t => t.SubjectId)
                .Index(t => t.SubjectId);
            
            CreateTable(
                "dbo.ScheduleProtectionPracticalMarks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ScheduleProtectionPracticalId = c.Int(nullable: false),
                        StudentId = c.Int(nullable: false),
                        Comment = c.String(),
                        Mark = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ScheduleProtectionPracticals", t => t.ScheduleProtectionPracticalId)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.ScheduleProtectionPracticalId)
                .Index(t => t.StudentId);
            
            CreateTable(
                "dbo.ScheduleProtectionPracticals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        GroupId = c.Int(nullable: false),
                        SubjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.GroupId)
                .ForeignKey("dbo.Subjects", t => t.SubjectId)
                .Index(t => t.GroupId)
                .Index(t => t.SubjectId);
            
            CreateTable(
                "dbo.StudentPracticalMarks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PracticalId = c.Int(nullable: false),
                        StudentId = c.Int(nullable: false),
                        Mark = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Practicals", t => t.PracticalId)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.PracticalId)
                .Index(t => t.StudentId);
            
            CreateTable(
                "dbo.Practicals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Theme = c.String(),
                        Duration = c.Int(nullable: false),
                        SubjectId = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        ShortName = c.String(),
                        Attachments = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subjects", t => t.SubjectId)
                .Index(t => t.SubjectId);
            
            CreateTable(
                "dbo.SubjectModules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubjectId = c.Int(nullable: false),
                        ModuleId = c.Int(nullable: false),
                        IsVisible = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Modules", t => t.ModuleId)
                .ForeignKey("dbo.Subjects", t => t.SubjectId)
                .Index(t => t.ModuleId)
                .Index(t => t.SubjectId);
            
            CreateTable(
                "dbo.Modules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DisplayName = c.String(),
                        Visible = c.Boolean(nullable: false),
                        ModuleType = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Folders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Pid = c.Int(nullable: false),
                        SubjectModuleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SubjectModules", t => t.SubjectModuleId)
                .Index(t => t.SubjectModuleId);
            
            CreateTable(
                "dbo.Materials",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 128),
                        Text = c.String(),
                        Folders_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Folders", t => t.Folders_Id)
                .Index(t => t.Folders_Id);
            
            CreateTable(
                "dbo.SubjectNewses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Body = c.String(),
                        Disabled = c.Boolean(nullable: false),
                        EditDate = c.DateTime(nullable: false),
                        SubjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.SubjectId);
            
            CreateTable(
                "dbo.Lectures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Theme = c.String(),
                        Duration = c.Int(nullable: false),
                        SubjectId = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        Attachments = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subjects", t => t.SubjectId)
                .Index(t => t.SubjectId);
            
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionId = c.Int(nullable: false),
                        Content = c.String(nullable: false),
                        СorrectnessIndicator = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.ProjectUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ProjectRoleId = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.ProjectRoles", t => t.ProjectRoleId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.ProjectId)
                .Index(t => t.ProjectRoleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Details = c.String(),
                        DateOfChange = c.DateTime(nullable: false),
                        CreatorId = c.Int(nullable: false),
                        Attachments = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .Index(t => t.CreatorId);
            
            CreateTable(
                "dbo.ProjectComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommentText = c.String(),
                        UserId = c.Int(nullable: false),
                        CommentingDate = c.DateTime(nullable: false),
                        ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ProjectId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Bugs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        Summary = c.String(),
                        Description = c.String(),
                        Steps = c.String(),
                        ExpectedResult = c.String(),
                        ReportingDate = c.DateTime(nullable: false),
                        ModifyingDate = c.DateTime(nullable: false),
                        SymptomId = c.Int(nullable: false),
                        SeverityId = c.Int(nullable: false),
                        StatusId = c.Int(nullable: false),
                        ReporterId = c.Int(nullable: false),
                        EditorId = c.Int(nullable: false),
                        AssignedDeveloperId = c.Int(nullable: false),
                        Attachments = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.ReporterId)
                .ForeignKey("dbo.Users", t => t.AssignedDeveloperId)
                .ForeignKey("dbo.BugSymptoms", t => t.SymptomId)
                .ForeignKey("dbo.BugSeverities", t => t.SeverityId)
                .ForeignKey("dbo.BugStatuses", t => t.StatusId)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ReporterId)
                .Index(t => t.AssignedDeveloperId)
                .Index(t => t.SymptomId)
                .Index(t => t.SeverityId)
                .Index(t => t.StatusId)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.BugSymptoms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BugSeverities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BugStatuses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BugLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        UserName = c.String(),
                        LogDate = c.DateTime(nullable: false),
                        BugId = c.Int(nullable: false),
                        PrevStatusId = c.Int(nullable: false),
                        CurrStatusId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bugs", t => t.BugId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.BugId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ProjectMatrixRequirements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Number = c.String(),
                        Covered = c.Boolean(nullable: false),
                        ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.ProjectRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AnswerOnTestQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        TestId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        QuestionId = c.Int(nullable: false),
                        Points = c.Int(nullable: false),
                        TestEnded = c.Boolean(nullable: false),
                        AnswerString = c.String(),
                        Time = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.TestPassResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.Int(nullable: false),
                        TestId = c.Int(nullable: false),
                        Points = c.Int(),
                        Percent = c.Int(),
                        StartTime = c.DateTime(nullable: false),
                        Comment = c.String(),
                        CalculationType = c.Int(),
                        TestName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.StudentId);
            
            CreateTable(
                "dbo.UserMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RecipientId = c.Int(nullable: false),
                        AuthorId = c.Int(nullable: false),
                        MessageId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        DeletedById = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.RecipientId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.AuthorId, cascadeDelete: false)
                .ForeignKey("dbo.Messages", t => t.MessageId, cascadeDelete: true)
                .Index(t => t.RecipientId)
                .Index(t => t.AuthorId)
                .Index(t => t.MessageId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Subject = c.String(),
                        AttachmentsPath = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FileName = c.String(),
                        PathName = c.String(),
                        AttachmentType = c.Int(nullable: false),
                        Message_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Messages", t => t.Message_Id)
                .Index(t => t.Message_Id);
            
            CreateTable(
                "dbo.webpages_OAuthMembership",
                c => new
                    {
                        Provider = c.String(nullable: false, maxLength: 30),
                        ProviderUserId = c.String(nullable: false, maxLength: 100),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Provider, t.ProviderUserId })
                .ForeignKey("dbo.webpages_Membership", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ScoObjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Path = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        Enabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TinCanObjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Path = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        Enabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserLabFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Comments = c.String(),
                        Attachments = c.String(),
                        UserId = c.Int(nullable: false),
                        SubjectId = c.Int(nullable: false),
                        Date = c.DateTime(),
                        IsReceived = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AccessCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DiplomProjectNews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Body = c.String(),
                        Disabled = c.Boolean(nullable: false),
                        EditDate = c.DateTime(nullable: false),
                        Attachments = c.String(),
                        LecturerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lecturers", t => t.LecturerId, cascadeDelete: true)
                .Index(t => t.LecturerId);
            
            CreateTable(
                "dbo.DiplomProjectTaskSheetTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LecturerId = c.Int(nullable: false),
                        Name = c.String(),
                        InputData = c.String(),
                        RpzContent = c.String(),
                        DrawMaterials = c.String(),
                        Consultants = c.String(),
                        Faculty = c.String(),
                        Univer = c.String(),
                        HeadCathedra = c.String(),
                        DateEnd = c.DateTime(),
                        DateStart = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CourseProjectNews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Body = c.String(),
                        Disabled = c.Boolean(nullable: false),
                        EditDate = c.DateTime(nullable: false),
                        Attachments = c.String(),
                        SubjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.SubjectId);
            
            CreateTable(
                "dbo.CourseProjectTaskSheetTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LecturerId = c.Int(nullable: false),
                        Name = c.String(),
                        InputData = c.String(),
                        Faculty = c.String(),
                        Univer = c.String(),
                        HeadCathedra = c.String(),
                        RpzContent = c.String(),
                        DrawMaterials = c.String(),
                        Consultants = c.String(),
                        DateEnd = c.DateTime(),
                        DateStart = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.webpages_UsersInRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.webpages_Membership", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.webpages_Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.webpages_UsersInRoles", new[] { "RoleId" });
            DropIndex("dbo.webpages_UsersInRoles", new[] { "UserId" });
            DropIndex("dbo.CourseProjectNews", new[] { "SubjectId" });
            DropIndex("dbo.DiplomProjectNews", new[] { "LecturerId" });
            DropIndex("dbo.webpages_OAuthMembership", new[] { "UserId" });
            DropIndex("dbo.Attachments", new[] { "Message_Id" });
            DropIndex("dbo.UserMessages", new[] { "MessageId" });
            DropIndex("dbo.UserMessages", new[] { "AuthorId" });
            DropIndex("dbo.UserMessages", new[] { "RecipientId" });
            DropIndex("dbo.TestPassResults", new[] { "StudentId" });
            DropIndex("dbo.AnswerOnTestQuestions", new[] { "UserId" });
            DropIndex("dbo.ProjectMatrixRequirements", new[] { "ProjectId" });
            DropIndex("dbo.BugLogs", new[] { "UserId" });
            DropIndex("dbo.BugLogs", new[] { "BugId" });
            DropIndex("dbo.Bugs", new[] { "ProjectId" });
            DropIndex("dbo.Bugs", new[] { "StatusId" });
            DropIndex("dbo.Bugs", new[] { "SeverityId" });
            DropIndex("dbo.Bugs", new[] { "SymptomId" });
            DropIndex("dbo.Bugs", new[] { "AssignedDeveloperId" });
            DropIndex("dbo.Bugs", new[] { "ReporterId" });
            DropIndex("dbo.ProjectComments", new[] { "UserId" });
            DropIndex("dbo.ProjectComments", new[] { "ProjectId" });
            DropIndex("dbo.Projects", new[] { "CreatorId" });
            DropIndex("dbo.ProjectUsers", new[] { "UserId" });
            DropIndex("dbo.ProjectUsers", new[] { "ProjectRoleId" });
            DropIndex("dbo.ProjectUsers", new[] { "ProjectId" });
            DropIndex("dbo.Answers", new[] { "QuestionId" });
            DropIndex("dbo.Lectures", new[] { "SubjectId" });
            DropIndex("dbo.SubjectNewses", new[] { "SubjectId" });
            DropIndex("dbo.Materials", new[] { "Folders_Id" });
            DropIndex("dbo.Folders", new[] { "SubjectModuleId" });
            DropIndex("dbo.SubjectModules", new[] { "SubjectId" });
            DropIndex("dbo.SubjectModules", new[] { "ModuleId" });
            DropIndex("dbo.Practicals", new[] { "SubjectId" });
            DropIndex("dbo.StudentPracticalMarks", new[] { "StudentId" });
            DropIndex("dbo.StudentPracticalMarks", new[] { "PracticalId" });
            DropIndex("dbo.ScheduleProtectionPracticals", new[] { "SubjectId" });
            DropIndex("dbo.ScheduleProtectionPracticals", new[] { "GroupId" });
            DropIndex("dbo.ScheduleProtectionPracticalMarks", new[] { "StudentId" });
            DropIndex("dbo.ScheduleProtectionPracticalMarks", new[] { "ScheduleProtectionPracticalId" });
            DropIndex("dbo.Labs", new[] { "SubjectId" });
            DropIndex("dbo.StudentLabMarks", new[] { "StudentId" });
            DropIndex("dbo.StudentLabMarks", new[] { "LabId" });
            DropIndex("dbo.LecturesScheduleVisitings", new[] { "SubjectId" });
            DropIndex("dbo.LecturesVisitMarks", new[] { "StudentId" });
            DropIndex("dbo.LecturesVisitMarks", new[] { "LecturesScheduleVisitingId" });
            DropIndex("dbo.CourseProjectConsultationMarks", new[] { "ConsultationDateId" });
            DropIndex("dbo.CourseProjectConsultationMarks", new[] { "StudentId" });
            DropIndex("dbo.CourseProjectConsultationDates", new[] { "LecturerId" });
            DropIndex("dbo.CourseProjectConsultationDates", new[] { "SubjectId" });
            DropIndex("dbo.CoursePercentagesGraphToGroups", new[] { "CoursePercentagesGraphId" });
            DropIndex("dbo.CoursePercentagesGraphToGroups", new[] { "GroupId" });
            DropIndex("dbo.CoursePercentagesResults", new[] { "StudentId" });
            DropIndex("dbo.CoursePercentagesResults", new[] { "CoursePercentagesGraphId" });
            DropIndex("dbo.CoursePercentagesGraph", new[] { "SubjectId" });
            DropIndex("dbo.CoursePercentagesGraph", new[] { "LecturerId" });
            DropIndex("dbo.CourseProjectGroups", new[] { "GroupId" });
            DropIndex("dbo.CourseProjectGroups", new[] { "CourseProjectId" });
            DropIndex("dbo.AssignedCourseProjects", new[] { "StudentId" });
            DropIndex("dbo.AssignedCourseProjects", new[] { "CourseProjectId" });
            DropIndex("dbo.CourseProjects", new[] { "LecturerId" });
            DropIndex("dbo.CourseProjects", new[] { "SubjectId" });
            DropIndex("dbo.DiplomProjectConsultationMarks", new[] { "StudentId" });
            DropIndex("dbo.DiplomProjectConsultationMarks", new[] { "ConsultationDateId" });
            DropIndex("dbo.DiplomProjectConsultationDates", new[] { "LecturerId" });
            DropIndex("dbo.DiplomPercentagesGraphToGroups", new[] { "DiplomPercentagesGraphId" });
            DropIndex("dbo.DiplomPercentagesGraphToGroups", new[] { "GroupId" });
            DropIndex("dbo.DiplomPercentagesResults", new[] { "StudentId" });
            DropIndex("dbo.DiplomPercentagesResults", new[] { "DiplomPercentagesGraphId" });
            DropIndex("dbo.DiplomPercentagesGraph", new[] { "LecturerId" });
            DropIndex("dbo.SubjectLecturers", new[] { "SubjectId" });
            DropIndex("dbo.SubjectLecturers", new[] { "LecturerId" });
            DropIndex("dbo.Lecturers", new[] { "Id" });
            DropIndex("dbo.DiplomProjectGroups", new[] { "GroupId" });
            DropIndex("dbo.DiplomProjectGroups", new[] { "DiplomProjectId" });
            DropIndex("dbo.DiplomProjects", new[] { "LecturerId" });
            DropIndex("dbo.AssignedDiplomProjects", new[] { "StudentId" });
            DropIndex("dbo.AssignedDiplomProjects", new[] { "DiplomProjectId" });
            DropIndex("dbo.TestUnlocks", new[] { "TestId" });
            DropIndex("dbo.TestUnlocks", new[] { "StudentId" });
            DropIndex("dbo.ScheduleProtectionLabMarks", new[] { "StudentId" });
            DropIndex("dbo.ScheduleProtectionLabMarks", new[] { "ScheduleProtectionLabId" });
            DropIndex("dbo.ScheduleProtectionLabs", new[] { "SuGroupId" });
            DropIndex("dbo.SubGroups", new[] { "SubjectGroupId" });
            DropIndex("dbo.SubjectStudents", new[] { "SubjectGroupId" });
            DropIndex("dbo.SubjectStudents", new[] { "StudentId" });
            DropIndex("dbo.SubjectStudents", new[] { "SubGroupId" });
            DropIndex("dbo.Students", new[] { "UserId" });
            DropIndex("dbo.Students", new[] { "GroupId" });
            DropIndex("dbo.Groups", new[] { "Secretary_Id" });
            DropIndex("dbo.SubjectGroups", new[] { "SubjectId" });
            DropIndex("dbo.SubjectGroups", new[] { "GroupId" });
            DropIndex("dbo.Tests", new[] { "SubjectId" });
            DropIndex("dbo.Questions", new[] { "TestId" });
            DropIndex("dbo.ConceptQuestions", new[] { "ConceptId" });
            DropIndex("dbo.ConceptQuestions", new[] { "QuestionId" });
            DropIndex("dbo.Concept", new[] { "UserId" });
            DropIndex("dbo.Concept", new[] { "ParentId" });
            DropIndex("dbo.Concept", new[] { "SubjectId" });
            DropIndex("dbo.webpages_Membership", new[] { "UserId" });
            DropForeignKey("dbo.webpages_UsersInRoles", "RoleId", "dbo.webpages_Roles");
            DropForeignKey("dbo.webpages_UsersInRoles", "UserId", "dbo.webpages_Membership");
            DropForeignKey("dbo.CourseProjectNews", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.DiplomProjectNews", "LecturerId", "dbo.Lecturers");
            DropForeignKey("dbo.webpages_OAuthMembership", "UserId", "dbo.webpages_Membership");
            DropForeignKey("dbo.Attachments", "Message_Id", "dbo.Messages");
            DropForeignKey("dbo.UserMessages", "MessageId", "dbo.Messages");
            DropForeignKey("dbo.UserMessages", "AuthorId", "dbo.Users");
            DropForeignKey("dbo.UserMessages", "RecipientId", "dbo.Users");
            DropForeignKey("dbo.TestPassResults", "StudentId", "dbo.Users");
            DropForeignKey("dbo.AnswerOnTestQuestions", "UserId", "dbo.Users");
            DropForeignKey("dbo.ProjectMatrixRequirements", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.BugLogs", "UserId", "dbo.Users");
            DropForeignKey("dbo.BugLogs", "BugId", "dbo.Bugs");
            DropForeignKey("dbo.Bugs", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Bugs", "StatusId", "dbo.BugStatuses");
            DropForeignKey("dbo.Bugs", "SeverityId", "dbo.BugSeverities");
            DropForeignKey("dbo.Bugs", "SymptomId", "dbo.BugSymptoms");
            DropForeignKey("dbo.Bugs", "AssignedDeveloperId", "dbo.Users");
            DropForeignKey("dbo.Bugs", "ReporterId", "dbo.Users");
            DropForeignKey("dbo.ProjectComments", "UserId", "dbo.Users");
            DropForeignKey("dbo.ProjectComments", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Projects", "CreatorId", "dbo.Users");
            DropForeignKey("dbo.ProjectUsers", "UserId", "dbo.Users");
            DropForeignKey("dbo.ProjectUsers", "ProjectRoleId", "dbo.ProjectRoles");
            DropForeignKey("dbo.ProjectUsers", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Answers", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Lectures", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.SubjectNewses", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.Materials", "Folders_Id", "dbo.Folders");
            DropForeignKey("dbo.Folders", "SubjectModuleId", "dbo.SubjectModules");
            DropForeignKey("dbo.SubjectModules", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.SubjectModules", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.Practicals", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.StudentPracticalMarks", "StudentId", "dbo.Students");
            DropForeignKey("dbo.StudentPracticalMarks", "PracticalId", "dbo.Practicals");
            DropForeignKey("dbo.ScheduleProtectionPracticals", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.ScheduleProtectionPracticals", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.ScheduleProtectionPracticalMarks", "StudentId", "dbo.Students");
            DropForeignKey("dbo.ScheduleProtectionPracticalMarks", "ScheduleProtectionPracticalId", "dbo.ScheduleProtectionPracticals");
            DropForeignKey("dbo.Labs", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.StudentLabMarks", "StudentId", "dbo.Students");
            DropForeignKey("dbo.StudentLabMarks", "LabId", "dbo.Labs");
            DropForeignKey("dbo.LecturesScheduleVisitings", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.LecturesVisitMarks", "StudentId", "dbo.Students");
            DropForeignKey("dbo.LecturesVisitMarks", "LecturesScheduleVisitingId", "dbo.LecturesScheduleVisitings");
            DropForeignKey("dbo.CourseProjectConsultationMarks", "ConsultationDateId", "dbo.CourseProjectConsultationDates");
            DropForeignKey("dbo.CourseProjectConsultationMarks", "StudentId", "dbo.Students");
            DropForeignKey("dbo.CourseProjectConsultationDates", "LecturerId", "dbo.Lecturers");
            DropForeignKey("dbo.CourseProjectConsultationDates", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.CoursePercentagesGraphToGroups", "CoursePercentagesGraphId", "dbo.CoursePercentagesGraph");
            DropForeignKey("dbo.CoursePercentagesGraphToGroups", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.CoursePercentagesResults", "StudentId", "dbo.Students");
            DropForeignKey("dbo.CoursePercentagesResults", "CoursePercentagesGraphId", "dbo.CoursePercentagesGraph");
            DropForeignKey("dbo.CoursePercentagesGraph", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.CoursePercentagesGraph", "LecturerId", "dbo.Lecturers");
            DropForeignKey("dbo.CourseProjectGroups", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.CourseProjectGroups", "CourseProjectId", "dbo.CourseProjects");
            DropForeignKey("dbo.AssignedCourseProjects", "StudentId", "dbo.Students");
            DropForeignKey("dbo.AssignedCourseProjects", "CourseProjectId", "dbo.CourseProjects");
            DropForeignKey("dbo.CourseProjects", "LecturerId", "dbo.Lecturers");
            DropForeignKey("dbo.CourseProjects", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.DiplomProjectConsultationMarks", "StudentId", "dbo.Students");
            DropForeignKey("dbo.DiplomProjectConsultationMarks", "ConsultationDateId", "dbo.DiplomProjectConsultationDates");
            DropForeignKey("dbo.DiplomProjectConsultationDates", "LecturerId", "dbo.Lecturers");
            DropForeignKey("dbo.DiplomPercentagesGraphToGroups", "DiplomPercentagesGraphId", "dbo.DiplomPercentagesGraph");
            DropForeignKey("dbo.DiplomPercentagesGraphToGroups", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.DiplomPercentagesResults", "StudentId", "dbo.Students");
            DropForeignKey("dbo.DiplomPercentagesResults", "DiplomPercentagesGraphId", "dbo.DiplomPercentagesGraph");
            DropForeignKey("dbo.DiplomPercentagesGraph", "LecturerId", "dbo.Lecturers");
            DropForeignKey("dbo.SubjectLecturers", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.SubjectLecturers", "LecturerId", "dbo.Lecturers");
            DropForeignKey("dbo.Lecturers", "Id", "dbo.Users");
            DropForeignKey("dbo.DiplomProjectGroups", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.DiplomProjectGroups", "DiplomProjectId", "dbo.DiplomProjects");
            DropForeignKey("dbo.DiplomProjects", "LecturerId", "dbo.Lecturers");
            DropForeignKey("dbo.AssignedDiplomProjects", "StudentId", "dbo.Students");
            DropForeignKey("dbo.AssignedDiplomProjects", "DiplomProjectId", "dbo.DiplomProjects");
            DropForeignKey("dbo.TestUnlocks", "TestId", "dbo.Tests");
            DropForeignKey("dbo.TestUnlocks", "StudentId", "dbo.Students");
            DropForeignKey("dbo.ScheduleProtectionLabMarks", "StudentId", "dbo.Students");
            DropForeignKey("dbo.ScheduleProtectionLabMarks", "ScheduleProtectionLabId", "dbo.ScheduleProtectionLabs");
            DropForeignKey("dbo.ScheduleProtectionLabs", "SuGroupId", "dbo.SubGroups");
            DropForeignKey("dbo.SubGroups", "SubjectGroupId", "dbo.SubjectGroups");
            DropForeignKey("dbo.SubjectStudents", "SubjectGroupId", "dbo.SubjectGroups");
            DropForeignKey("dbo.SubjectStudents", "StudentId", "dbo.Students");
            DropForeignKey("dbo.SubjectStudents", "SubGroupId", "dbo.SubGroups");
            DropForeignKey("dbo.Students", "UserId", "dbo.Users");
            DropForeignKey("dbo.Students", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Groups", "Secretary_Id", "dbo.Lecturers");
            DropForeignKey("dbo.SubjectGroups", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.SubjectGroups", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Tests", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.Questions", "TestId", "dbo.Tests");
            DropForeignKey("dbo.ConceptQuestions", "ConceptId", "dbo.Concept");
            DropForeignKey("dbo.ConceptQuestions", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Concept", "UserId", "dbo.Users");
            DropForeignKey("dbo.Concept", "ParentId", "dbo.Concept");
            DropForeignKey("dbo.Concept", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.webpages_Membership", "UserId", "dbo.Users");
            DropTable("dbo.webpages_UsersInRoles");
            DropTable("dbo.CourseProjectTaskSheetTemplates");
            DropTable("dbo.CourseProjectNews");
            DropTable("dbo.DiplomProjectTaskSheetTemplates");
            DropTable("dbo.DiplomProjectNews");
            DropTable("dbo.AccessCodes");
            DropTable("dbo.UserLabFiles");
            DropTable("dbo.TinCanObjects");
            DropTable("dbo.ScoObjects");
            DropTable("dbo.webpages_OAuthMembership");
            DropTable("dbo.Attachments");
            DropTable("dbo.Messages");
            DropTable("dbo.UserMessages");
            DropTable("dbo.TestPassResults");
            DropTable("dbo.AnswerOnTestQuestions");
            DropTable("dbo.ProjectRoles");
            DropTable("dbo.ProjectMatrixRequirements");
            DropTable("dbo.BugLogs");
            DropTable("dbo.BugStatuses");
            DropTable("dbo.BugSeverities");
            DropTable("dbo.BugSymptoms");
            DropTable("dbo.Bugs");
            DropTable("dbo.ProjectComments");
            DropTable("dbo.Projects");
            DropTable("dbo.ProjectUsers");
            DropTable("dbo.Answers");
            DropTable("dbo.Lectures");
            DropTable("dbo.SubjectNewses");
            DropTable("dbo.Materials");
            DropTable("dbo.Folders");
            DropTable("dbo.Modules");
            DropTable("dbo.SubjectModules");
            DropTable("dbo.Practicals");
            DropTable("dbo.StudentPracticalMarks");
            DropTable("dbo.ScheduleProtectionPracticals");
            DropTable("dbo.ScheduleProtectionPracticalMarks");
            DropTable("dbo.Labs");
            DropTable("dbo.StudentLabMarks");
            DropTable("dbo.LecturesScheduleVisitings");
            DropTable("dbo.LecturesVisitMarks");
            DropTable("dbo.CourseProjectConsultationMarks");
            DropTable("dbo.CourseProjectConsultationDates");
            DropTable("dbo.CoursePercentagesGraphToGroups");
            DropTable("dbo.CoursePercentagesResults");
            DropTable("dbo.CoursePercentagesGraph");
            DropTable("dbo.CourseProjectGroups");
            DropTable("dbo.AssignedCourseProjects");
            DropTable("dbo.CourseProjects");
            DropTable("dbo.DiplomProjectConsultationMarks");
            DropTable("dbo.DiplomProjectConsultationDates");
            DropTable("dbo.DiplomPercentagesGraphToGroups");
            DropTable("dbo.DiplomPercentagesResults");
            DropTable("dbo.DiplomPercentagesGraph");
            DropTable("dbo.SubjectLecturers");
            DropTable("dbo.Lecturers");
            DropTable("dbo.DiplomProjectGroups");
            DropTable("dbo.DiplomProjects");
            DropTable("dbo.AssignedDiplomProjects");
            DropTable("dbo.TestUnlocks");
            DropTable("dbo.ScheduleProtectionLabMarks");
            DropTable("dbo.ScheduleProtectionLabs");
            DropTable("dbo.SubGroups");
            DropTable("dbo.SubjectStudents");
            DropTable("dbo.Students");
            DropTable("dbo.Groups");
            DropTable("dbo.SubjectGroups");
            DropTable("dbo.Subjects");
            DropTable("dbo.Tests");
            DropTable("dbo.Questions");
            DropTable("dbo.ConceptQuestions");
            DropTable("dbo.Concept");
            DropTable("dbo.Users");
            DropTable("dbo.webpages_Roles");
            DropTable("dbo.webpages_Membership");
            DropTable("dbo.TestQuestionPassResults");
            DropTable("dbo.WatchingTime");
        }
    }
}
