/*
	Query kanji by random text

	-- Sample
	EXEC spKanjiQueryByText
		@text = N'awa',
		@byRomaji = 1,
		@byLanguage = 0,
		@jlptLevel = NULL,
		@filterID = NULL,
		@langID = 1,
		@userID = NULL,
		@pageIndex = 0,
		@pageSize = 35
*/

IF (OBJECT_ID('spKanjiQueryByText') IS NOT NULL)
	DROP PROCEDURE spKanjiQueryByText
GO

CREATE PROCEDURE spKanjiQueryByText
	@text NVARCHAR(255),
	@byRomaji bit,
	@byLanguage bit,
	@jlptLevel TINYINT,
	@filterID TINYINT,
	@langID TINYINT,
	@userID BIGINT,
	@pageIndex INT,
	@pageSize INT
AS

SET NOCOUNT ON
DECLARE @skipCount AS INT = @pageIndex * @pageSize
DECLARE @searchPattern AS NVARCHAR(255) = @text + '%'

EXEC spDropTempTable '#tmpKanjiID'
CREATE TABLE #tmpKanjiID
(
	kanjiID SMALLINT NOT NULL
)

-------------------------------------------------------------------------------
-- Find kanji ID which match the specified text
-------------------------------------------------------------------------------

-- Input 1 (Romaji)
IF @byRomaji = 1
BEGIN
	INSERT #tmpKanjiID
		(kanjiID)
	SELECT
		tblKanjiReading.kanjiID
	FROM
		tblKanjiReading
	WHERE
		tblKanjiReading.romaji LIKE @searchPattern
END

-- Input 2 (Language)
IF @byLanguage = 1
BEGIN
	INSERT #tmpKanjiID
		(kanjiID)
	SELECT
		tblKanjiMeaning.kanjiID
	FROM
		tblKanjiMeaning
	WHERE
		tblKanjiMeaning.langID = @langID
		AND
		tblKanjiMeaning.text LIKE @searchPattern
END

-- Delete duplicates
;
WITH
	cte
	AS
	(
		SELECT
			#tmpKanjiID.kanjiID,
			ROW_NUMBER() OVER (PARTITION BY #tmpKanjiID.kanjiID ORDER BY #tmpKanjiID.kanjiID) AS rowNumber
		FROM #tmpKanjiID
	)
DELETE cte WHERE rowNumber > 1

-------------------------------------------------------------------------------
-- Prepare Kanji
-------------------------------------------------------------------------------

EXEC spDropTempTable '#tmpKanji'
SELECT
	tblKanjiMasterData.kanjiID,
	tblKanjiMasterData.sorting,
	tblKanjiMasterData.[char],
	tblKanjiMasterData.jlptLevel,
	tblKanjiUserEntry.rating
INTO
	#tmpKanji
FROM
	#tmpKanjiID
	INNER JOIN tblKanjiMasterData
	ON tblKanjiMasterData.kanjiID = #tmpKanjiID.kanjiID
	LEFT JOIN tblKanjiUserEntry
	ON tblKanjiUserEntry.userID = @userID
		AND tblKanjiUserEntry.kanjiID = tblKanjiMasterData.kanjiID
		AND tblKanjiUserEntry.isDeleted = 0
WHERE
	(
		1 = CASE WHEN @jlptLevel IS NOT NULL
			THEN CASE WHEN tblKanjiMasterData.jlptLevel = @jlptLevel THEN 1 ELSE 0 END
			ELSE 1
		END
	)
	AND
	(
		1 = CASE @filterID
			WHEN 1 THEN CASE WHEN tblKanjiUserEntry.rating IS NOT NULL THEN 1 ELSE 0 END -- 1: user kanji
			WHEN 2 THEN CASE WHEN tblKanjiUserEntry.rating <> 0 THEN 1 ELSE 0 END -- 2: favourite kanji
			ELSE 1
		END
	)
ORDER BY
	tblKanjiMasterData.jlptLevel DESC,
	tblKanjiMasterData.sorting ASC
OFFSET @skipCount ROWS
FETCH NEXT @pageSize ROWS ONLY

-------------------------------------------------------------------------------
-- Output
-------------------------------------------------------------------------------

EXEC spKanjiQuery
	@langID = @langID

GO