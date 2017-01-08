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
	INSERT @t (TermLike) SELECT REPLACE('%'+Item+'%', '%%', '%') From [dbo].[FN_split_by](REPLACE(@term, N' ',N'%'),N'%')
	INSERT @e (TermLike) SELECT REPLACE('%'+Item+'%', '%%', '%') From [dbo].[FN_split_by](REPLACE(@emails, N' ',N'%'), N'%')
	INSERT @d (TermLike) SELECT REPLACE('%'+Item+'%', '%%', '%') From [dbo].[FN_split_by](REPLACE(@emailDomains, N' ',N'%'), N'%')

	SELECT TOP (100) 
		m.Match, 
		p.[Id],
		p.[Produkt],
		p.[IdKunde]
		FROM (
			SELECT Id, SUM(Match) AS Match FROM
			(
				SELECT DISTINCT [Id], 1 as Match
					FROM [dbo].[Produkt] p
					INNER JOIN @t as t
					ON p.Produkt LIKE t.TermLike 
			UNION All

				SELECT pd.Id, Match FROM
				(
					SELECT DISTINCT a.[IdKunde], 10 as Match
						FROM [dbo].[Ansprechpartner] a
						INNER JOIN @e as e
						ON a.email LIKE e.TermLike

				UNION All

					SELECT DISTINCT a.[IdKunde], 5 as Match
						FROM [dbo].[Ansprechpartner] a
						INNER JOIN @d as d
						ON a.email LIKE d.TermLike
				) as u
				INNER JOIN [dbo].[Produkt] pd
					ON u.IdKunde = pd.IdKunde
 
			) as u
			GROUP BY Id
		) as m
		INNER JOIN [dbo].[Produkt] p
		ON m.Id = p.IdKunde
		ORDER BY m.Match DESC, p.Produkt asc
		;
	
END;
