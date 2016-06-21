/************************************************************
 * Code formatted by SoftTree SQL Assistant © v6.5.278
 * Time: 21.06.2016 14:37:27
 ************************************************************/

 /*******************************************
 * add client request
 *******************************************/
IF OBJECT_ID(N'dbo.add_request', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_request;
GO
CREATE PROCEDURE dbo.add_request
	@id UNIQUEIDENTIFIER,
	@browser VARCHAR(150),
	@clientIP VARCHAR(150),
	@rawUrl VARCHAR(255),
	@requestType VARCHAR(20),
	@urlRef VARCHAR(MAX),
	@sessionId VARCHAR(128),
	@userAgent VARCHAR(150)
AS
BEGIN
	INSERT INTO D_REQUEST
	  (
	    Id,
	    SessionId,
	    UrlRef,
	    Browser,
	    ClientIP,
	    UserAgent,
	    RequestType,
	    DateCreate,
	    RawUrl
	  )
	VALUES
	  (
	    @id,
	    @sessionId,
	    @urlRef,
	    @browser,
	    @clientIP,
	    @userAgent,
	    @requestType,
	    GETDATE(),
	    @rawUrl
	  )
END
GO