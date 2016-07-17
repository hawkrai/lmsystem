CREATE TABLE [dbo].[ConceptQuestions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ConceptId] [int] NOT NULL,
	[QuestionId] [int] NOT NULL
 CONSTRAINT [PK_dbo.ConceptQuestions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ConceptQuestions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ConceptQuestions_dbo.Concept_ConceptId] FOREIGN KEY([ConceptId])
REFERENCES [dbo].[Concept] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ConceptQuestions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ConceptQuestions_dbo.Questions_QuestionId] FOREIGN KEY([QuestionId])
REFERENCES [dbo].[Questions] ([Id])
GO
