
/*Курсовые проекты*/
CREATE TABLE [dbo].[CourseProjects] (
    [CourseProjectId] INT             IDENTITY (1, 1) NOT NULL,
    [Theme]           NVARCHAR (2048) NOT NULL,
    [LecturerId]      INT             NULL,
    [InputData]       NVARCHAR (MAX)  NULL,
    [RpzContent]      NVARCHAR (MAX)  NULL,
    [DrawMaterials]   NVARCHAR (MAX)  NULL,
    [Consultants]     NVARCHAR (MAX)  NULL,
    [Workflow]        NVARCHAR (MAX)  NULL,
    [SubjectId]       INT             NULL,
    CONSTRAINT [PK_dbo.CourseProjects] PRIMARY KEY CLUSTERED ([CourseProjectId] ASC),
    CONSTRAINT [FK_dbo.CourseProjects_dbo.Lecturers_LecturerId] FOREIGN KEY ([LecturerId]) REFERENCES [dbo].[Lecturers] ([Id]),
    CONSTRAINT [FK_dbo.CourseProjects_dbo.Subjects_SubjectId] FOREIGN KEY ([SubjectId]) REFERENCES [dbo].[Subjects] ([Id])
);

/*Курсовые проекты для группы*/
CREATE TABLE [dbo].[CourseProjectGroups] (
    [CourseProjectGroupId] INT IDENTITY (1, 1) NOT NULL,
    [CourseProjectId]      INT NOT NULL,
    [GroupId]              INT NOT NULL,
    CONSTRAINT [PK_dbo.CourseProjectGroups] PRIMARY KEY CLUSTERED ([CourseProjectGroupId] ASC),
    CONSTRAINT [FK_dbo.CourseProjectGroups_dbo.CourseProjects_CourseProjectId] FOREIGN KEY ([CourseProjectId]) REFERENCES [dbo].[CourseProjects] ([CourseProjectId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.CourseProjectGroups_dbo.Groups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Groups] ([Id]) ON DELETE CASCADE
);


/*Выбор курсового проекта студентом*/
CREATE TABLE [dbo].[AssignedCourseProjects] (
    [Id]              INT      IDENTITY (1, 1) NOT NULL,
    [StudentId]       INT      NOT NULL,
    [CourseProjectId] INT      NOT NULL,
    [ApproveDate]     DATETIME NULL,
    [Mark]            INT      NULL,
    CONSTRAINT [PK_dbo.AssignedCourseProjects] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.AssignedCourseProjects_dbo.CourseProjects_CourseProjectId] FOREIGN KEY ([CourseProjectId]) REFERENCES [dbo].[CourseProjects] ([CourseProjectId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.AssignedCourseProjects_dbo.Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [dbo].[Students] ([UserId])
);


/*Даты консультаций по курсовым проектам*/
CREATE TABLE [dbo].[CourseProjectConsultationDates] (
    [Id]         INT      IDENTITY (1, 1) NOT NULL,
    [LecturerId] INT      NOT NULL,
    [Day]        DATETIME NOT NULL,
    [SubjectId]  INT      NULL,
    CONSTRAINT [PK_dbo.CourseProjectConsultationDates] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.CourseProjectConsultationDates_dbo.Lecturers_LecturerId] FOREIGN KEY ([LecturerId]) REFERENCES [dbo].[Lecturers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.CourseProjectConsultationDates_dbo.Subjects_SubjectId] FOREIGN KEY ([SubjectId]) REFERENCES [dbo].[Subjects] ([Id])
);

/*Посещение консультаций*/
CREATE TABLE [dbo].[CourseProjectConsultationMarks] (
    [Id]                 INT           IDENTITY (1, 1) NOT NULL,
    [ConsultationDateId] INT           NOT NULL,
    [StudentId]          INT           NOT NULL,
    [Mark]               CHAR (2)      NULL,
    [Comments]           NVARCHAR (50) NULL,
    CONSTRAINT [PK_dbo.CourseProjectConsultationMarks] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.CourseProjectConsultationMarks_dbo.CourseProjectConsultationDates_ConsultationDateId] FOREIGN KEY ([ConsultationDateId]) REFERENCES [dbo].[CourseProjectConsultationDates] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.CourseProjectConsultationMarks_dbo.Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [dbo].[Students] ([UserId])
);

/*График процентовок*/
CREATE TABLE [dbo].[CoursePercentagesGraph] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [LecturerId] INT            NOT NULL,
    [Name]       NVARCHAR (100) NOT NULL,
    [Percentage] FLOAT (53)     NOT NULL,
    [Date]       DATETIME       NOT NULL,
    [SubjectId]  INT            NULL,
    CONSTRAINT [PK_dbo.CoursePercentagesGraph] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.CoursePercentagesGraph_dbo.Subjects_SubjectId] FOREIGN KEY ([SubjectId]) REFERENCES [dbo].[Subjects] ([Id]),
    CONSTRAINT [FK_dbo.CoursePercentagesGraph_dbo.Lecturers_LecturerId] FOREIGN KEY ([LecturerId]) REFERENCES [dbo].[Lecturers] ([Id]) ON DELETE CASCADE
);


/*График процентовок для группы*/
CREATE TABLE [dbo].[CoursePercentagesGraphToGroups] (
    [CoursePercentagesGraphToGroupId] INT IDENTITY (1, 1) NOT NULL,
    [CoursePercentagesGraphId]        INT NOT NULL,
    [GroupId]                         INT NOT NULL,
    CONSTRAINT [PK_dbo.CoursePercentagesGraphToGroups] PRIMARY KEY CLUSTERED ([CoursePercentagesGraphToGroupId] ASC),
    CONSTRAINT [FK_dbo.CoursePercentagesGraphToGroups_dbo.CoursePercentagesGraph_CoursePercentagesGraphId] FOREIGN KEY ([CoursePercentagesGraphId]) REFERENCES [dbo].[CoursePercentagesGraph] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.CoursePercentagesGraphToGroups_dbo.Groups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Groups] ([Id]) ON DELETE CASCADE
);

/*Результаты процентовок*/
CREATE TABLE [dbo].[CoursePercentagesResults] (
    [Id]                       INT           IDENTITY (1, 1) NOT NULL,
    [CoursePercentagesGraphId] INT           NOT NULL,
    [StudentId]                INT           NOT NULL,
    [Mark]                     INT           NULL,
    [Comments]                 NVARCHAR (50) NULL,
    CONSTRAINT [PK_dbo.CoursePercentagesResults] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.CoursePercentagesResults_dbo.CoursePercentagesGraph_CoursePercentagesGraphId] FOREIGN KEY ([CoursePercentagesGraphId]) REFERENCES [dbo].[CoursePercentagesGraph] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.CoursePercentagesResults_dbo.Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [dbo].[Students] ([UserId])
);

/*Шаблон Листа задания*/
CREATE TABLE [dbo].[CourseProjectTaskSheetTemplates] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [LecturerId]    INT            NOT NULL,
    [Name]          NVARCHAR (MAX) NULL,
    [InputData]     NVARCHAR (MAX) NULL,
    [RpzContent]    NVARCHAR (MAX) NULL,
    [DrawMaterials] NVARCHAR (MAX) NULL,
    [Consultants]   NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.CourseProjectTaskSheetTemplates] PRIMARY KEY CLUSTERED ([Id] ASC)
);






