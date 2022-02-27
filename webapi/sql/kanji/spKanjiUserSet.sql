/*
    Set values for a kanji entry of the user

    -- Sample
    EXEC spKanjiUserSet
        @userID = 1,
        @kanjiID = 1,
        @rating = 35,
        @notes = NULL,
        @mark = 12,
        @ticksModified = 123
*/

IF (OBJECT_ID('spKanjiUserSet') IS NOT NULL)
	DROP PROCEDURE spKanjiUserSet
GO

CREATE PROCEDURE spKanjiUserSet
    @userID BIGINT,
    @kanjiID SMALLINT,
    @rating TINYINT,
    @notes NVARCHAR(MAX),
    @mark TINYINT,
    @ticksModified BIGINT
AS

SET NOCOUNT ON

UPDATE
    tblKanjiUserEntry
SET
    tblKanjiUserEntry.rating = ISNULL(@rating, tblKanjiUserEntry.rating),
    tblKanjiUserEntry.notes = ISNULL(@notes, tblKanjiUserEntry.notes),
    tblKanjiUserEntry.mark = ISNULL(@mark, tblKanjiUserEntry.mark),
    tblKanjiUserEntry.ticksModified = @ticksModified
WHERE
    tblKanjiUserEntry.userID = @userID
    AND
    tblKanjiUserEntry.kanjiID = @kanjiID
    AND
    tblKanjiUserEntry.isDeleted = 0

GO