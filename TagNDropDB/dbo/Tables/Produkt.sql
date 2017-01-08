CREATE TABLE [dbo].[Produkt] (
    [Id]      INT            IDENTITY (1, 1) NOT NULL,
    [IdKunde] INT            NULL,
    [Produkt] NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_Produkt] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Produkt_Kunde] FOREIGN KEY ([IdKunde]) REFERENCES [dbo].[Kunde] ([Id])
);

