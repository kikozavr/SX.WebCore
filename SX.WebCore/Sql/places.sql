/************************************************************
 * Code formatted by SoftTree SQL Assistant © v6.5.278
 * Time: 21.06.2016 12:35:14
 ************************************************************/

/*******************************************
 * get place
 *******************************************/
IF OBJECT_ID(N'dbo.get_place', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_place;
GO
CREATE PROCEDURE dbo.get_place
	@placeId INT
AS
BEGIN
	SELECT TOP(1) *
	FROM   D_PLACE            AS dp
	       LEFT JOIN D_PLACE  AS dp2
	            ON  dp2.Id = dp.ParentPlaceId
	WHERE  dp2.Id = @placeId
END
GO

/*******************************************
 * create place
 *******************************************/
IF OBJECT_ID(N'dbo.add_place', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_place;
GO
CREATE PROCEDURE dbo.add_place
	@title NVARCHAR(100),
	@titleUrl VARCHAR(255),
	@parentPlaceId INT
AS
BEGIN
	IF NOT EXISTS (
	       SELECT TOP(1) *
	       FROM   D_PLACE AS dp
	       WHERE  dp.TitleUrl = @titleUrl
	   )
	BEGIN
	    INSERT INTO D_PLACE
	      (
	        Title,
	        TitleUrl,
	        ParentPlaceId,
	        DateUpdate,
	        DateCreate
	      )
	    VALUES
	      (
	        @title,
	        @titleUrl,
	        @parentPlaceId,
	        GETDATE(),
	        GETDATE()
	      )
	    
	    DECLARE @placeId INT
	    SET @placeId = @@identity
	    
	    EXEC dbo.get_place @placeId
	END
END
GO

/*******************************************
 * update place
 *******************************************/
IF OBJECT_ID(N'dbo.update_place', N'P') IS NOT NULL
    DROP PROCEDURE dbo.update_place;
GO
CREATE PROCEDURE dbo.update_place
	@placeId INT,
	@title NVARCHAR(100),
	@titleUrl VARCHAR(255),
	@parentPlaceId INT
AS
BEGIN
	UPDATE D_PLACE
	SET    Title = @title,
	       TitleUrl = @parentPlaceId,
	       ParentPlaceId = @parentPlaceId,
	       DateUpdate = GETDATE()
	WHERE  Id = @placeId
	
	EXEC dbo.get_place @placeId
END
GO

/*******************************************
 * delete place
 *******************************************/
IF OBJECT_ID(N'dbo.del_place', N'P') IS NOT NULL
    DROP PROCEDURE dbo.del_place;
GO
CREATE PROCEDURE dbo.del_place
	@placeId INT
AS
BEGIN
	WITH j(Id, ParentPlaceId) AS(
	         SELECT dp.Id,
	                dp.ParentPlaceId
	         FROM   D_PLACE AS dp
	         WHERE  dp.Id = @placeId
	         UNION ALL
	         SELECT dp.Id,
	                dp.ParentPlaceId
	         FROM   D_PLACE  AS dp
	                JOIN j   AS j
	                     ON  j.Id = dp.ParentPlaceId
	     )
	
	DELETE 
	FROM   D_PLACE
	WHERE  id IN (SELECT j.Id
	              FROM   j AS j)
END
GO