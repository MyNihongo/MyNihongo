/*
    Query detail data for a single kanji

    -- Sample
    EXEC spKanjiQueryDetail
        @langID = 1,
        @userID = NULL,
        @kanjiID = 1
*/

IF (OBJECT_ID('spKanjiQueryDetail') IS NOT NULL)
	DROP PROCEDURE spKanjiQueryDetail
GO

CREATE PROCEDURE spKanjiQueryDetail
    @langID TINYINT,
    @userID BIGINT,
    @kanjiID SMALLINT
AS

SET NOCOUNT ON

EXEC spDropTempTable '#tmpKanji'
SELECT TOP(1)
    tblKanjiMasterData.kanjiID,
    tblKanjiMasterData.[char],
    tblKanjiMasterData.jlptLevel,
    tblKanjiUserEntry.rating,
    tblKanjiUserEntry.notes,
    tblKanjiUserEntry.mark
INTO
    #tmpKanji
FROM
    tblKanjiMasterData
    LEFT JOIN tblKanjiUserEntry
    ON tblKanjiUserEntry.userID = @userID
        AND tblKanjiUserEntry.kanjiID = @kanjiID
        AND tblKanjiUserEntry.isDeleted = 0
WHERE
    tblKanjiMasterData.kanjiID = @kanjiID

-------------------------------------------------------------------------------
-- Output
-------------------------------------------------------------------------------

EXEC spKanjiQuery
	@langID = @langID

GO