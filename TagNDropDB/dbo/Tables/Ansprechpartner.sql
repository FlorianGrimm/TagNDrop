CREATE TABLE [dbo].[Ansprechpartner] (
    [Id]      INT            IDENTITY (1, 1) NOT NULL,
    [IdKunde] INT            NOT NULL,
    [email]   NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_Ansprechpartner] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Ansprechpartner_Kunde] FOREIGN KEY ([IdKunde]) REFERENCES [dbo].[Kunde] ([Id])
);

