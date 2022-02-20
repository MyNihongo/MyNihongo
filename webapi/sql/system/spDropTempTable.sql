IF (OBJECT_ID('spDropTempTable') IS NOT NULL)
	DROP PROCEDURE spDropTempTable
GO

CREATE PROCEDURE spDropTempTable
    @strTempTable varchar(255)
AS

SET NOCOUNT ON

IF OBJECT_ID('tempdb..' + @strTempTable) IS NULL
    RETURN

DECLARE @sql varchar(MAX) = CONCAT('DROP TABLE ', @strTempTable)
EXEC (@sql)
GO
