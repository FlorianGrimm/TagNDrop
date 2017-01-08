CREATE PROCEDURE [dbo].[SearchTermLieferant]
(
@term NVARCHAR(MAX),
@emails NVARCHAR(MAX),
@emailDomains NVARCHAR(MAX)
)
AS BEGIN
	SET NOCOUNT ON;
	DECLARE @t as TABLE (TermLike NVARCHAR(MAX));
	INSERT @t (TermLike) SELECT REPLACE('%'+Item+'%', '%%', '%') From [dbo].[FN_split_by](REPLACE(@term, N' ',N'%'),N'%')
	SELECT 
		m.Match, 
		l.Id, 
		l.Lieferant
		FROM (
			SELECT Id, SUM(Match) AS Match FROM
			(
				SELECT DISTINCT [Id], 1 as Match
					FROM [dbo].[Lieferant] l
					INNER JOIN @t as t
					ON l.Lieferant LIKE t.TermLike 
 
			) as u
			GROUP BY Id
		) as m
		INNER JOIN [dbo].[Lieferant] l
		ON m.Id = l.Id
		ORDER BY m.Match DESC, l.Lieferant asc
		;
	END;
