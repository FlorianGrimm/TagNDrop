CREATE TABLE [dbo].[Kunde] (
    [Id]    INT            IDENTITY (1, 1) NOT NULL,
    [Kunde] NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_Kunde] PRIMARY KEY CLUSTERED ([Id] ASC)
);

