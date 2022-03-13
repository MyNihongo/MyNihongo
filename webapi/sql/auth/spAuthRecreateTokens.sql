/*
    Revokes all current tokens and creates new access and refresh tokens

    -- Sample
    EXEC spAuthRecreateTokens
        @connectionID = ,
        @ticksAccessValidTo = 12345,
        @ticksRefreshValidTo = 123456
*/

IF (OBJECT_ID('spAuthRecreateTokens') IS NOT NULL)
	DROP PROCEDURE spAuthRecreateTokens
GO

CREATE PROCEDURE spAuthRecreateTokens
    @connectionID UNIQUEIDENTIFIER,
    @ticksAccessValidTo BIGINT,
    @ticksRefreshValidTo BIGINT
AS

SET NOCOUNT ON

-- Revoke all existing tokens
DELETE
    tblAuthConnectionToken
WHERE
    tblAuthConnectionToken.connectionID = @connectionID


-- Create new tokens
DECLARE @accessTokenID UNIQUEIDENTIFIER = NEWID(),
    @refreshTokenID UNIQUEIDENTIFIER = NEWID()

INSERT tblAuthConnectionToken (tokenID, connectionID, ticksValidTo) VALUES (@accessTokenID, @connectionID, @ticksAccessValidTo)
INSERT tblAuthConnectionToken (tokenID, connectionID, ticksValidTo) VALUES (@refreshTokenID, @connectionID, @ticksRefreshValidTo)

SELECT @accessTokenID, @refreshTokenID

GO
