/*
    Adds a kanji to the user selection if it did not exist previously
    Otherwise restores the disabled entry

    -- Sample
    EXEC spKanjiUserAdd
        @userID = 1,
        @kanjiID = 1,
        @ticksModified = 123
*/

IF (OBJECT_ID('spKanjiUserAdd') IS NOT NULL)
	DROP PROCEDURE spKanjiUserAdd
GO

CREATE PROCEDURE spKanjiUserAdd
    @userID BIGINT,
    @kanjiID SMALLINT,
    @ticksModified BIGINT
AS

SET NOCOUNT ON

MERGE tblKanjiUserEntry AS tblDst
USING (SELECT @userID, @kanjiID, @ticksModified) AS tblSrc (userID, kanjiID, ticksModified)
ON (tblDst.userID = tblSrc.userID AND tblDst.kanjiID = tblSrc.kanjiID)
WHEN MATCHED THEN
    UPDATE SET
        tblDst.isDeleted = 0,
        tblDst.ticksModified = CASE WHEN tblDst.isDeleted = 1 THEN tblSrc.ticksModified ELSE tblDst.ticksModified END
WHEN NOT MATCHED THEN
    INSERT (userID, kanjiID, ticksModified)
    VALUES (tblSrc.userID, tblSrc.kanjiID, tblSrc.ticksModified)
OUTPUT 
    inserted.rating,
    inserted.notes,
    inserted.mark;

GO