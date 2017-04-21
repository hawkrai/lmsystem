CREATE TABLE [dbo].[WatchingTime] (
    [Id]        INT IDENTITY (1, 1) NOT NULL,
    [UserId]    INT NULL,
    [ConceptId] INT NULL,
    [Time]      INT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

