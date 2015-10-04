IF OBJECT_ID(N'dbo.Concept', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[Concept]
END
GO
CREATE TABLE [dbo].[Concept](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](250) NOT NULL,
	[Container] [varchar](max) NULL,
	[ParentId] [int] NULL,
	[IsGroup] [bit] NOT NULL,
	[NextConcept] [int] NULL,
	[PrevConcept] [int] NULL,
	[UserId] [int] NOT NULL,
	[SubjectId] [int] NOT NULL,
	[Published] [bit] NOT NULL,
	[ReadOnly] [bit] NOT NULL,
 CONSTRAINT [PK_Concept] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Concept]  WITH CHECK ADD  CONSTRAINT [FK_Concept_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Concept]  WITH CHECK ADD  CONSTRAINT [FK_Concept_Subject] FOREIGN KEY([SubjectId])
REFERENCES [dbo].[Subjects] ([Id])
GO

insert into [dbo].[Modules]
values('CM', 'нгЬЪ', 1,14,8)
GO

insert into [dbo].[SubjectModules]
values(1,14,0)

