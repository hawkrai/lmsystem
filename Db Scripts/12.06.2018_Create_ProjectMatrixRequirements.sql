USE [LMPlatform2]
GO

/****** Object:  Table [dbo].[ProjectMatrixRequirements]    Script Date: 12.06.2018 9:14:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ProjectMatrixRequirements](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Number] [nvarchar](50) NOT NULL,
	[Covered] [bit] NOT NULL,
	[ProjectId] [int] NULL,
 CONSTRAINT [PK_ProjectMatrixRequirements] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[ProjectMatrixRequirements] ADD  CONSTRAINT [DF_ProjectMatrixRequirements_Covered]  DEFAULT ('FALSE') FOR [Covered]
GO

ALTER TABLE [dbo].[ProjectMatrixRequirements]  WITH CHECK ADD  CONSTRAINT [FK_ProjectMatrixRequirements_Projects] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Projects] ([Id])
GO

ALTER TABLE [dbo].[ProjectMatrixRequirements] CHECK CONSTRAINT [FK_ProjectMatrixRequirements_Projects]
GO
