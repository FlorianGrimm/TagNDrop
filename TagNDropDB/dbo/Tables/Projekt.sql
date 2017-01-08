CREATE TABLE [dbo].[Projekt] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [IdProdukt] INT            NULL,
    [Projekt]   NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Projekt_Produkt] FOREIGN KEY ([IdProdukt]) REFERENCES [dbo].[Produkt] ([Id])
);

