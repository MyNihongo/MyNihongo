/*
	Query kanji by characters

	-- Sample
	EXEC spDropTempTable #tmpChar

	CREATE TABLE #tmpChar
    (
        [char] NCHAR(1) COLLATE Japanese_Bushu_Kakusu_100_BIN PRIMARY KEY
    )

	INSERT #tmpChar ([char]) VALUES (N'日')
	INSERT #tmpChar ([char]) VALUES (N'本')
	INSERT #tmpChar ([char]) VALUES (N'人')

	EXEC spKanjiQueryByChar
		@jlptLevel = NULL,
		@filterID = NULL,
		@langID = 1,
		@userID = NULL,
		@pageIndex = 0,
		@pageSize = 35
*/

IF (OBJECT_ID('spKanjiQueryByChar') IS NOT NULL)
	DROP PROCEDURE spKanjiQueryByChar
GO

CREATE PROCEDURE spKanjiQueryByChar
	@jlptLevel TINYINT,
	@filterID TINYINT,
	@langID TINYINT,
	@userID BIGINT,
	@pageIndex INT,
	@pageSize INT
AS

SET NOCOUNT ON
DECLARE @skipCount AS INT = @pageIndex * @pageSize

IF OBJECT_ID('tempdb..#tmpChar') IS NULL
BEGIN
	CREATE TABLE #tmpChar
	(
		[char] NCHAR(1) COLLATE Japanese_Bushu_Kakusu_100_BIN PRIMARY KEY
	)
END

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
	#tmpChar
	INNER JOIN tblKanjiMasterData
	ON tblKanjiMasterData.[char] = #tmpChar.[char]
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