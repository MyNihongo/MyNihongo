/*
	Query kanji by the JLPT level

	-- Sample
	EXEC spKanjiQueryByJlpt
		@jlptLevel = NULL,
		@filterID = NULL,
		@langID = 1,
		@userID = NULL,
		@pageIndex = 0,
		@pageSize = 35
*/

IF (OBJECT_ID('spKanjiQueryByJlpt') IS NOT NULL)
	DROP PROCEDURE spKanjiQueryByJlpt
GO

CREATE PROCEDURE spKanjiQueryByJlpt
	@jlptLevel TINYINT,
	@filterID TINYINT,
	@langID TINYINT,
	@userID BIGINT,
	@pageIndex INT,
	@pageSize INT
AS

SET NOCOUNT ON
DECLARE @skipCount AS INT = @pageIndex * @pageSize

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
	tblKanjiMasterData
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
