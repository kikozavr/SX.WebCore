/************************************************************
 * Code formatted by SoftTree SQL Assistant © v6.5.278
 * Time: 21.06.2016 14:39:17
 ************************************************************/
 
/*******************************************
 * get picture
 *******************************************/
IF OBJECT_ID(N'dbo.get_picture', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_picture;
GO
CREATE PROCEDURE dbo.get_picture
	@pictureId UNIQUEIDENTIFIER
AS
BEGIN
	SELECT TOP(1) *
	FROM   D_PICTURE dp
	WHERE  dp.ID = @pictureId
END
GO

/*******************************************
 * delete picture
 *******************************************/
IF OBJECT_ID(N'dbo.del_picture', N'P') IS NOT NULL
    DROP PROCEDURE dbo.del_picture;
GO
CREATE PROCEDURE dbo.del_picture
	@pictureId UNIQUEIDENTIFIER
AS
BEGIN
	BEGIN TRANSACTION
	
	UPDATE DV_MATERIAL
	SET    FrontPictureId = NULL
	WHERE  FrontPictureId = @pictureId
	
	UPDATE AspNetUsers
	SET    AvatarId = NULL
	WHERE  AvatarId = @pictureId
	
	UPDATE D_MATERIAL_CATEGORY
	SET    FrontPictureId = NULL
	WHERE  FrontPictureId = @pictureId
	
	UPDATE D_AUTHOR_APHORISM
	SET    PictureId = NULL
	WHERE  PictureId = @pictureId
	
	UPDATE D_SITE_PARTNER
	SET    PictureId = NULL
	WHERE  PictureId = @pictureId
	
	DELETE 
	FROM   D_PICTURE
	WHERE  Id = @pictureId
	
	COMMIT TRANSACTION
END
GO