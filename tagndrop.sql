USE [master]
GO
/****** Object:  Database [TagNDropDB]    Script Date: 1/9/2017 12:57:31 AM ******/
CREATE DATABASE [TagNDropDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TagNDropDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\TagNDropDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'TagNDropDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\TagNDropDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [TagNDropDB] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TagNDropDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [TagNDropDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [TagNDropDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [TagNDropDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [TagNDropDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [TagNDropDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [TagNDropDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [TagNDropDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [TagNDropDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [TagNDropDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [TagNDropDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [TagNDropDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [TagNDropDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [TagNDropDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [TagNDropDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [TagNDropDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [TagNDropDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [TagNDropDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [TagNDropDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [TagNDropDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [TagNDropDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [TagNDropDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [TagNDropDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [TagNDropDB] SET RECOVERY FULL 
GO
ALTER DATABASE [TagNDropDB] SET  MULTI_USER 
GO
ALTER DATABASE [TagNDropDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [TagNDropDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [TagNDropDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [TagNDropDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [TagNDropDB] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'TagNDropDB', N'ON'
GO
ALTER DATABASE [TagNDropDB] SET QUERY_STORE = OFF
GO
USE [TagNDropDB]
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [TagNDropDB]
GO
/****** Object:  User [DEV\DevSp16Farm]    Script Date: 1/9/2017 12:57:31 AM ******/
CREATE USER [DEV\DevSp16Farm] FOR LOGIN [DEV\DevSp16Farm] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [DEV\DevSp16Admin]    Script Date: 1/9/2017 12:57:31 AM ******/
CREATE USER [DEV\DevSp16Admin] FOR LOGIN [DEV\DevSp16Admin] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[Ansprechpartner]    Script Date: 1/9/2017 12:57:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ansprechpartner](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdKunde] [int] NOT NULL,
	[email] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Ansprechpartner] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[viewAnsprechpartner]    Script Date: 1/9/2017 12:57:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[viewAnsprechpartner]
AS
SELECT        Id, IdKunde, email
FROM            dbo.Ansprechpartner

GO
/****** Object:  Table [dbo].[Kunde]    Script Date: 1/9/2017 12:57:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Kunde](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Kunde] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Kunde] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[viewKunde]    Script Date: 1/9/2017 12:57:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[viewKunde]
AS
SELECT        
Kunde_Id = k.Id, 
Kunde_Kunde = k.Kunde
FROM dbo.Kunde k



GO
/****** Object:  Table [dbo].[Lieferant]    Script Date: 1/9/2017 12:57:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lieferant](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Lieferant] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Lieferant] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[viewLieferant]    Script Date: 1/9/2017 12:57:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[viewLieferant]
AS
SELECT        
Lieferant_Id = l.Id, 
Lieferant_Lieferant = l.Lieferant
FROM dbo.Lieferant l

GO
/****** Object:  Table [dbo].[Produkt]    Script Date: 1/9/2017 12:57:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Produkt](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdKunde] [int] NULL,
	[Produkt] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Produkt] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[viewProdukt]    Script Date: 1/9/2017 12:57:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[viewProdukt]
AS
SELECT        
Produkt_Id = pd.Id, 
Produkt_IdKunde = pd.IdKunde, 
Produkt_Produkt = pd.Produkt
FROM dbo.Produkt pd

GO
/****** Object:  Table [dbo].[Projekt]    Script Date: 1/9/2017 12:57:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Projekt](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdProdukt] [int] NULL,
	[Projekt] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[viewProjekt]    Script Date: 1/9/2017 12:57:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[viewProjekt]
AS
SELECT        
Projekt_Id = pj.Id,
Projekt_IdProdukt = pj.IdProdukt,
Projekt_Projekt = pj.Projekt
FROM dbo.Projekt pj



GO
/****** Object:  View [dbo].[viewKundeProdukt]    Script Date: 1/9/2017 12:57:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[viewKundeProdukt]
AS
SELECT        
pd.Produkt_Id, 
pd.Produkt_IdKunde, 
pd.Produkt_Produkt, 
k.Kunde_Id,
k.Kunde_Kunde
FROM dbo.viewKunde k
INNER JOIN dbo.viewProdukt pd
	ON k.Kunde_Id = pd.Produkt_IdKunde



GO
/****** Object:  View [dbo].[viewKundeProduktProjekt]    Script Date: 1/9/2017 12:57:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[viewKundeProduktProjekt]
AS
SELECT        
pj.Projekt_Id, 
pj.Projekt_IdProdukt, 
pj.Projekt_Projekt, 
pd.Produkt_Id,
pd.Produkt_IdKunde, 
pd.Produkt_Produkt, 
k.Kunde_Id,
k.Kunde_Kunde
FROM dbo.viewKunde AS k 
INNER JOIN dbo.viewProdukt AS pd 
	ON k.Kunde_Id = pd.Produkt_IdKunde
INNER JOIN dbo.viewProjekt AS pj 
	ON pd.Produkt_Id = pj.Projekt_IdProdukt



GO
/****** Object:  UserDefinedFunction [dbo].[FN_split_by]    Script Date: 1/9/2017 12:57:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[FN_split_by]
(
   @List NVARCHAR(MAX),
   @Delimiter NVARCHAR(1)
)
RETURNS TABLE
WITH SCHEMABINDING AS
RETURN
     -- SOLVIN
     -- 2014-10-13 Florian
     -- 2014-11-05 Florian: bugfix last part
  WITH E1(N)        AS ( SELECT 1 UNION ALL SELECT 1 UNION ALL SELECT 1 UNION ALL SELECT 1 UNION ALL SELECT 1 UNION ALL SELECT 1 UNION ALL SELECT 1 UNION ALL SELECT 1 UNION ALL SELECT 1 UNION ALL SELECT 1),
       E2(N)        AS (SELECT 1 FROM E1 a, E1 b),
       E4(N)        AS (SELECT 1 FROM E2 a, E2 b),
       E8(N)        AS (SELECT 1 FROM E4 a, E2 b),
       cteTally(N)  AS (SELECT TOP (DATALENGTH(ISNULL(@List,N''))/2) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) FROM E8),
       cteStart(N1) AS (SELECT 0 UNION ALL SELECT t.N+1 FROM cteTally t WHERE (SUBSTRING(@List,t.N,1) = @Delimiter))
  SELECT Item = SUBSTRING(@List, s.N1, ISNULL(NULLIF(CHARINDEX(@Delimiter,@List,s.N1),0)-s.N1,DATALENGTH(ISNULL(@List,N''))/2+1))
    FROM cteStart s;

GO
ALTER TABLE [dbo].[Ansprechpartner]  WITH CHECK ADD  CONSTRAINT [FK_Ansprechpartner_Kunde] FOREIGN KEY([IdKunde])
REFERENCES [dbo].[Kunde] ([Id])
GO
ALTER TABLE [dbo].[Ansprechpartner] CHECK CONSTRAINT [FK_Ansprechpartner_Kunde]
GO
ALTER TABLE [dbo].[Produkt]  WITH CHECK ADD  CONSTRAINT [FK_Produkt_Kunde] FOREIGN KEY([IdKunde])
REFERENCES [dbo].[Kunde] ([Id])
GO
ALTER TABLE [dbo].[Produkt] CHECK CONSTRAINT [FK_Produkt_Kunde]
GO
ALTER TABLE [dbo].[Projekt]  WITH CHECK ADD  CONSTRAINT [FK_Projekt_Produkt] FOREIGN KEY([IdProdukt])
REFERENCES [dbo].[Produkt] ([Id])
GO
ALTER TABLE [dbo].[Projekt] CHECK CONSTRAINT [FK_Projekt_Produkt]
GO
/****** Object:  StoredProcedure [dbo].[QueryKunde]    Script Date: 1/9/2017 12:57:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[QueryKunde]
(
@ids NVARCHAR(MAX),
@returnParent bit,
@returnThis bit,
@returnChildren bit
)
AS BEGIN
	SET NOCOUNT ON;
	DECLARE @i as TABLE (Id int);
	INSERT @i (Id) SELECT DISTINCT CAST(REPLACE(REPLACE(Item, N'\;', N','), N'\!', N'\') AS int) From [dbo].[FN_split_by](@ids, N',');
	-- IF (@returnParent=1) BEGIN END;
	IF (@returnThis=1) BEGIN
		SELECT 
			N'Kunde' as MetaEntityName, 
			k.Kunde_Id, 
			k.Kunde_Kunde
		FROM @i i
		INNER JOIN [dbo].[viewKunde] k
			ON i.Id = k.Kunde_Id
		ORDER BY k.Kunde_Kunde asc
		;
	END;
	IF (@returnChildren=1) BEGIN 
		SELECT 
			N'Produkt' as MetaEntityName, 
			pd.Produkt_Id, 
			pd.Produkt_Produkt,
			pd.Produkt_IdKunde
		FROM @i i
		INNER JOIN [dbo].[viewProdukt] pd
			ON i.Id = pd.Produkt_IdKunde
		ORDER BY pd.Produkt_Produkt asc
		;
	END;
END;
GO
/****** Object:  StoredProcedure [dbo].[QueryLieferant]    Script Date: 1/9/2017 12:57:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[QueryLieferant]
(
@ids NVARCHAR(MAX),
@returnParent bit,
@returnThis bit,
@returnChildren bit
)
AS BEGIN
	SET NOCOUNT ON;
	DECLARE @i as TABLE (Id int);
	INSERT @i (Id) SELECT DISTINCT CAST(REPLACE(REPLACE(Item, N'\;', N','), N'\!', N'\') AS int) From [dbo].[FN_split_by](@ids, N',');
	--IF (@returnParent=1) BEGIN END;
	IF (@returnThis=1) BEGIN
		SELECT 
			N'Lieferant' as MetaEntityName, 
			l.Lieferant_Id, 
			l.Lieferant_Lieferant
			FROM @i i
			INNER JOIN [dbo].[viewLieferant] l
			ON i.Id = l.Lieferant_Id
			ORDER BY l.Lieferant_Lieferant asc
			;
	END;
	-- IF (@returnChildren=1) BEGIN END;
END;
GO
/****** Object:  StoredProcedure [dbo].[QueryProdukt]    Script Date: 1/9/2017 12:57:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[QueryProdukt]
(
@ids NVARCHAR(MAX),
@returnParent bit,
@returnThis bit,
@returnChildren bit
)
AS BEGIN
	SET NOCOUNT ON;
	DECLARE @i as TABLE (Id int);
	INSERT @i (Id) SELECT DISTINCT CAST(REPLACE(REPLACE(Item, N'\;', N','), N'\!', N'\') AS int) From [dbo].[FN_split_by](@ids, N',');
	IF (@returnParent=1) BEGIN
		SELECT 
			N'Kunde' as MetaEntityName, 
			k.Kunde_Id, 
			k.Kunde_Kunde
		FROM (
			SELECT DISTINCT
				pd.Produkt_IdKunde
			FROM @i i
				INNER JOIN [dbo].[viewProdukt] pd
					ON i.Id = pd.Produkt_Id
			) as c
			INNER JOIN [dbo].[viewKunde] k
				ON c.Produkt_IdKunde = k.Kunde_Id
		ORDER BY k.Kunde_Kunde asc
		;
	END;
	IF (@returnThis=1) BEGIN
		SELECT 
			N'Produkt' as MetaEntityName, 
			pd.Produkt_Id, 
			pd.Produkt_Produkt,
			pd.Produkt_IdKunde
			FROM @i i
			INNER JOIN [dbo].[viewProdukt] pd
			ON i.Id = pd.Produkt_Id
			ORDER BY pd.Produkt_Produkt asc
			;
	END;
	IF (@returnChildren=1) BEGIN 
		SELECT 
			N'Projekt' as MetaEntityName, 
			pj.Projekt_Id, 
			pj.Projekt_Projekt,
			pj.Projekt_IdProdukt
		FROM @i i
			INNER JOIN [dbo].[viewProjekt] pj
			ON i.Id = pj.Projekt_IdProdukt
		ORDER BY pj.Projekt_Projekt asc
			;
	END;
END;
GO
/****** Object:  StoredProcedure [dbo].[QueryProjekt]    Script Date: 1/9/2017 12:57:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[QueryProjekt]
(
@ids NVARCHAR(MAX),
@returnParent bit,
@returnThis bit,
@returnChildren bit
)
AS BEGIN
	SET NOCOUNT ON;
	DECLARE @i as TABLE (Id int);
	INSERT @i (Id) SELECT DISTINCT CAST(REPLACE(REPLACE(Item, N'\;', N','), N'\!', N'\') AS int) From [dbo].[FN_split_by](@ids, N',');
	IF (@returnParent=1) BEGIN
		SELECT 
			N'Produkt' as MetaEntityName, 
			pd.Produkt_Id, 
			pd.Produkt_Produkt,
			pd.Produkt_IdKunde
		FROM (
			SELECT DISTINCT
				pj.Projekt_IdProdukt
			FROM @i i
				INNER JOIN [dbo].[viewProjekt] pj
					ON i.Id = pj.Projekt_Id
			) as c
			INNER JOIN [dbo].[viewProdukt] pd
				ON c.Projekt_IdProdukt = pd.Produkt_Id
		ORDER BY pd.Produkt_Produkt asc
			;
	END;
	IF (@returnThis=1) BEGIN
		SELECT 
			N'Projekt' as MetaEntityName, 
			pj.Projekt_Id, 
			pj.Projekt_Projekt,
			pj.Projekt_IdProdukt
		FROM @i i
			INNER JOIN [dbo].[viewProjekt] pj
			ON i.Id = pj.Projekt_Id
		ORDER BY pj.Projekt_Projekt asc
			;
	END;
	-- no children
	-- IF (@returnChildren=1) BEGIN END;
END;
GO
/****** Object:  StoredProcedure [dbo].[SearchTermKunde]    Script Date: 1/9/2017 12:57:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SearchTermKunde]
(
@term NVARCHAR(MAX),
@emails NVARCHAR(MAX),
@emailDomains NVARCHAR(MAX)
)
AS BEGIN
	SET NOCOUNT ON;
	DECLARE @t as TABLE (TermLike NVARCHAR(MAX));
	DECLARE @e as TABLE (TermLike NVARCHAR(MAX));
	DECLARE @d as TABLE (TermLike NVARCHAR(MAX));
	INSERT @t (TermLike) SELECT Item From [dbo].[FN_split_by](@term, N' ');
	INSERT @e (TermLike) SELECT Item From [dbo].[FN_split_by](@emails, N' ');
	INSERT @d (TermLike) SELECT Item From [dbo].[FN_split_by](@emailDomains, N' ');
	SELECT 
		N'Kunde' as MetaEntityName,
		m.Match, 
		k.Kunde_Id, 
		k.Kunde_Kunde
		FROM (
			SELECT Kunde_Id, SUM(Match) AS Match FROM
			--SELECT Id, Match FROM
			(
				SELECT DISTINCT k.Kunde_Id, 1 as Match
					FROM [dbo].[viewKunde] as k
					INNER JOIN @t as t
					ON k.Kunde_Kunde LIKE t.TermLike 

			UNION All

				SELECT DISTINCT a.[IdKunde], 10 as Match
					FROM [dbo].[viewAnsprechpartner] a
					INNER JOIN @e as e
					ON a.email LIKE e.TermLike

			UNION All

				SELECT DISTINCT a.[IdKunde], 5 as Match
					FROM [dbo].[viewAnsprechpartner] a
					INNER JOIN @d as d
					ON a.email LIKE d.TermLike
 
			) as u
			GROUP BY Kunde_Id
		) as m
		INNER JOIN [dbo].[viewKunde] k
		ON m.Kunde_Id = k.Kunde_Id
		ORDER BY m.Match DESC, k.Kunde_Kunde asc
		;
	END;

GO
/****** Object:  StoredProcedure [dbo].[SearchTermLieferant]    Script Date: 1/9/2017 12:57:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SearchTermLieferant]
(
@term NVARCHAR(MAX),
@emails NVARCHAR(MAX),
@emailDomains NVARCHAR(MAX)
)
AS BEGIN
	SET NOCOUNT ON;
	DECLARE @t as TABLE (TermLike NVARCHAR(MAX));
	INSERT @t (TermLike) SELECT Item From [dbo].[FN_split_by](@term, N' ');
	SELECT 
	   'Lieferant' as MetaEntityName,
		m.Match, 
		l.Lieferant_Id, 
		l.Lieferant_Lieferant
		FROM (
			SELECT Lieferant_Id, SUM(Match) AS Match FROM
			(
				SELECT DISTINCT Lieferant_Id, 1 as Match
					FROM [dbo].[viewLieferant] l
					INNER JOIN @t as t
					ON l.Lieferant_Lieferant LIKE t.TermLike 
 
			) as u
			GROUP BY Lieferant_Id
		) as m
		INNER JOIN [dbo].[viewLieferant] l
		ON m.Lieferant_Id = l.Lieferant_Id
		ORDER BY m.Match DESC, l.Lieferant_Lieferant asc
		;
	END;

GO
/****** Object:  StoredProcedure [dbo].[SearchTermProdukt]    Script Date: 1/9/2017 12:57:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SearchTermProdukt]
(
	@term NVARCHAR(4000),
	@emails NVARCHAR(MAX),
	@emailDomains NVARCHAR(MAX)
)
AS BEGIN
	SET NOCOUNT ON;
	DECLARE @t as TABLE (TermLike NVARCHAR(MAX));
	DECLARE @e as TABLE (TermLike NVARCHAR(MAX));
	DECLARE @d as TABLE (TermLike NVARCHAR(MAX));
	INSERT @t (TermLike) SELECT Item From [dbo].[FN_split_by](@term, N' ');
	INSERT @e (TermLike) SELECT Item From [dbo].[FN_split_by](@emails, N' ');
	INSERT @d (TermLike) SELECT Item From [dbo].[FN_split_by](@emailDomains, N' ');

	SELECT TOP (100) 
	    N'Produkt' as MetaEntityName,
		m.Match, 
		pd.[Produkt_Id],
		pd.[Produkt_Produkt],
		pd.[Produkt_IdKunde]
		FROM (
			SELECT Produkt_Id, SUM(Match) AS Match FROM
			(
				SELECT DISTINCT [Produkt_Id], 1 as Match
					FROM [dbo].[viewProdukt] p
					INNER JOIN @t as t
					ON p.Produkt_Produkt LIKE t.TermLike 
			UNION All

				SELECT pd.Produkt_Id, Match FROM
				(
					SELECT DISTINCT a.[IdKunde], 10 as Match
						FROM [dbo].[viewAnsprechpartner] a
						INNER JOIN @e as e
						ON a.email LIKE e.TermLike

				UNION All

					SELECT DISTINCT a.[IdKunde], 5 as Match
						FROM [dbo].[viewAnsprechpartner] a
						INNER JOIN @d as d
						ON a.email LIKE d.TermLike
				) as u
				INNER JOIN [dbo].[viewProdukt] pd
					ON u.IdKunde = pd.Produkt_IdKunde
 
			) as u
			GROUP BY Produkt_Id
		) as m
		INNER JOIN [dbo].[viewProdukt] pd
		ON m.Produkt_Id = pd.Produkt_IdKunde
		ORDER BY m.Match DESC, pd.Produkt_Produkt asc
		;
	
END;

GO
/****** Object:  StoredProcedure [dbo].[SearchTermProjekt]    Script Date: 1/9/2017 12:57:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SearchTermProjekt]
(
	@term NVARCHAR(4000),
	@emails NVARCHAR(MAX),
	@emailDomains NVARCHAR(MAX)
)
AS BEGIN
	SET NOCOUNT ON;
	DECLARE @t as TABLE (TermLike NVARCHAR(MAX));
	DECLARE @e as TABLE (TermLike NVARCHAR(MAX));
	DECLARE @d as TABLE (TermLike NVARCHAR(MAX));
	INSERT @t (TermLike) SELECT Item From [dbo].[FN_split_by](@term, N' ');
	INSERT @e (TermLike) SELECT Item From [dbo].[FN_split_by](@emails, N' ');
	INSERT @d (TermLike) SELECT Item From [dbo].[FN_split_by](@emailDomains, N' ');

	SELECT TOP (100) 
	    N'Projekt' as MetaEntityName,
		m.Match, 
		pr.Projekt_Id,
		pr.Projekt_Projekt,
		pr.Projekt_IdProdukt
		FROM (
			SELECT Projekt_Id, SUM(Match) AS Match FROM
			(
				SELECT DISTINCT Projekt_Id, 1 as Match
					FROM [dbo].[viewProjekt] p
					INNER JOIN @t as t
						ON p.Projekt_Projekt LIKE t.TermLike 
			UNION All

				SELECT pj.Projekt_Id, Match FROM
				(
					SELECT DISTINCT a.[IdKunde], 10 as Match
						FROM [dbo].[viewAnsprechpartner] a
						INNER JOIN @e as e
							ON a.email LIKE e.TermLike

				UNION All

					SELECT DISTINCT a.[IdKunde], 5 as Match
						FROM [dbo].[viewAnsprechpartner] a
						INNER JOIN @d as d
							ON a.email LIKE d.TermLike
				) as u
				INNER JOIN [dbo].[viewProdukt] pd
					ON u.IdKunde = pd.Produkt_IdKunde
				INNER JOIN [dbo].[viewProjekt] pj
					ON pd.Produkt_Id = pj.Projekt_IdProdukt
			) as u
			GROUP BY Projekt_Id
		) as m
		INNER JOIN [dbo].[viewProjekt] pr
		ON m.Projekt_Id = pr.Projekt_Id
		ORDER BY m.Match DESC, pr.Projekt_Projekt asc
		;

END;
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Ansprechpartner"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 119
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewAnsprechpartner'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewAnsprechpartner'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Kunde"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 102
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewKunde'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewKunde'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Kunde"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 102
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Produkt"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 119
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewKundeProdukt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewKundeProdukt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "k"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 102
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "pd"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 119
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "pj"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 119
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewKundeProduktProjekt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewKundeProduktProjekt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Lieferant"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 102
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewLieferant'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewLieferant'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Produkt"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 119
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewProdukt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewProdukt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Projekt"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 119
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewProjekt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewProjekt'
GO
USE [master]
GO
ALTER DATABASE [TagNDropDB] SET  READ_WRITE 
GO
