/*
    Removes a kanji from the user selection

    -- Sample
    EXEC spKanjiUserRemove
        @userID = 1,
        @kanjiID = 1,
        @ticksModified = 123
*/

IF (OBJECT_ID('spKanjiUserRemove') IS NOT NULL)
	DROP PROCEDURE spKanjiUserRemove
GO

CREATE PROCEDURE spKanjiUserRemove
    @userID BIGINT,
    @kanjiID SMALLINT,
    @ticksModified BIGINT
AS

SET NOCOUNT ON

UPDATE
    tblKanjiUserEntry
SET
    tblKanjiUserEntry.isDeleted = 1,
    tblKanjiUserEntry.ticksModified = @ticksModified
WHERE
    tblKanjiUserEntry.userID = @userID
    AND
    tblKanjiUserEntry.kanjiID = @kanjiID
    AND
    tblKanjiUserEntry.isDeleted = 0

GO