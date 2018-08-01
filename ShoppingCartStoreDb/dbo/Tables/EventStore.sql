CREATE TABLE [dbo].[EventStore] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (100) NOT NULL,
    [OccuredAt] DATETIME2 (7)  NOT NULL,
    [Content]   NVARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

