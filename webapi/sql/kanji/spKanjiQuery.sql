/*
    Internal procedure to output kanji query
*/

IF (OBJECT_ID('spKanjiQuery') IS NOT NULL)
	DROP PROCEDURE spKanjiQuery
GO

CREATE PROCEDURE spKanjiQuery
    @langID TINYINT
AS

SET NOCOUNT ON

IF OBJECT_ID('tempdb..#tmpKanji') IS NULL
BEGIN
    CREATE TABLE #tmpKanji
    (
        kanjiID SMALLINT PRIMARY KEY
    )
END

-- Output 1 (Readings)
SELECT
    #tmpKanji.kanjiID,
    tblKanjiReading.[type],
    tblKanjiReading.main,
    tblKanjiReading.[secondary],
    tblKanjiReading.romaji
FROM
    #tmpKanji
    INNER JOIN tblKanjiReading
    ON tblKanjiReading.kanjiID = #tmpKanji.kanjiID
ORDER by
    #tmpKanji.kanjiID ASC,
    tblKanjiReading.sorting ASC

-- Output 2 (Meanings)
SELECT
    #tmpKanji.kanjiID,
    tblKanjiMeaning.text
FROM
    #tmpKanji
    INNER JOIN tblKanjiMeaning
    ON tblKanjiMeaning.kanjiID = #tmpKanji.kanjiID
WHERE
    tblKanjiMeaning.langID = @langID
ORDER BY
    #tmpKanji.kanjiID ASC,
    tblKanjiMeaning.sorting ASC

-- Output 3 (MasterData)
SELECT *
FROM #tmpKanji

GO
