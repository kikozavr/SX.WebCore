/************************************************************
 * Code formatted by SoftTree SQL Assistant © v6.5.278
 * Time: 21.06.2016 12:44:29
 ************************************************************/

/*******************************************
 * get site partner
 *******************************************/
IF OBJECT_ID(N'dbo.get_site_partner', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_site_partner;
GO
CREATE PROCEDURE dbo.get_site_partner
	@partnerId INT
AS
BEGIN
	SELECT TOP(1) *
	FROM   D_SITE_PARTNER       AS dsp
	       LEFT JOIN D_PICTURE  AS dp
	            ON  dp.Id = dsp.PictureId
	WHERE  dsp.Id = @partnerId
END
GO

/*******************************************
 * create place
 *******************************************/
IF OBJECT_ID(N'dbo.add_site_partner', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_site_partner;
GO
CREATE PROCEDURE dbo.add_site_partner
	@title NVARCHAR(100),
	@titleUrl VARCHAR(255),
	@url VARCHAR(255),
	@pictureId UNIQUEIDENTIFIER
AS
BEGIN
	IF NOT EXISTS (
	       SELECT TOP(1) *
	       FROM   D_SITE_PARTNER AS dsp
	       WHERE  dsp.TitleUrl = @titleUrl
	   )
	BEGIN
	    INSERT INTO D_SITE_PARTNER
	      (
	        Title,
	        TitleUrl,
	        [Url],
	        PictureId,
	        DateUpdate,
	        DateCreate
	      )
	    VALUES
	      (
	        @title,
	        @titleUrl,
	        @url,
	        @pictureId,
	        GETDATE(),
	        GETDATE()
	      )
	    
	    DECLARE @partnerId INT
	    SET @partnerId = @@identity
	    
	    EXEC dbo.get_site_partner @partnerId
	END
END
GO

/*******************************************
 * update site partner
 *******************************************/
IF OBJECT_ID(N'dbo.update_site_partner', N'P') IS NOT NULL
    DROP PROCEDURE dbo.update_site_partner;
GO
CREATE PROCEDURE dbo.update_site_partner
	@partnerId INT,
	@title NVARCHAR(100),
	@titleUrl VARCHAR(255),
	@url VARCHAR(255),
	@pictureId UNIQUEIDENTIFIER
AS
BEGIN
	UPDATE D_SITE_PARTNER
	SET    Title = @title,
	       TitleUrl = @titleUrl,
	       [Url] = @url,
	       PictureId = @pictureId,
	       DateUpdate = GETDATE()
	WHERE  Id = @partnerId
	
	EXEC dbo.get_site_partner @partnerId
END
GO

/*******************************************
 * delete site partner
 *******************************************/
IF OBJECT_ID(N'dbo.del_site_partner', N'P') IS NOT NULL
    DROP PROCEDURE dbo.del_site_partner;
GO
CREATE PROCEDURE dbo.del_site_partner
	@partnerId INT
AS
BEGIN
	DELETE 
	FROM   D_SITE_PARTNER
	WHERE  Id = @partnerId
END
GO