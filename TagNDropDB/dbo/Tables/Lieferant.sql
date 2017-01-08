CREATE TABLE [dbo].[Lieferant] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Lieferant] NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_Lieferant] PRIMARY KEY CLUSTERED ([Id] ASC)
);

