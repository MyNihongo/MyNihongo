/*
    Updates the access info for the token and returns the connection ID

    -- Sample
    EXEC spAuthConnectionSet
        @tokenID = ,
        @userID = 1,
        @ipAddress = '192.168.102.254',
        @clientInfo = 'Samsung Galaxy A52s 5G',
        @tickNow = 12345
*/

IF (OBJECT_ID('spAuthConnectionSet') IS NOT NULL)
	DROP PROCEDURE spAuthConnectionSet
GO

CREATE PROCEDURE spAuthConnectionSet
    @tokenID UNIQUEIDENTIFIER,
    @userID BIGINT,
    @ipAddress VARCHAR(255),
    @clientInfo VARCHAR(255),
    @tickNow BIGINT
AS

SET NOCOUNT ON

UPDATE TOP(1)
    tblAuthConnection
SET
    tblAuthConnection.ipAddress = @ipAddress,
    tblAuthConnection.clientInfo = @clientInfo,
    tblAuthConnection.ticksLatestAccess = @tickNow
OUTPUT
    inserted.connectionID
FROM
    tblAuthConnection
    INNER JOIN tblAuthConnectionToken
        ON tblAuthConnectionToken.connectionID = tblAuthConnection.connectionID
WHERE
    tblAuthConnectionToken.tokenID = @tokenID
    AND
    tblAuthConnectionToken.ticksValidTo < @tickNow
    AND
    tblAuthConnection.userID = @userID

GO
