/************************************************************
 * Code formatted by SoftTree SQL Assistant � v6.5.278
 * Time: 14.09.2016 15:59:23
 ************************************************************/

/*******************************************
 * �������� ���� html
 *******************************************/
IF OBJECT_ID(N'dbo.func_strip_html', N'FN') IS NOT NULL
    DROP FUNCTION dbo.func_strip_html;
GO 
CREATE FUNCTION dbo.func_strip_html
(
	@HTMLText NVARCHAR(MAX)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE @Start INT
	DECLARE @End INT
	DECLARE @Length INT
	
	-- Replace the HTML entity &amp; with the '&' character (this needs to be done first, as
	-- '&' might be double encoded as '&amp;amp;')
	SET @Start = CHARINDEX('&amp;', @HTMLText)
	SET @End = @Start + 4
	SET @Length = (@End - @Start) + 1
	
	WHILE (@Start > 0 AND @End > 0 AND @Length > 0)
	BEGIN
	    SET @HTMLText = STUFF(@HTMLText, @Start, @Length, '&')
	    SET @Start = CHARINDEX('&amp;', @HTMLText)
	    SET @End = @Start + 4
	    SET @Length = (@End - @Start) + 1
	END
	
	-- Replace the HTML entity &ndash; with the ' - ' character
	SET @Start = CHARINDEX('&ndash;', @HTMLText)
	SET @End = @Start + 6
	SET @Length = (@End - @Start) + 1
	
	WHILE (@Start > 0 AND @End > 0 AND @Length > 0)
	BEGIN
	    SET @HTMLText = STUFF(@HTMLText, @Start, @Length, ' - ')
	    SET @Start = CHARINDEX('&ndash;', @HTMLText)
	    SET @End = @Start + 6
	    SET @Length = (@End - @Start) + 1
	END
	
	-- Replace the HTML entity &laquo; with the ' " ' character
	SET @Start = CHARINDEX('&laquo;', @HTMLText)
	SET @End = @Start + 6
	SET @Length = (@End - @Start) + 1
	
	WHILE (@Start > 0 AND @End > 0 AND @Length > 0)
	BEGIN
	    SET @HTMLText = STUFF(@HTMLText, @Start, @Length, '"')
	    SET @Start = CHARINDEX('&laquo;', @HTMLText)
	    SET @End = @Start + 6
	    SET @Length = (@End - @Start) + 1
	END
	
	-- Replace the HTML entity &raquo; with the ' " ' character
	SET @Start = CHARINDEX('&raquo;', @HTMLText)
	SET @End = @Start + 6
	SET @Length = (@End - @Start) + 1
	
	WHILE (@Start > 0 AND @End > 0 AND @Length > 0)
	BEGIN
	    SET @HTMLText = STUFF(@HTMLText, @Start, @Length, '"')
	    SET @Start = CHARINDEX('&raquo;', @HTMLText)
	    SET @End = @Start + 6
	    SET @Length = (@End - @Start) + 1
	END
	
	-- Replace the HTML entity &lt; with the '<' character
	SET @Start = CHARINDEX('&lt;', @HTMLText)
	SET @End = @Start + 3
	SET @Length = (@End - @Start) + 1
	
	WHILE (@Start > 0 AND @End > 0 AND @Length > 0)
	BEGIN
	    SET @HTMLText = STUFF(@HTMLText, @Start, @Length, '<')
	    SET @Start = CHARINDEX('&lt;', @HTMLText)
	    SET @End = @Start + 3
	    SET @Length = (@End - @Start) + 1
	END
	
	-- Replace the HTML entity &gt; with the '>' character
	SET @Start = CHARINDEX('&gt;', @HTMLText)
	SET @End = @Start + 3
	SET @Length = (@End - @Start) + 1
	
	WHILE (@Start > 0 AND @End > 0 AND @Length > 0)
	BEGIN
	    SET @HTMLText = STUFF(@HTMLText, @Start, @Length, '>')
	    SET @Start = CHARINDEX('&gt;', @HTMLText)
	    SET @End = @Start + 3
	    SET @Length = (@End - @Start) + 1
	END
	
	-- Replace the HTML entity &amp; with the '&' character
	SET @Start = CHARINDEX('&amp;amp;', @HTMLText)
	SET @End = @Start + 4
	SET @Length = (@End - @Start) + 1
	
	WHILE (@Start > 0 AND @End > 0 AND @Length > 0)
	BEGIN
	    SET @HTMLText = STUFF(@HTMLText, @Start, @Length, '&')
	    SET @Start = CHARINDEX('&amp;amp;', @HTMLText)
	    SET @End = @Start + 4
	    SET @Length = (@End - @Start) + 1
	END
	
	-- Replace the HTML entity &nbsp; with the ' ' character
	SET @Start = CHARINDEX('&nbsp;', @HTMLText)
	SET @End = @Start + 5
	SET @Length = (@End - @Start) + 1
	
	WHILE (@Start > 0 AND @End > 0 AND @Length > 0)
	BEGIN
	    SET @HTMLText = STUFF(@HTMLText, @Start, @Length, ' ')
	    SET @Start = CHARINDEX('&nbsp;', @HTMLText)
	    SET @End = @Start + 5
	    SET @Length = (@End - @Start) + 1
	END
	
	-- Replace the HTML entity &quot; with the '"' character
	SET @Start = CHARINDEX('&quot;', @HTMLText)
	SET @End = @Start + 5
	SET @Length = (@End - @Start) + 1
	
	WHILE (@Start > 0 AND @End > 0 AND @Length > 0)
	BEGIN
	    SET @HTMLText = STUFF(@HTMLText, @Start, @Length, '"')
	    SET @Start = CHARINDEX('&quot;', @HTMLText)
	    SET @End = @Start + 5
	    SET @Length = (@End - @Start) + 1
	END
	
	-- Replace any <br> tags with a newline
	SET @Start = CHARINDEX('<br>', @HTMLText)
	SET @End = @Start + 3
	SET @Length = (@End - @Start) + 1
	
	WHILE (@Start > 0 AND @End > 0 AND @Length > 0)
	BEGIN
	    SET @HTMLText = STUFF(@HTMLText, @Start, @Length, CHAR(13) + CHAR(10))
	    --CHAR(13) + CHAR(10)
	    SET @Start = CHARINDEX('<br>', @HTMLText)
	    SET @End = @Start + 3
	    SET @Length = (@End - @Start) + 1
	END
	
	-- Replace any <br/> tags with a newline
	SET @Start = CHARINDEX('<br/>', @HTMLText)
	SET @End = @Start + 4
	SET @Length = (@End - @Start) + 1
	
	WHILE (@Start > 0 AND @End > 0 AND @Length > 0)
	BEGIN
	    SET @HTMLText = STUFF(@HTMLText, @Start, @Length, CHAR(13) + CHAR(10))
	    --CHAR(13) + CHAR(10)
	    SET @Start = CHARINDEX('<br/>', @HTMLText)
	    SET @End = @Start + 4
	    SET @Length = (@End - @Start) + 1
	END
	
	-- Replace any <br /> tags with a newline
	SET @Start = CHARINDEX('<br />', @HTMLText)
	SET @End = @Start + 5
	SET @Length = (@End - @Start) + 1
	
	WHILE (@Start > 0 AND @End > 0 AND @Length > 0)
	BEGIN
	    SET @HTMLText = STUFF(@HTMLText, @Start, @Length, CHAR(13) + CHAR(10))
	    --CHAR(13) + CHAR(10)
	    SET @Start = CHARINDEX('<br />', @HTMLText)
	    SET @End = @Start + 5
	    SET @Length = (@End - @Start) + 1
	END
	
	-- Remove anything between <whatever> tags
	SET @Start = CHARINDEX('<', @HTMLText)
	SET @End = CHARINDEX('>', @HTMLText, CHARINDEX('<', @HTMLText))
	SET @Length = (@End - @Start) + 1
	
	WHILE (@Start > 0 AND @End > 0 AND @Length > 0)
	BEGIN
	    SET @HTMLText = STUFF(@HTMLText, @Start, @Length, '')
	    SET @Start = CHARINDEX('<', @HTMLText)
	    SET @End = CHARINDEX('>', @HTMLText, CHARINDEX('<', @HTMLText))
	    SET @Length = (@End - @Start) + 1
	END
	
	RETURN LTRIM(RTRIM(@HTMLText))
END
GO









/*******************************************
 * ������ ����������
 *******************************************/
IF OBJECT_ID(N'dbo.get_preview_materials', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_preview_materials;
GO
CREATE PROCEDURE dbo.get_preview_materials(
    @lettersCount     INT,
    @gameTitle        VARCHAR(100),
    @categoryId       VARCHAR(100)
)
AS
BEGIN
	SELECT TOP(8) da.Id,
	       dm.Title,
	       dm.TitleUrl,
	       dm.DateCreate,
	       dm.ModelCoreType,
	       dm.DateOfPublication,
	       dm.ViewsCount,
	       dbo.get_comments_count(dm.Id, dm.ModelCoreType) AS CommentsCount,
	       CASE 
	            WHEN dm.Foreword IS NOT NULL THEN dm.Foreword
	            ELSE SUBSTRING(dbo.func_strip_html(dm.Html), 0, @lettersCount) +
	                 '...'
	       END                    AS Foreword,
	       anu.NikName            AS UserName,
	       dg.Title               AS GameTitle
	FROM   D_ARTICLE              AS da
	       JOIN DV_MATERIAL       AS dm
	            ON  dm.Id = da.Id
	            AND dm.ModelCoreType = da.ModelCoreType
	            AND (dm.Show = 1 AND dm.DateOfPublication <= GETDATE())
	       LEFT JOIN AspNetUsers  AS anu
	            ON  anu.Id = dm.UserId
	       LEFT JOIN D_GAME       AS dg
	            ON  dg.Id = da.GameId
	WHERE  (@gameTitle IS NULL)
	       OR  (
	               @gameTitle IS NOT NULL
	               AND @categoryId IS NULL
	               AND dg.TitleUrl = @gameTitle
	           )
	       OR  (
	               @gameTitle IS NOT NULL
	               AND @categoryId IS NOT NULL
	               AND dg.TitleUrl = @gameTitle
	               AND dm.CategoryId = @categoryId
	           )
	ORDER BY
	       dm.DateCreate DESC
END
GO

/*******************************************
 * �������� �������� �� TitleUrl
 *******************************************/
IF OBJECT_ID(N'dbo.get_material_by_url', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_material_by_url;
GO
CREATE PROCEDURE dbo.get_material_by_url
	@year INT,
	@month INT,
	@day INT,
	@title_url NVARCHAR(255),
	@mct INT
AS
BEGIN
	--SELECT dm.*,
	--       dh.UserName,
	--       dg.TitleUrl            AS GameTitleUrl,
	--       CASE
	--            WHEN dm.Foreword IS NOT NULL THEN dm.Foreword
	--            ELSE SUBSTRING(dbo.FUNC_STRIP_HTML(dm.Html), 0, 200) +
	--                 '...'
	--       END                    AS Foreword,
	--       (
	--           SELECT COUNT(1)
	--           FROM   D_COMMENT AS dc
	--           WHERE  dc.MaterialId = dm.Id
	--                  AND dc.ModelCoreType = dm.ModelCoreType
	--       )                      AS CommentsCount,
	--       anu.NikName            AS UserNikName
	--FROM   DV_MATERIAL            AS dm
	--       LEFT JOIN D_ARTICLE    AS da
	--            ON  da.ModelCoreType = dm.ModelCoreType
	--            AND da.Id = dm.Id
	--       LEFT JOIN D_NEWS       AS dn
	--            ON  dn.Id = dm.Id
	--            AND dn.ModelCoreType = dm.ModelCoreType
	--       LEFT JOIN D_HUMOR      AS dh
	--            ON  dh.ModelCoreType = dm.ModelCoreType
	--            AND dh.Id = dm.Id
	--       LEFT JOIN D_GAME       AS dg
	--            ON  (dg.Id = da.GameId OR dg.Id = dn.GameId)
	--       LEFT JOIN AspNetUsers  AS anu
	--            ON  anu.Id = dm.UserId
	--WHERE  dm.TitleUrl = @title_url
	--       AND dm.Show = 1
	--       AND dm.DateOfPublication <= GETDATE()
	--       AND dm.ModelCoreType = @mct
	--       AND (
	--               YEAR(dm.DateCreate) = @year
	--               AND MONTH(dm.DateCreate) = @month
	--               AND DAY(dm.DateCreate) = @day
	--           )
	SELECT dm.*,
	       (
	           SELECT COUNT(1)
	           FROM   D_COMMENT AS dc
	           WHERE  dc.MaterialId = dm.Id
	                  AND dc.ModelCoreType = @mct
	       )                              AS CommentsCount,
	       dmc.*,
	       anu.*,
	       dp.Id,
	       dp.Width,
	       dp.Height,
	       dst.*
	FROM   DV_MATERIAL                    AS dm
	       LEFT JOIN D_MATERIAL_CATEGORY  AS dmc
	            ON  dmc.Id = dm.CategoryId
	       LEFT JOIN AspNetUsers          AS anu
	            ON  anu.Id = dm.UserId
	       LEFT JOIN D_PICTURE            AS dp
	            ON  dp.Id = dm.FrontPictureId
	       LEFT JOIN D_SEO_TAGS           AS dst
	            ON  dst.MaterialId = dm.Id
	            AND dst.ModelCoreType = @mct
	WHERE  (
	           YEAR(dm.DateCreate) = @year
	           AND MONTH(dm.DateCreate) = @month
	           AND DAY(dm.DateCreate) = @day
	       )
	       AND dm.TitleUrl = @title_url
	       AND dm.ModelCoreType = @mct
	       AND dm.DateOfPublication <= GETDATE()
END
GO
	
/*******************************************
 * �������� �������� �� id
 *******************************************/
IF OBJECT_ID(N'dbo.get_material_by_id', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_material_by_id;
GO
CREATE PROCEDURE dbo.get_material_by_id
	@id INT,
	@mct INT
AS
BEGIN
	SELECT dm.*,
	       (
	           SELECT COUNT(1)
	           FROM   D_COMMENT AS dc
	           WHERE  dc.MaterialId = dm.Id
	                  AND dc.ModelCoreType = @mct
	       )                              AS CommentsCount,
	       dmc.*,
	       anu.*,
	       dp.Id,
	       dp.Width,
	       dp.Height,
	       dp.Caption
	FROM   DV_MATERIAL                    AS dm
	       LEFT JOIN D_MATERIAL_CATEGORY  AS dmc
	            ON  dmc.Id = dm.CategoryId
	       LEFT JOIN AspNetUsers          AS anu
	            ON  anu.Id = dm.UserId
	       LEFT JOIN D_PICTURE            AS dp
	            ON  dp.Id = dm.FrontPictureId
	WHERE  dm.Id = @id
	       AND dm.ModelCoreType = @mct
END
GO

/*******************************************
 * �������� ����� ���������
 *******************************************/
IF OBJECT_ID(N'dbo.get_material_videos', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_material_videos;
GO
CREATE PROCEDURE dbo.get_material_videos(@mid INT, @mct INT)
AS
BEGIN
	SELECT dv.*
	FROM   D_VIDEO_LINK         AS dvl
	       JOIN D_VIDEO         AS dv
	            ON  dv.Id = dvl.VideoId
	       LEFT JOIN D_ARTICLE  AS da
	            ON  da.ModelCoreType = dvl.ModelCoreType
	            AND da.Id = @mid
	       LEFT JOIN D_NEWS     AS dn
	            ON  dn.ModelCoreType = dvl.ModelCoreType
	            AND dn.Id = @mid
	WHERE  dvl.ModelCoreType = @mct
	       AND dvl.MaterialId = @mid
END
GO

/*******************************************
 * �������� ����������� ���������
 *******************************************/
IF OBJECT_ID(N'dbo.get_material_comments', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_material_comments;
GO
CREATE PROCEDURE dbo.get_material_comments(@mid INT, @mct INT)
AS
BEGIN
	SELECT dc.*,
	       anu.*
	FROM   D_COMMENT              AS dc
	       LEFT JOIN AspNetUsers  AS anu
	            ON  anu.Id = dc.UserId
	WHERE  dc.MaterialId = @mid
	       AND dc.ModelCoreType = @mct
	ORDER BY
	       dc.DateCreate DESC
END
GO

/*******************************************
 * ������� ��������� ���-�� ������������ ���������
 *******************************************/
IF OBJECT_ID(N'dbo.get_comments_count', N'FN') IS NOT NULL
    DROP FUNCTION dbo.get_comments_count;
GO
CREATE FUNCTION dbo.get_comments_count
(
	@mid     INT,
	@mct     INT
)
RETURNS INT
AS
BEGIN
	DECLARE @res INT
	SELECT @res = COUNT(1)
	FROM   D_COMMENT AS dc
	WHERE  dc.MaterialId = @mid
	       AND dc.ModelCoreType = @mct
	
	RETURN @res
END
GO






/*******************************************
 * �������� ����������� ���������
 *******************************************/
IF OBJECT_ID(N'dbo.add_material_comment', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_material_comment;
GO
CREATE PROCEDURE dbo.add_material_comment(
    @mid      INT,
    @mct      INT,
    @uid      NVARCHAR(128),
    @html     NVARCHAR(MAX),
    @un       NVARCHAR(50)
)
AS
BEGIN
	INSERT INTO D_COMMENT
	  (
	    MaterialId,
	    ModelCoreType,
	    UserId,
	    Html,
	    DateUpdate,
	    DateCreate,
	    UserName
	  )
	VALUES
	  (
	    @mid,
	    @mct,
	    @uid,
	    @html,
	    GETDATE(),
	    GETDATE(),
	    @un
	  )
	
	DECLARE @id INT
	SELECT @id = @@identity
	SELECT *
	FROM   D_COMMENT AS dc
	WHERE  dc.Id = @id
END
GO

/*******************************************
 * �������� ��������� ����������
 *******************************************/
IF OBJECT_ID(N'dbo.get_material_category', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_material_category;
GO
CREATE PROCEDURE dbo.get_material_category
	@categoryId NVARCHAR(128)
AS
BEGIN
	SELECT *
	FROM   D_MATERIAL_CATEGORY            AS dmc
	       LEFT JOIN D_PICTURE            AS dp
	            ON  dp.Id = dmc.FrontPictureId
	       LEFT JOIN D_MATERIAL_CATEGORY  AS dmc2
	            ON  dmc2.Id = dmc.ParentCategoryId
	WHERE  dmc.Id = @categoryId
END
GO

/*******************************************
 * �������� ��������� ����������
 *******************************************/
IF OBJECT_ID(N'dbo.update_material_category', N'P') IS NOT NULL
    DROP PROCEDURE dbo.update_material_category;
GO
CREATE PROCEDURE dbo.update_material_category
	@oldCategoryId NVARCHAR(128),
	@categoryId NVARCHAR(128),
	@title NVARCHAR(100),
	@mct INT,
	@pcid NVARCHAR(128),
	@pictureId UNIQUEIDENTIFIER
AS
BEGIN
	IF (@oldCategoryId = @categoryId)
	BEGIN
	    UPDATE D_MATERIAL_CATEGORY
	    SET    Title                = @title,
	           ModelCoreType        = @mct,
	           ParentCategoryId     = @pcid,
	           FrontPictureId       = @pictureId
	    WHERE  Id                   = @categoryId
	END
	ELSE
	BEGIN
	    IF NOT EXISTS (
	           SELECT TOP 1 dmc.Id
	           FROM   D_MATERIAL_CATEGORY AS dmc
	           WHERE  dmc.Id = @categoryId
	       )
	    BEGIN
	        BEGIN TRANSACTION
	        
	        ALTER TABLE [dbo].[DV_MATERIAL] DROP CONSTRAINT 
	        [FK_dbo.DV_MATERIAL_dbo.D_MATERIAL_CATEGORY_CategoryId];
	        
	        UPDATE DV_MATERIAL
	        SET    CategoryId = @categoryId
	        WHERE  CategoryId = @oldCategoryId
	        
	        PRINT '1'
	        
	        UPDATE D_MATERIAL_CATEGORY
	        SET    ParentCategoryId = @categoryId
	        WHERE  ParentCategoryId = @oldCategoryId
	        
	        PRINT '2'
	        
	        UPDATE D_MATERIAL_CATEGORY
	        SET    Id = @categoryId,
	               Title = @title,
	               ModelCoreType = @mct,
	               ParentCategoryId = @pcid,
	               FrontPictureId = @pictureId
	        WHERE  Id = @oldCategoryId
	        
	        PRINT '3'
	        
	        ALTER TABLE [dbo].[DV_MATERIAL]  
	        WITH CHECK ADD CONSTRAINT 
	             [FK_dbo.DV_MATERIAL_dbo.D_MATERIAL_CATEGORY_CategoryId] FOREIGN 
	             KEY([CategoryId])
	             REFERENCES [dbo].[D_MATERIAL_CATEGORY] ([Id]);
	        
	        PRINT '4'
	        
	        ALTER TABLE [dbo].[DV_MATERIAL] CHECK CONSTRAINT 
	        [FK_dbo.DV_MATERIAL_dbo.D_MATERIAL_CATEGORY_CategoryId];
	        
	        PRINT '5'
	        
	        COMMIT TRANSACTION
	    END
	END
	
	EXEC dbo.get_material_category @categoryId
END
GO

/*******************************************
 * �������� ��������� ����������
 *******************************************/
IF OBJECT_ID(N'dbo.add_material_category', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_material_category;
GO
CREATE PROCEDURE dbo.add_material_category
	@categoryId NVARCHAR(128),
	@title NVARCHAR(100),
	@mct INT,
	@pcid NVARCHAR(128),
	@pictureId UNIQUEIDENTIFIER
AS
BEGIN
	IF NOT EXISTS (
	       SELECT TOP 1 dmc.Id
	       FROM   D_MATERIAL_CATEGORY AS dmc
	       WHERE  dmc.Id = @categoryId
	   )
	BEGIN
	    DECLARE @date DATETIME = GETDATE()
	    INSERT INTO D_MATERIAL_CATEGORY
	      (
	        Id,
	        Title,
	        ModelCoreType,
	        ParentCategoryId,
	        FrontPictureId,
	        DateCreate
	      )
	    VALUES
	      (
	        @categoryId,
	        @title,
	        @mct,
	        @pcid,
	        @pictureId,
	        @date
	      )
	    
	    EXEC dbo.get_material_category @categoryId
	END
END
GO

/*******************************************
 * ������� ��������� ����������
 *******************************************/
IF OBJECT_ID(N'dbo.del_material_category', N'P') IS NOT NULL
    DROP PROCEDURE dbo.del_material_category;
GO
CREATE PROCEDURE dbo.del_material_category
	@catId VARCHAR(100)
AS
BEGIN
	BEGIN TRANSACTION
	
	DECLARE @idForDel TABLE (Id VARCHAR(100));
	WITH j(Id, ParentCategoryId) AS
	     (
	         SELECT dmc1.Id,
	                dmc1.ParentCategoryId
	         FROM   D_MATERIAL_CATEGORY AS dmc1
	         WHERE  dmc1.Id = @catId
	         UNION ALL
	         SELECT dmc2.Id,
	                dmc2.ParentCategoryId
	         FROM   D_MATERIAL_CATEGORY AS dmc2
	                JOIN j
	                     ON  dmc2.ParentCategoryId = j.Id
	     )
	
	INSERT INTO @idForDel
	SELECT j.Id
	FROM   j AS j
	
	--������� �������� ���������
	DELETE 
	FROM   D_APHORISM
	WHERE  Id IN (SELECT dm.Id
	              FROM   DV_MATERIAL      AS dm
	                     JOIN D_APHORISM  AS da
	                          ON  da.Id = dm.Id
	                          AND da.ModelCoreType = dm.ModelCoreType
	              WHERE  dm.CategoryId IN (SELECT fd.Id
	                                       FROM   @idForDel fd))
	
	DELETE 
	FROM   DV_MATERIAL
	WHERE  TitleUrl IN (SELECT dm.TitleUrl
	                    FROM   DV_MATERIAL AS dm
	                           JOIN D_APHORISM AS da
	                                ON  da.Id = dm.Id
	                                AND da.ModelCoreType = dm.ModelCoreType
	                    WHERE  dm.CategoryId IN (SELECT fd.Id
	                                             FROM   @idForDel fd))
	
	
	--�������� ������ � �������
	UPDATE DV_MATERIAL
	SET    CategoryId = NULL
	WHERE  CategoryId IN (SELECT fd.Id
	                      FROM   @idForDel fd) 
	
	--������� ���������	         
	DELETE 
	FROM   D_MATERIAL_CATEGORY
	WHERE  Id IN (SELECT fd.Id
	              FROM   @idForDel fd) 
	
	COMMIT TRANSACTION
END
GO

/*******************************************
* �������� ����� �� ���������
*******************************************/
IF OBJECT_ID(N'dbo.get_report_aphorisms', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_report_aphorisms;
GO
CREATE PROCEDURE dbo.get_report_aphorisms
	@catId VARCHAR(255)
AS
BEGIN
	SELECT dmc.Title                    AS Category,
	       daa.Name                     AS Author,
	       dm.DateCreate,
	       dm.Title,
	       dm.Html
	FROM   D_APHORISM                   AS da
	       JOIN DV_MATERIAL             AS dm
	            ON  dm.Id = da.Id
	            AND dm.ModelCoreType = da.ModelCoreType
	       LEFT JOIN D_AUTHOR_APHORISM  AS daa
	            ON  daa.Id = da.AuthorId
	       JOIN D_MATERIAL_CATEGORY     AS dmc
	            ON  dmc.Id = dm.CategoryId
	ORDER BY
	       dm.DateCreate DESC
END
GO

/*******************************************
* �������� �������� ��������
*******************************************/
IF OBJECT_ID(N'dbo.get_aphorism_page_model', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_aphorism_page_model;
GO
CREATE PROCEDURE dbo.get_aphorism_page_model
(
    @title_url         NVARCHAR(255),
    @author_amount     INT,
    @cat_amount        INT
)
AS
BEGIN
	DECLARE @authorId     INT,
	        @catId        NVARCHAR(100)
	
	SELECT @authorId = da.AuthorId,
	       @catId = dm.CategoryId
	FROM   D_APHORISM        AS da
	       JOIN DV_MATERIAL  AS dm
	            ON  dm.Id = da.Id
	            AND dm.ModelCoreType = da.ModelCoreType
	WHERE  dm.TitleUrl = @title_url
	
	SELECT x.Id,
	       x.DateOfPublication,
	       x.ViewsCount,
	       x.Flag,
	       x.Title,
	       x.TitleUrl,
	       x.Html,
	       x.Foreword,
	       x.CategoryId,
	       dmc.Id,
	       dmc.Title,
	       x.AuthorId                   AS Id,
	       daa.Name,
	       daa.PictureId,
	       daa.TitleUrl,
	       COUNT(dc.Id)                 AS CommentsCount
	FROM   (
	           SELECT dm.*,
	                  da.AuthorId,
	                  0                 AS Flag
	           FROM   D_APHORISM        AS da
	                  JOIN DV_MATERIAL  AS dm
	                       ON  dm.Id = da.Id
	                       AND dm.ModelCoreType = da.ModelCoreType
	           WHERE  dm.TitleUrl = @title_url
	                  AND dm.Show = 1
	           UNION ALL
	           SELECT TOP(@author_amount) dm.*,
	                  da.AuthorId,
	                  1                 AS Flag
	           FROM   D_APHORISM        AS da
	                  JOIN DV_MATERIAL  AS dm
	                       ON  dm.Id = da.Id
	                       AND dm.ModelCoreType = da.ModelCoreType
	           WHERE  (
	                      (@authorId IS NULL AND da.AuthorId IS NULL)
	                      OR (@authorId IS NOT NULL AND da.AuthorId IN (@authorId))
	                  )
	                  AND (dm.TitleUrl NOT IN (@title_url))
	                  AND dm.Show = 1
	           UNION ALL
	           SELECT TOP(@cat_amount) dm.*,
	                  da.AuthorId,
	                  2                 AS Flag
	           FROM   D_APHORISM        AS da
	                  JOIN DV_MATERIAL  AS dm
	                       ON  dm.Id = da.Id
	                       AND dm.ModelCoreType = da.ModelCoreType
	           WHERE  dm.CategoryId IN (@catId)
	                  AND (dm.TitleUrl NOT IN (@title_url))
	                  AND dm.Show = 1
	       ) x
	       LEFT JOIN D_COMMENT          AS dc
	            ON  dc.MaterialId = x.Id
	            AND dc.ModelCoreType = x.ModelCoreType
	       JOIN D_MATERIAL_CATEGORY     AS dmc
	            ON  dmc.Id = x.CategoryId
	       LEFT JOIN D_AUTHOR_APHORISM  AS daa
	            ON  daa.Id = x.AuthorId
	GROUP BY
	       x.Id,
	       x.DateOfPublication,
	       x.ViewsCount,
	       x.Title,
	       x.TitleUrl,
	       x.Html,
	       x.Foreword,
	       x.CategoryId,
	       x.AuthorId,
	       x.Flag,
	       dmc.Title,
	       daa.Name,
	       daa.PictureId,
	       daa.TitleUrl,
	       dmc.Id
END
GO

/*******************************************
* �������� ��������� �������
*******************************************/
IF OBJECT_ID(N'dbo.get_random_aphorism', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_random_aphorism;
GO
CREATE PROCEDURE dbo.get_random_aphorism(@mid INT)
AS
BEGIN
	SELECT TOP(1) *
	FROM   D_APHORISM                   AS da
	       JOIN DV_MATERIAL             AS dm
	            ON  dm.Id = da.Id
	            AND dm.ModelCoreType = da.ModelCoreType
	       JOIN D_MATERIAL_CATEGORY     AS dmc
	            ON  dmc.Id = dm.CategoryId
	       LEFT JOIN D_AUTHOR_APHORISM  AS daa
	            ON  daa.Id = da.AuthorId
	WHERE  (@mid IS NULL)
	       OR  (@mid IS NOT NULL AND da.Id NOT IN (@mid))
	       AND dm.CategoryId IS NOT NULL
	ORDER BY
	       NEWID()
END
GO


/*******************************************
* �������� ��������� ���������
*******************************************/
IF OBJECT_ID(N'dbo.get_aphorism_categories', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_aphorism_categories;
GO
CREATE PROCEDURE dbo.get_aphorism_categories(@cur NVARCHAR(100))
AS
BEGIN
	WITH c(Id, Title, [Ref]) AS (
	         SELECT dmc.Id               AS Id,
	                dmc.Title,
	                NULL                 AS [Ref]
	         FROM   D_MATERIAL_CATEGORY  AS dmc
	                JOIN DV_MATERIAL     AS dm
	                     ON  dm.CategoryId = dmc.Id
	                JOIN D_APHORISM      AS da
	                     ON  da.ModelCoreType = dm.ModelCoreType
	                     AND da.Id = dm.Id
	         GROUP BY
	                dmc.Id,
	                dmc.Title
	         UNION ALL
	         SELECT daa.TitleUrl       AS Id,
	                daa.Name,
	                CategoryId         AS [Ref]
	         FROM   D_AUTHOR_APHORISM  AS daa
	                JOIN D_APHORISM    AS da
	                     ON  da.AuthorId = daa.Id
	                JOIN DV_MATERIAL   AS dm
	                     ON  dm.Id = da.Id
	                     AND dm.ModelCoreType = da.ModelCoreType
	                     AND dm.CategoryId = CategoryId
	         GROUP BY
	                daa.TitleUrl,
	                daa.Name,
	                CategoryId
	     )
	
	SELECT *
	FROM   c AS c
END
GO

/*******************************************
* ������� ������ ���������
*******************************************/
IF OBJECT_ID(N'dbo.del_author_aphorism', N'P') IS NOT NULL
    DROP PROCEDURE dbo.del_author_aphorism;
GO
CREATE PROCEDURE dbo.del_author_aphorism(@authorId INT)
AS
BEGIN
	UPDATE D_APHORISM
	SET    AuthorId = NULL
	WHERE  AuthorId = @authorId
	
	DELETE 
	FROM   D_AUTHOR_APHORISM
	WHERE  Id = @authorId
END
GO

/*******************************************
 * ���������� ���������
 *******************************************/
IF OBJECT_ID(N'dbo.get_popular_materials', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_popular_materials;
GO
CREATE PROCEDURE dbo.get_popular_materials
	@mid INT,
	@mct INT,
	@amount INT
AS
BEGIN
	SELECT TOP(@amount)
	       dm.DateCreate,
	       dm.DateOfPublication,
	       dm.Title,
	       dm.TitleUrl,
	       dm.ModelCoreType,
	       COUNT(dc.Id)         AS CommentsCount,
	       SUM(dm.LikeUpCount + dm.LikeDownCount) AS LikesCount,
	       SUM(dm.ViewsCount)      ViewsCount
	FROM   DV_MATERIAL          AS dm
	       LEFT JOIN D_COMMENT  AS dc
	            ON  dc.ModelCoreType = dm.ModelCoreType
	            AND dc.MaterialId = dm.Id
	WHERE  dm.ModelCoreType = @mct
	       AND dm.Show = 1
	       AND dm.DateOfPublication <= GETDATE()
	       AND dm.Id NOT IN (@mid)
	GROUP BY
	       dm.IsTop,
	       dm.DateCreate,
	       dm.DateOfPublication,
	       dm.Title,
	       dm.TitleUrl,
	       dm.ModelCoreType
	HAVING SUM(dm.LikeUpCount + dm.LikeDownCount) > 0 OR COUNT(dm.ViewsCount) >
	       0
	ORDER BY
	       dm.IsTop DESC,
	       CommentsCount DESC,
	       LikesCount DESC,
	       ViewsCount DESC
END
GO

/*******************************************
* ������ ���������
*******************************************/
IF OBJECT_ID(N'dbo.get_other_materials', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_other_materials;
GO
CREATE PROCEDURE dbo.get_other_materials
(@mid INT, @mct INT, @dir BIT, @amount INT = 3)
AS
BEGIN
	DECLARE @date DATETIME
	SELECT @date = dm.DateOfPublication
	FROM   DV_MATERIAL AS dm
	WHERE  dm.Id = @mid
	       AND dm.ModelCoreType = @mct
	
	IF (@dir = 1)
	    SELECT TOP(@amount) dm.Id,
	           dm.DateCreate,
	           dm.DateOfPublication,
	           dm.Title,
	           dm.TitleUrl,
	           dm.Foreword,
	           dm.ModelCoreType,
	           dm.UserId,
	           anu.AvatarId,
	           anu.NikName
	    FROM   DV_MATERIAL       AS dm
	           JOIN AspNetUsers  AS anu
	                ON  anu.Id = dm.UserId
	    WHERE  dm.ModelCoreType = @mct
	           AND (
	                   dm.Id IN (@mid)
	                   OR (dm.Id NOT IN (@mid) AND dm.DateOfPublication > @date)
	               )
	    ORDER BY
	           dm.DateOfPublication DESC
	ELSE 
	IF (@dir = 0)
	    SELECT TOP(@amount) dm.Id,
	           dm.DateCreate,
	           dm.DateOfPublication,
	           dm.Title,
	           dm.TitleUrl,
	           dm.Foreword,
	           dm.ModelCoreType,
	           dm.UserId,
	           anu.AvatarId,
	           anu.NikName
	    FROM   DV_MATERIAL       AS dm
	           JOIN AspNetUsers  AS anu
	                ON  anu.Id = dm.UserId
	    WHERE  dm.ModelCoreType = @mct
	           AND (
	                   dm.Id IN (@mid)
	                   OR (dm.Id NOT IN (@mid) AND dm.DateOfPublication < @date)
	               )
	    ORDER BY
	           dm.DateOfPublication DESC
	
	RETURN
END
GO

/*******************************************
 * �������� ������ ���������� �������
 *******************************************/
IF OBJECT_ID(N'dbo.get_banned_urls', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_banned_urls;
GO
CREATE PROCEDURE dbo.get_banned_urls
AS
BEGIN
	SELECT dbu.[Url]
	FROM   D_BANNED_URL AS dbu
END
GO

/*******************************************
 * ������� ���������� �����
 *******************************************/
IF OBJECT_ID(N'dbo.del_banned_url', N'P') IS NOT NULL
    DROP PROCEDURE dbo.del_banned_url;
GO
CREATE PROCEDURE dbo.del_banned_url
	@bannedUrlId INT
AS
BEGIN
	DELETE 
	FROM   D_BANNED_URL
	WHERE  Id = @bannedUrlId
END
GO

/*******************************************
 * �������� ���������� �����
 *******************************************/
IF OBJECT_ID(N'dbo.get_banned_url', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_banned_url;
GO
CREATE PROCEDURE dbo.get_banned_url
	@bannedUrlId INT
AS
BEGIN
	SELECT *
	FROM   D_BANNED_URL AS dbu
	WHERE  dbu.Id = @bannedUrlId
END
GO

/*******************************************
 * �������� ���������� �����
 *******************************************/
IF OBJECT_ID(N'dbo.add_banned_url', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_banned_url;
GO
CREATE PROCEDURE dbo.add_banned_url
	@url NVARCHAR(255),
	@couse NVARCHAR(255)
AS
BEGIN
	IF NOT EXISTS (
	       SELECT dbu.Id
	       FROM   D_BANNED_URL AS dbu
	       WHERE  dbu.[Url] = @url
	   )
	BEGIN
	    DECLARE @date DATETIME = GETDATE()
	    INSERT INTO D_BANNED_URL
	      (
	        [Url],
	        Couse,
	        DateUpdate,
	        DateCreate
	      )
	    VALUES
	      (
	        @url,
	        @couse,
	        @date,
	        @date
	      )
	    
	    DECLARE @id INT
	    SELECT @id = @@identity
	    
	    EXEC dbo.get_banned_url @id
	END
END
GO

/*******************************************
 * �������� ���������� �����
 *******************************************/
IF OBJECT_ID(N'dbo.update_banned_url', N'P') IS NOT NULL
    DROP PROCEDURE dbo.update_banned_url;
GO
CREATE PROCEDURE dbo.update_banned_url
	@id INT,
	@url NVARCHAR(255),
	@couse NVARCHAR(255)
AS
BEGIN
	UPDATE D_BANNED_URL
	SET    [Url] = @url,
	       Couse = @couse,
	       DateUpdate = GETDATE()
	WHERE  Id = @id
	
	EXEC dbo.get_banned_url @id
END
GO

/*******************************************
 * �������� �������� �����
 *******************************************/
IF OBJECT_ID(N'dbo.get_seo_keyword', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_seo_keyword;
GO
CREATE PROCEDURE dbo.get_seo_keyword
	@id INT
AS
BEGIN
	SELECT *
	FROM   D_SEO_KEYWORD AS dsk
	WHERE  dsk.Id = @id
END
GO

/*******************************************
 * �������� �������� �����
 *******************************************/
IF OBJECT_ID(N'dbo.add_seo_keyword', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_seo_keyword;
GO
CREATE PROCEDURE dbo.add_seo_keyword
	@sti INT,
	@value NVARCHAR(50)
AS
BEGIN
	IF NOT EXISTS (
	       SELECT dsk.Id
	       FROM   D_SEO_KEYWORD AS dsk
	       WHERE  dsk.SeoTagsId = @sti
	              AND dsk.[Value] = @value
	   )
	BEGIN
	    DECLARE @date DATETIME = GETDATE()
	    
	    INSERT INTO D_SEO_KEYWORD
	      (
	        [Value],
	        SeoTagsId,
	        DateUpdate,
	        DateCreate
	      )
	    VALUES
	      (
	        @value,
	        @sti,
	        @date,
	        @date
	      )
	    
	    DECLARE @id INT
	    SELECT @id = @@identity
	    
	    EXEC dbo.get_seo_keyword @id
	END
END
GO

/*******************************************
 * ���������������� �������� �� ����
 *******************************************/
IF OBJECT_ID(N'dbo.get_game_by_url', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_game_by_url;
GO
CREATE PROCEDURE dbo.get_game_by_url(@titleUrl VARCHAR(50))
AS
BEGIN
	SELECT *
	FROM   D_GAME AS dg
	WHERE  dg.TitleUrl = @titleUrl
	       AND dg.Show = 1
END
GO

/*******************************************
 * ��������� ��������� �� ����
 *******************************************/
IF OBJECT_ID(N'dbo.get_game_materials', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_game_materials;
GO
CREATE PROCEDURE dbo.get_game_materials
	@titleUrl VARCHAR(50),
	@amount INT
AS
BEGIN
	SELECT x.Id,
	       x.ModelCoreType,
	       x.Title,
	       x.TitleUrl,
	       x.DateCreate,
	       x.DateOfPublication,
	       x.ViewsCount,
	       CASE 
	            WHEN x.ModelCoreType = 6 THEN x.Html
	            ELSE x.Foreword
	       END  AS Foreword,
	       CASE 
	            WHEN x.ModelCoreType = 6 THEN x.PictureId
	            ELSE x.FrontPictureId
	       END  AS FrontPictureId,
	       x.CategoryId,
	       (
	           SELECT TOP(1) dvl.VideoId
	           FROM   D_VIDEO_LINK AS dvl
	           WHERE  dvl.MaterialId = x.Id
	                  AND dvl.ModelCoreType = x.ModelCoreType
	       )    AS TopVideoId,
	       dbo.get_comments_count(x.Id, x.ModelCoreType) AS CommentsCount
	FROM   (
	           SELECT TOP(@amount) dm.*,
	                  NULL              AS PictureId,
	                  dn.GameId
	           FROM   D_NEWS            AS dn
	                  JOIN DV_MATERIAL  AS dm
	                       ON  dm.Id = dn.Id
	                       AND dm.ModelCoreType = dn.ModelCoreType
	                  JOIN D_GAME       AS dg
	                       ON  dg.Id = dn.GameId
	                       AND dg.TitleUrl = @titleUrl
	                       AND dg.Show = 1
	           WHERE  dm.Show = 1
	                  AND dm.DateOfPublication <= GETDATE()
	           ORDER BY
	                  dm.DateOfPublication DESC
	           UNION ALL
	           SELECT TOP(@amount) dm.*,
	                  NULL              AS PictureId,
	                  da.GameId
	           FROM   D_ARTICLE         AS da
	                  JOIN DV_MATERIAL  AS dm
	                       ON  dm.Id = da.Id
	                       AND dm.ModelCoreType = da.ModelCoreType
	                  JOIN D_GAME       AS dg
	                       ON  dg.Id = da.GameId
	                       AND dg.TitleUrl = @titleUrl
	                       AND dg.Show = 1
	           WHERE  dm.Show = 1
	                  AND dm.DateOfPublication <= GETDATE()
	           ORDER BY
	                  dm.DateOfPublication DESC
	           UNION ALL
	           SELECT TOP(@amount) dm.*,
	                  daa.PictureId,
	                  dmc.GameId
	           FROM   D_APHORISM        AS da2
	                  LEFT JOIN D_AUTHOR_APHORISM AS daa
	                       ON  daa.Id = da2.AuthorId
	                  JOIN DV_MATERIAL  AS dm
	                       ON  dm.Id = da2.Id
	                       AND dm.ModelCoreType = da2.ModelCoreType
	                  JOIN D_MATERIAL_CATEGORY AS dmc
	                       ON  dmc.Id = dm.CategoryId
	                  JOIN D_GAME       AS dg
	                       ON  dg.Id = dmc.GameId
	                       AND dg.Show = 1
	                       AND dg.TitleUrl = @titleUrl
	           WHERE  dm.Show = 1
	                  AND dm.DateOfPublication <= GETDATE()
	           ORDER BY
	                  dm.DateOfPublication DESC
	       )    AS x
END
GO

/*******************************************
 * �������� ����� ��� ����
 *******************************************/
IF OBJECT_ID(N'dbo.get_game_videos', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_game_videos;
GO
CREATE PROCEDURE dbo.get_game_videos(@titleUrl VARCHAR(50))
AS
BEGIN
	SELECT dv.*
	FROM   D_VIDEO                AS dv
	       JOIN D_VIDEO_LINK      AS dvl
	            ON  dvl.VideoId = dv.Id
	       LEFT JOIN DV_MATERIAL  AS dm
	            ON  dm.Id = dvl.MaterialId
	            AND dm.ModelCoreType = dvl.ModelCoreType
	            AND dm.Show = 1
	            AND dm.DateOfPublication <= GETDATE()
	       LEFT JOIN D_NEWS       AS dn
	            ON  dn.Id = dm.Id
	            AND dn.ModelCoreType = dm.ModelCoreType
	       LEFT JOIN D_ARTICLE    AS da
	            ON  da.Id = dm.Id
	            AND da.ModelCoreType = dm.ModelCoreType
	       LEFT JOIN D_GAME       AS dg
	            ON  (dg.Id = dn.GameId OR dg.Id = da.GameId)
	            AND dg.Show = 1
	WHERE  (dn.GameId IS NOT NULL OR da.GameId IS NOT NULL)
	       AND dg.TitleUrl = @titleUrl
END
GO

/*******************************************
 * �������� ����������
 *******************************************/
IF OBJECT_ID(N'dbo.add_employee', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_employee;
GO
CREATE PROCEDURE dbo.add_employee(@uid VARCHAR(128))
AS
BEGIN
	INSERT INTO D_EMPLOYEE
	  (
	    Id,
	    DateCreate
	  )
	VALUES
	  (
	    @uid,
	    GETDATE()
	  )
END
GO

/*******************************************
 * ������� ����������
 *******************************************/
IF OBJECT_ID(N'dbo.del_employee', N'P') IS NOT NULL
    DROP PROCEDURE dbo.del_employee;
GO
CREATE PROCEDURE dbo.del_employee(@uid VARCHAR(128))
AS
BEGIN
	DELETE 
	FROM   D_EMPLOYEE
	WHERE  Id = @uid
END
GO

/*******************************************
 * ����������
 *******************************************/
IF OBJECT_ID(N'dbo.get_employees', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_employees;
GO
CREATE PROCEDURE dbo.get_employees(@id VARCHAR(128))
AS
BEGIN
	SELECT *
	FROM   D_EMPLOYEE        AS de
	       JOIN AspNetUsers  AS anu
	            ON  anu.Id = de.Id
	WHERE  (@id IS NULL OR de.Id = @id)
END
GO

/*******************************************
 * ��������� ���������� ����������
 *******************************************/
IF OBJECT_ID(N'dbo.add_material_view', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_material_view;
GO
CREATE PROCEDURE dbo.add_material_view(@mid INT, @mct INT)
AS
BEGIN
	DECLARE @oldCount INT
	SELECT @oldCount = dm.ViewsCount
	FROM   DV_MATERIAL AS dm
	WHERE  dm.Id = @mid
	       AND dm.ModelCoreType = @mct
	
	UPDATE DV_MATERIAL
	SET    ViewsCount            = @oldCount + 1
	WHERE  Id                    = @mid
	       AND ModelCoreType     = @mct
END
GO

/*******************************************
 * �������
 *******************************************/
IF OBJECT_ID(N'dbo.get_banners', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_banners;
GO
CREATE PROCEDURE dbo.get_banners(@id UNIQUEIDENTIFIER, @place INT)
AS
BEGIN
	SELECT db.*,
	       dp.Id,
	       dp.Caption
	FROM   D_BANNER             AS db
	       LEFT JOIN D_PICTURE  AS dp
	            ON  dp.Id = db.PictureId
	WHERE  (@id IS NULL OR db.Id = @id)
	       AND (@place IS NULL OR db.Place = @place)
END
GO

/*******************************************
 * �������� ������
 *******************************************/
IF OBJECT_ID(N'dbo.update_banner', N'P') IS NOT NULL
    DROP PROCEDURE dbo.update_banner;
GO
CREATE PROCEDURE dbo.update_banner(
    @id         UNIQUEIDENTIFIER,
    @url        VARCHAR(255),
    @pid        UNIQUEIDENTIFIER,
    @title      NVARCHAR(100),
    @place      INT,
    @rawUrl     NVARCHAR(255),
    @desc       NVARCHAR(MAX)
)
AS
BEGIN
	UPDATE D_BANNER
	SET    Title             = @title,
	       PictureId         = @pid,
	       [Url]             = @url,
	       DateUpdate        = GETDATE(),
	       RawUrl            = @rawUrl,
	       Place             = @place,
	       [Description]     = @desc
	WHERE  Id                = @id
END
GO

/*******************************************
 * ������� ������
 *******************************************/
IF OBJECT_ID(N'dbo.del_banner', N'P') IS NOT NULL
    DROP PROCEDURE dbo.del_banner;
GO
CREATE PROCEDURE dbo.del_banner
	@bannerId UNIQUEIDENTIFIER
AS
BEGIN
	DELETE 
	FROM   D_BANNER
	WHERE  Id = @bannerId
END
GO

/*******************************************
 * �������� ���������� ������ �������
 *******************************************/
IF OBJECT_ID(N'dbo.add_banner_clicks_count', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_banner_clicks_count;
GO
CREATE PROCEDURE dbo.add_banner_clicks_count
	@bId UNIQUEIDENTIFIER,
	@alIds NVARCHAR(MAX) = NULL
AS
BEGIN
	UPDATE D_BANNER
	SET    ClicksCount = db.ClicksCount + 1,
	       DateUpdate = GETDATE()
	FROM   D_BANNER AS db
	WHERE  db.Id = @bId
	
	IF (@alIds IS NOT NULL)
	BEGIN
	    DECLARE @q NVARCHAR(MAX)
	    SET @q = 
	        '
	             DECLARE @affiliateLinkId UNIQUEIDENTIFIER
		DECLARE c CURSOR LOCAL FORWARD_ONLY FAST_FORWARD 
		FOR
		    SELECT dal.Id
		    FROM   D_AFFILIATE_LINK AS dal
		    WHERE  dal.Id IN (' + @alIds +
	        ')
		
		OPEN c
		FETCH NEXT FROM c INTO @affiliateLinkId
		WHILE @@FETCH_STATUS = 0
		BEGIN
		    IF NOT EXISTS (
		           SELECT TOP 1 dabv.BannerId
		           FROM   D_AFFILIATE_BANNER_VIEW AS dabv
		           WHERE  dabv.BannerId = ''' + CAST(@bId AS NVARCHAR(128)) +
	        '''
		                  AND dabv.AffiliatelinkId = @affiliateLinkId
		    )
		    BEGIN
		    	INSERT INTO D_AFFILIATE_BANNER_VIEW
		          (
		            BannerId,
		            AffiliatelinkId,
		            DateCreate
		          )
		        VALUES
		          (
		            ''' + CAST(@bId AS NVARCHAR(128)) +
	        ''',
		            @affiliateLinkId,
		            GETDATE()
		          )
		    END
		    FETCH NEXT FROM c INTO @affiliateLinkId
		END
		CLOSE c
		DEALLOCATE c'
	    
	    EXEC (@q)
	END
END
GO

/*******************************************
 * �������� ���������� ������� ������ ��������
 *******************************************/
IF OBJECT_ID(N'dbo.add_banners_shows_count', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_banners_shows_count;
GO
CREATE PROCEDURE dbo.add_banners_shows_count
	@keys VARCHAR(MAX)
AS
BEGIN
	EXEC (
	         'UPDATE D_BANNER
	SET
		ShowsCount = db.ShowsCount+1,
		DateUpdate=GETDATE()
	FROM D_BANNER AS db WHERE db.Id IN (' + @keys + ')'
	     )
END
GO

/*******************************************
 * ������ ���� �������� �����
 *******************************************/
IF OBJECT_ID(N'dbo.get_site_settings', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_site_settings;
GO
CREATE PROCEDURE dbo.get_site_settings
AS
BEGIN
	SELECT *
	FROM   D_SITE_SETTING AS dss
END
GO

/*******************************************
 * ������ �������� ����� �� ������
 *******************************************/
IF OBJECT_ID(N'dbo.get_site_settings_by_keys', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_site_settings_by_keys;
GO
CREATE PROCEDURE dbo.get_site_settings_by_keys
	@keys NVARCHAR(MAX)
AS
BEGIN
	EXEC (
	         'SELECT*FROM D_SITE_SETTING AS dss WHERE dss.Id IN (' + @keys + ')'
	     )
END
GO

/*******************************************
 * ���� ��������� ������
 *******************************************/
IF OBJECT_ID(N'dbo.get_random_site_tests', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_random_site_tests;
GO
CREATE PROCEDURE dbo.get_random_site_tests
	@amount INT
AS
BEGIN
	SELECT TOP(@amount)
	       dst.Title,
	       dst.[Description],
	       dst.TitleUrl,
	       dst.Rules
	FROM   D_SITE_TEST AS dst
	WHERE  dst.Show = 1
	ORDER BY
	       NEWID()
END
GO

/*******************************************
 * �������� �������� �����
 *******************************************/
IF OBJECT_ID(N'dbo.add_site_test_show', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_site_test_show;
GO
CREATE PROCEDURE dbo.add_site_test_show
	@testId INT
AS
BEGIN
	DECLARE @oldViewsCount INT
	SELECT TOP 1 @oldViewsCount = dst.ViewsCount
	FROM   D_SITE_TEST AS dst
	WHERE  dst.Id = @testId
	
	UPDATE D_SITE_TEST
	SET    ViewsCount = @oldViewsCount + 1
	WHERE  Id = @testId
	
	SELECT @oldViewsCount + 1
END
GO

/*******************************************
 * ������� ������� ��� ������
 *******************************************/
IF OBJECT_ID(N'dbo.get_site_test_matrix', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_site_test_matrix;
GO
CREATE PROCEDURE dbo.get_site_test_matrix
	@testId INT,
	@page INT = 1,
	@pageSize INT = 10,
	@count INT OUTPUT
AS
BEGIN
	DECLARE @x         NVARCHAR(MAX) = '',
	        @title     NVARCHAR(400)
	
	SET @page = (@page -1) * @pageSize
	
	DECLARE c CURSOR  
	FOR
	    SELECT dsts.Title
	    FROM   D_SITE_TEST_SUBJECT AS dsts
	    WHERE  dsts.TestId = @testId
	    GROUP BY
	           dsts.Title
	
	OPEN c
	FETCH NEXT FROM c INTO @title
	WHILE @@FETCH_STATUS = 0
	BEGIN
	    SET @x = @x + ',[' + @title + ']'
	    FETCH NEXT FROM c INTO @title
	END
	CLOSE c
	DEALLOCATE c
	
	IF (@x <> '')
	BEGIN
	    SET @x = RIGHT(@x, LEN(@x) -1)
	    
	    EXEC (
	             'SELECT *
FROM   (
           SELECT dsts.Title,
                  dstq.[Text],
                  dsta.IsCorrect
           FROM   D_SITE_TEST_SUBJECT AS dsts
                  JOIN D_SITE_TEST_QUESTION AS dstq
                       ON  dstq.TestId = ' + @testId +
	             '
                  LEFT JOIN D_SITE_TEST_ANSWER AS dsta
                       ON  dsta.SubjectId = dsts.Id
                       AND dsta.QuestionId = dstq.Id
           WHERE  dsts.TestId = ' + @testId +
	             '
       ) t
       PIVOT(
           SUM(IsCorrect) FOR t.Title IN (' + @x +
	             ')
       ) p ORDER BY p.[Text] OFFSET ' + @page + ' ROWS FETCH NEXT ' + @pageSize 
	             +
	             ' ROWS ONLY'
	         )
	END
	
	SELECT @count = COUNT(1)
	FROM   D_SITE_TEST_QUESTION AS dstq
	WHERE  dstq.TestId = @testId
	
	RETURN
END
GO

/*******************************************
 * ����� �������� ������� ����� 
 *******************************************/
IF OBJECT_ID(N'dbo.revert_site_test_matrix_value', N'P') IS NOT NULL
    DROP PROCEDURE dbo.revert_site_test_matrix_value;
GO
CREATE PROCEDURE dbo.revert_site_test_matrix_value
	@subjectTitle NVARCHAR(400),
	@questionText NVARCHAR(500),
	@value INT
AS
BEGIN
	UPDATE D_SITE_TEST_ANSWER
	SET    IsCorrect = @value
	WHERE  Id IN (SELECT dsta.Id
	              FROM   D_SITE_TEST_ANSWER AS dsta
	                     JOIN D_SITE_TEST_QUESTION AS dstq
	                          ON  dstq.Id = dsta.QuestionId
	                          AND dstq.[Text] = @questionText
	                     JOIN D_SITE_TEST_SUBJECT AS dsts
	                          ON  dsts.Id = dsta.SubjectId
	                          AND dsts.Title = @subjectTitle)
END
GO

/*******************************************
 * �������� �������� ��������
 *******************************************/
IF OBJECT_ID(N'dbo.get_page_redirect', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_page_redirect;
GO
CREATE PROCEDURE dbo.get_page_redirect
	@rawUrl VARCHAR(255)
AS
BEGIN
	SELECT TOP(1) *
	FROM   D_REDIRECT AS dr
	WHERE  dr.OldUrl = @rawUrl
END
GO

/*******************************************
 * �������� �������� ��������
 *******************************************/
IF OBJECT_ID(N'dbo.add_redirect', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_redirect;
GO
CREATE PROCEDURE dbo.add_redirect
	@oldUrl VARCHAR(255),
	@newUrl VARCHAR(255)
AS
	BEGIN TRANSACTION
	DECLARE @date DATETIME = GETDATE();
	
	IF NOT EXISTS (
	       SELECT TOP 1 dr.Id
	       FROM   D_REDIRECT AS dr
	       WHERE  dr.OldUrl = @oldUrl
	   )
	BEGIN
	    UPDATE D_REDIRECT
	    SET    NewUrl         = @newUrl,
	           DateUpdate     = @date
	    WHERE  NewUrl         = @oldUrl;
	    
	    DECLARE @newId UNIQUEIDENTIFIER = NEWID();
	    
	    INSERT INTO D_REDIRECT
	      (
	        Id,
	        OldUrl,
	        NewUrl,
	        DateUpdate,
	        DateCreate
	      )
	    VALUES
	      (
	        @newId,
	        @oldUrl,
	        @newUrl,
	        @date,
	        @date
	      );
	    
	    SELECT TOP(1) *
	    FROM   D_REDIRECT AS dr
	    WHERE  dr.Id = @newId;
	END
	
	COMMIT TRANSACTION
GO

/*******************************************
 * �������� �������� ��������
 *******************************************/
IF OBJECT_ID(N'dbo.update_redirect', N'P') IS NOT NULL
    DROP PROCEDURE dbo.update_redirect;
GO
CREATE PROCEDURE dbo.update_redirect
	@redirectId UNIQUEIDENTIFIER,
	@oldUrl VARCHAR(255),
	@newUrl VARCHAR(255)
AS
BEGIN
	DECLARE @date DATETIME = GETDATE();
	
	UPDATE D_REDIRECT
	SET    OldUrl = @oldUrl,
	       NewUrl = @newUrl,
	       DateUpdate = @date
	WHERE  Id = @redirectId
	
	SELECT TOP(1) *
	FROM   D_REDIRECT AS dr
	WHERE  dr.Id = @redirectId;
END
GO

/*******************************************
 * ������� ��������
 *******************************************/
IF OBJECT_ID(N'dbo.del_redirect', N'P') IS NOT NULL
    DROP PROCEDURE dbo.del_redirect;
GO
CREATE PROCEDURE dbo.del_redirect
	@redirectId UNIQUEIDENTIFIER
AS
BEGIN
	DELETE 
	FROM   D_REDIRECT
	WHERE  Id = @redirectId
END
GO

/*******************************************
 * ��� seo info �����
 *******************************************/
IF OBJECT_ID(N'dbo.get_all_seo_info', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_all_seo_info;
GO
CREATE PROCEDURE dbo.get_all_seo_info
AS
BEGIN
	SELECT *
	FROM   D_SEO_TAGS AS dst
	ORDER BY
	       dst.RawUrl
END
GO

/*******************************************
 * Seo ���� ��������
 *******************************************/
IF OBJECT_ID(N'dbo.get_page_seo_info', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_page_seo_info;
GO
CREATE PROCEDURE dbo.get_page_seo_info
	@url VARCHAR(255)
AS
BEGIN
	SELECT TOP(1) *
	FROM   D_SEO_TAGS AS dsi
	WHERE  dsi.RawUrl = @url
END
GO

/*******************************************
 *�������� Seo ���� ��������
 *******************************************/
IF OBJECT_ID(N'dbo.update_material_seo_tags', N'P') IS NOT NULL
    DROP PROCEDURE dbo.update_material_seo_tags;
GO
CREATE PROCEDURE dbo.update_material_seo_tags
	@mid INT,
	@mct INT,
	@stid INT
AS
BEGIN
	UPDATE DV_MATERIAL
	SET    SeoTagsId             = @stid
	WHERE  Id                    = @mid
	       AND ModelCoreType     = @mct
END
GO

/*******************************************
 * ������� seo-���� ���������
 *******************************************/
IF OBJECT_ID(N'dbo.del_material_seo_tags', N'P') IS NOT NULL
    DROP PROCEDURE dbo.del_material_seo_tags;
GO
CREATE PROCEDURE dbo.del_material_seo_tags
	@mid INT,
	@mct INT
AS
	BEGIN TRANSACTION
	EXEC dbo.update_material_seo_tags @mid,
	     @mct,
	     NULL
	
	DELETE 
	FROM   D_SEO_TAGS
	WHERE  Id IN (SELECT dsi.Id
	              FROM   D_SEO_TAGS AS dsi
	              WHERE  MaterialId = @mid
	                     AND ModelCoreType = @mct)
	
	COMMIT TRANSACTION
GO

/*******************************************
 * ������� seo-���
 *******************************************/
IF OBJECT_ID(N'dbo.del_seo_tags', N'P') IS NOT NULL
    DROP PROCEDURE dbo.del_seo_tags;
GO
CREATE PROCEDURE dbo.del_seo_tags
	@seoTagsId INT
AS
	DELETE 
	FROM   D_SEO_TAGS
	WHERE  Id = @seoTagsId
GO

/*******************************************
 * Seo info ���������
 *******************************************/
IF OBJECT_ID(N'dbo.get_material_seo_info', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_material_seo_info;
GO
CREATE PROCEDURE dbo.get_material_seo_info
	@mid INT,
	@mct INT
AS
BEGIN
	SELECT TOP(1) *
	FROM   D_SEO_TAGS AS dst
	WHERE  (dst.MaterialId = @mid AND dst.ModelCoreType = @mct)
END
GO

/*******************************************
 * Seo info keywords
 *******************************************/
IF OBJECT_ID(N'dbo.get_page_seo_info_keywords', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_page_seo_info_keywords;
GO
CREATE PROCEDURE dbo.get_page_seo_info_keywords
	@seoTagsId INT
AS
BEGIN
	SELECT *
	FROM   D_SEO_KEYWORD AS dsk
	WHERE  dsk.SeoTagsId = @seoTagsId
END
GO

/*******************************************
 * get_users_by_emails
 *******************************************/
IF OBJECT_ID(N'dbo.get_users_by_emails', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_users_by_emails;
GO
CREATE PROCEDURE dbo.get_users_by_emails
	@emails VARCHAR(MAX)
AS
BEGIN
	EXEC (
	         'SELECT*FROM AspNetUsers AS anu WHERE anu.Email IN (' + @emails +
	         ')'
	     )
END
GO

/*******************************************
 * �������� ��������
 *******************************************/
IF OBJECT_ID(N'dbo.add_picture', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_picture;
GO
CREATE PROCEDURE dbo.add_picture
	@format NVARCHAR(50),
	@content VARBINARY(MAX),
	@width INT,
	@height INT,
	@caption NVARCHAR(100),
	@desc NVARCHAR(255),
	@size INT
AS
BEGIN
	DECLARE @id       UNIQUEIDENTIFIER = NEWID(),
	        @date     DATETIME = GETDATE();
	
	INSERT INTO D_PICTURE
	  (
	    Id,
	    ImgFormat,
	    OriginalContent,
	    Width,
	    Height,
	    Caption,
	    [Description],
	    [Size],
	    DateUpdate,
	    DateCreate
	  )
	VALUES
	  (
	    @id,
	    @format,
	    @content,
	    @width,
	    @height,
	    @caption,
	    @desc,
	    @size,
	    @date,
	    @date
	  )
	
	EXEC dbo.get_picture @id
END
GO

/*******************************************
 * �������� ��������
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
	
	--!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
	--UPDATE D_AUTHOR_APHORISM
	--SET    PictureId = NULL
	--WHERE  PictureId = @pictureId
	
	--UPDATE D_SITE_TEST_SUBJECT
	--SET    PictureId = NULL
	--WHERE  PictureId = @pictureId
	
	DELETE 
	FROM   D_PICTURE
	WHERE  Id = @pictureId
	
	COMMIT TRANSACTION
GO

/*******************************************
 * ������� �������������� ��������
 *******************************************/
IF OBJECT_ID(N'dbo.del_free_pictures', N'P') IS NOT NULL
    DROP PROCEDURE dbo.del_free_pictures;
GO
CREATE PROCEDURE dbo.del_free_pictures
	@ids NVARCHAR(MAX)
AS
BEGIN
	EXEC (
	         'DECLARE @size FLOAT=0;
	         SELECT @size=SUM(dp.[Size]) FROM D_PICTURE AS dp WHERE Id IN (' + @ids 
	         + ');  DELETE FROM D_PICTURE WHERE Id IN (' + @ids +
	         '); SELECT @size'
	     );
END
GO

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
	
	SELECT TOP 1 *
	FROM   D_REQUEST AS dr
	WHERE  dr.Id = @id
END
GO

/*******************************************
 * add site test
 *******************************************/
IF OBJECT_ID(N'dbo.add_site_test', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_site_test;
GO
CREATE PROCEDURE dbo.add_site_test
	@title NVARCHAR(200),
	@desc NVARCHAR(1000),
	@rules NVARCHAR(MAX),
	@titleUrl VARCHAR(255),
	@type INT
AS
BEGIN
	IF NOT EXISTS (
	       SELECT TOP(1) *
	       FROM   D_SITE_TEST AS dst
	       WHERE  dst.TitleUrl = @titleUrl
	   )
	BEGIN
	    INSERT INTO D_SITE_TEST
	      (
	        Title,
	        [Description],
	        Rules,
	        DateUpdate,
	        DateCreate,
	        TitleUrl,
	        [Type]
	      )
	    VALUES
	      (
	        @title,
	        @desc,
	        @rules,
	        GETDATE(),
	        GETDATE(),
	        @titleUrl,
	        @type
	      )
	    
	    DECLARE @id INT
	    SET @id = @@identity
	    SELECT TOP(1) *
	    FROM   D_SITE_TEST AS dst
	    WHERE  dst.Id = @id
	END
END
GO	

/*******************************************
 * ������� �����
 *******************************************/
IF OBJECT_ID(N'dbo.get_site_test_rules', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_site_test_rules;
GO
CREATE PROCEDURE dbo.get_site_test_rules
	@testId INT
AS
BEGIN
	SELECT TOP 1 dst.Title,
	       dst.Rules
	FROM   D_SITE_TEST AS dst
	WHERE  dst.Id = @testId
END
GO
 
/*******************************************
 * delete site test
 *******************************************/
IF OBJECT_ID(N'dbo.del_site_test', N'P') IS NOT NULL
    DROP PROCEDURE dbo.del_site_test;
GO
CREATE PROCEDURE dbo.del_site_test
	@testId INT
AS
	BEGIN TRANSACTION
	
	DELETE 
	FROM   D_SITE_TEST_ANSWER
	WHERE  SubjectId IN (SELECT dsts.Id
	                     FROM   D_SITE_TEST_SUBJECT AS dsts
	                     WHERE  dsts.TestId = @testId)
	
	DELETE 
	FROM   D_SITE_TEST
	WHERE  Id = @testId
	
	COMMIT TRANSACTION
GO

 /*******************************************
 * add site test question
 *******************************************/
IF OBJECT_ID(N'dbo.add_site_test_question', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_site_test_question;
GO
CREATE PROCEDURE dbo.add_site_test_question
	@testId INT,
	@text NVARCHAR(500)
AS
BEGIN
	IF NOT EXISTS (
	       SELECT TOP(1) *
	       FROM   D_SITE_TEST_QUESTION AS dstq
	       WHERE  dstq.TestId = @testId
	              AND dstq.[Text] = @text
	   )
	BEGIN
	    INSERT INTO D_SITE_TEST_QUESTION
	      (
	        TestId,
	        [Text],
	        DateUpdate,
	        DateCreate
	      )
	    VALUES
	      (
	        @testId,
	        @text,
	        GETDATE(),
	        GETDATE()
	      )
	    
	    DECLARE @id INT
	    SET @id = @@identity
	    
	    INSERT INTO D_SITE_TEST_ANSWER
	      (
	        QuestionId,
	        SubjectId,
	        DateUpdate,
	        DateCreate,
	        IsCorrect
	      )
	    SELECT @id,
	           dsts.Id,
	           GETDATE(),
	           GETDATE(),
	           0
	    FROM   D_SITE_TEST_SUBJECT AS dsts
	    WHERE  dsts.TestId = @testId
	    
	    SELECT TOP(1) *
	    FROM   D_SITE_TEST_QUESTION AS dstq
	    WHERE  dstq.Id = @id
	END
END
GO

 /*******************************************
 * add site test subject
 *******************************************/
IF OBJECT_ID(N'dbo.add_site_test_subject', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_site_test_subject;
GO
CREATE PROCEDURE dbo.add_site_test_subject
	@testId INT,
	@title NVARCHAR(400),
	@desc NVARCHAR(MAX),
	@pictureId UNIQUEIDENTIFIER
AS
BEGIN
	IF NOT EXISTS (
	       SELECT TOP(1) *
	       FROM   D_SITE_TEST_SUBJECT AS dsts
	       WHERE  dsts.TestId = @testId
	              AND dsts.Title = @title
	   )
	BEGIN
	    INSERT INTO D_SITE_TEST_SUBJECT
	      (
	        Title,
	        [Description],
	        TestId,
	        DateUpdate,
	        DateCreate,
	        PictureId
	      )
	    VALUES
	      (
	        @title,
	        @desc,
	        @testId,
	        GETDATE(),
	        GETDATE(),
	        @pictureId
	      )
	    
	    DECLARE @id INT
	    SET @id = @@identity
	    
	    INSERT INTO D_SITE_TEST_ANSWER
	      (
	        QuestionId,
	        SubjectId,
	        DateUpdate,
	        DateCreate,
	        IsCorrect
	      )
	    SELECT dstq.Id,
	           @id,
	           GETDATE(),
	           GETDATE(),
	           0
	    FROM   D_SITE_TEST_QUESTION AS dstq
	    WHERE  dstq.TestId = @testId
	    
	    SELECT TOP(1) *
	    FROM   D_SITE_TEST_SUBJECT AS dsts
	    WHERE  dsts.Id = @id
	END
END
GO

 /*******************************************
 * delete site test question
 *******************************************/
IF OBJECT_ID(N'dbo.del_site_test_question', N'P') IS NOT NULL
    DROP PROCEDURE dbo.del_site_test_question;
GO
CREATE PROCEDURE dbo.del_site_test_question
	@questionId INT
AS
	BEGIN TRANSACTION
	
	DELETE 
	FROM   D_SITE_TEST_ANSWER
	WHERE  QuestionId = @questionId
	
	DELETE 
	FROM   D_SITE_TEST_QUESTION
	WHERE  Id = @questionId
	
	COMMIT TRANSACTION
GO

 /*******************************************
 * delete site test subject
 *******************************************/
IF OBJECT_ID(N'dbo.del_site_test_subject', N'P') IS NOT NULL
    DROP PROCEDURE dbo.del_site_test_subject;
GO
CREATE PROCEDURE dbo.del_site_test_subject
	@subjectId INT
AS
	BEGIN TRANSACTION
	
	DELETE 
	FROM   D_SITE_TEST_ANSWER
	WHERE  SubjectId = @subjectId
	
	DELETE 
	FROM   D_SITE_TEST_SUBJECT
	WHERE  Id = @subjectId
	
	COMMIT TRANSACTION
GO

/*******************************************
 * �������� ����������� �����
 *******************************************/
IF OBJECT_ID(N'dbo.get_site_test_page', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_site_test_page;
GO
CREATE PROCEDURE dbo.get_site_test_page
	@titleUrl VARCHAR(255)
AS
BEGIN
	SELECT TOP(1) *
	FROM   D_SITE_TEST_ANSWER         AS dsta
	       JOIN D_SITE_TEST_QUESTION  AS dstq
	            ON  dstq.Id = dsta.QuestionId
	       JOIN D_SITE_TEST_SUBJECT   AS dsts
	            ON  dsts.Id = dsta.SubjectId
	       JOIN D_SITE_TEST           AS dst
	            ON  dst.Id = dstq.TestId
	            AND dst.TitleUrl = @titleUrl
	            AND dst.Show = 1
	ORDER BY
	       NEWID()
END
GO

/*******************************************
 * ��� ��� ���������� ������� �����
 *******************************************/
IF OBJECT_ID(N'dbo.get_site_test_next_guess_step', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_site_test_next_guess_step;
GO

IF TYPE_ID(N'dbo.OldSiteTestStepGuess') IS NOT NULL
    DROP TYPE dbo.OldSiteTestStepGuess
GO

CREATE TYPE dbo.OldSiteTestStepGuess AS TABLE 
(QuestionId INT, IsCorrect BIT, [Order] INT);  
GO  

/*******************************************
 * ��������� ������ ����� Guess
 *******************************************/
CREATE PROCEDURE dbo.get_site_test_next_guess_step
	@oldSteps dbo.OldSiteTestStepGuess READONLY,
	@subjectsCount INT OUTPUT
AS
BEGIN
	DECLARE @subjects TABLE (SubjectId INT)
	DECLARE @questions TABLE (QuestionId INT)
	
	BEGIN
		DECLARE @questionId     INT,
		        @isCorrect      INT
		
		DECLARE c CURSOR FORWARD_ONLY LOCAL READ_ONLY 
		FOR
		    SELECT os.QuestionId,
		           os.IsCorrect
		    FROM   @oldSteps AS os
		    ORDER BY
		           os.[Order]
		
		OPEN c
		FETCH NEXT FROM c INTO @questionId, @isCorrect
		WHILE @@FETCH_STATUS = 0
		BEGIN
		    INSERT INTO @subjects
		    SELECT DISTINCT dsta.SubjectId
		    FROM   D_SITE_TEST_ANSWER AS dsta
		    WHERE  dsta.QuestionId = @questionId
		           AND dsta.IsCorrect = @isCorrect
		           AND (
		                   dsta.SubjectId IN (SELECT s.SubjectId
		                                      FROM   @subjects AS s)
		                   OR (
		                          SELECT COUNT(1)
		                          FROM   @subjects
		                      ) = 0
		               )
		    
		    DELETE 
		    FROM   @subjects
		    WHERE  SubjectId IN (SELECT DISTINCT dsta.SubjectId
		                         FROM   D_SITE_TEST_ANSWER AS dsta
		                         WHERE  dsta.QuestionId = @questionId
		                                AND dsta.IsCorrect <> @isCorrect)
		    
		    FETCH NEXT FROM c INTO @questionId, @isCorrect
		END
		CLOSE c
		DEALLOCATE c
	END
	
	INSERT INTO @questions
	SELECT dsta.QuestionId
	FROM   D_SITE_TEST_ANSWER  AS dsta
	       JOIN @subjects      AS s
	            ON  s.SubjectId = dsta.SubjectId
	WHERE  dsta.QuestionId NOT IN (SELECT os.QuestionId
	                               FROM   @oldSteps AS os)
	       AND dsta.IsCorrect = 1
	
	SELECT @subjectsCount = COUNT(DISTINCT s.SubjectId)
	FROM   @subjects AS s
	
	IF (@subjectsCount > 1)
	    SELECT TOP(1) *
	    FROM   D_SITE_TEST_ANSWER         AS dsta
	           JOIN @questions            AS q
	                ON  q.QuestionId = dsta.QuestionId
	           JOIN D_SITE_TEST_QUESTION  AS dstq
	                ON  dstq.Id = dsta.QuestionId
	           JOIN D_SITE_TEST_SUBJECT   AS dsts
	                ON  dsts.Id = dsta.SubjectId
	           JOIN D_SITE_TEST           AS dst
	                ON  dst.Id = dstq.TestId
	ELSE
	    SELECT TOP(1) *
	    FROM   D_SITE_TEST_ANSWER         AS dsta
	           JOIN D_SITE_TEST_QUESTION  AS dstq
	                ON  dstq.Id = dsta.QuestionId
	           JOIN D_SITE_TEST_SUBJECT   AS dsts
	                ON  dsts.Id = dsta.SubjectId
	                AND dsts.Id IN (SELECT s.SubjectId
	                                FROM   @subjects AS s)
	           JOIN D_SITE_TEST           AS dst
	                ON  dst.Id = dstq.TestId
END
GO

/*******************************************
 * ��� ��� ���������� ������� �����
 *******************************************/
IF OBJECT_ID(N'dbo.get_site_test_next_normal_step', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_site_test_next_normal_step;
GO

IF OBJECT_ID(N'dbo.get_site_test_normal_results', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_site_test_normal_results;
GO

IF TYPE_ID(N'dbo.OldSiteTestStepNormal') IS NOT NULL
    DROP TYPE dbo.OldSiteTestStepNormal
GO

CREATE TYPE dbo.OldSiteTestStepNormal AS TABLE 
(QuestionId INT, SubjectId INT);  
GO 

/*******************************************
 * ��������� ������ ����� Normal
 *******************************************/
CREATE PROCEDURE dbo.get_site_test_next_normal_step
	@oldSteps dbo.OldSiteTestStepNormal READONLY,
	@subjectsCount INT OUTPUT,
	@allSubjectsCount INT OUTPUT
AS
BEGIN
	DECLARE @testId INT
	SELECT TOP(1) @testId = dsts.TestId
	FROM   D_SITE_TEST_SUBJECT  AS dsts
	       JOIN @oldSteps       AS os
	            ON  os.SubjectId = dsts.Id
	
	SELECT @allSubjectsCount = COUNT(DISTINCT dsts.Id)
	FROM   D_SITE_TEST_SUBJECT AS dsts
	WHERE  dsts.TestId = @testId
	
	SELECT @subjectsCount = COUNT(DISTINCT dsts.Id)
	FROM   D_SITE_TEST_SUBJECT AS dsts
	WHERE  dsts.Id NOT IN (SELECT os.SubjectId
	                       FROM   @oldSteps AS os)
	       AND dsts.TestId = @testId
	
	IF (@subjectsCount > 0)
	    SELECT TOP 1 *
	    FROM   D_SITE_TEST_ANSWER         AS dsta
	           JOIN D_SITE_TEST_QUESTION  AS dstq
	                ON  dstq.Id = dsta.QuestionId
	           JOIN D_SITE_TEST_SUBJECT   AS dsts
	                ON  dsts.Id = dsta.SubjectId
	           JOIN D_SITE_TEST           AS dst
	                ON  dst.Id = dstq.TestId
	                AND dst.Show = 1
	                AND dst.Id = @testId
	    WHERE  dsts.Id NOT IN (SELECT os.SubjectId
	                           FROM   @oldSteps AS os)
	ELSE
	    SELECT TOP 1 *
	    FROM   D_SITE_TEST_ANSWER         AS dsta
	           JOIN D_SITE_TEST_QUESTION  AS dstq
	                ON  dstq.Id = dsta.QuestionId
	           JOIN D_SITE_TEST_SUBJECT   AS dsts
	                ON  dsts.Id = dsta.SubjectId
	           JOIN D_SITE_TEST           AS dst
	                ON  dst.Id = dstq.TestId
	                AND dst.Show = 1
	                AND dst.Id = @testId
	    ORDER BY
	           NEWID()
END
GO

/*******************************************
 * ������� � ������� ����� normal
 *******************************************/
IF OBJECT_ID(N'dbo.get_site_test_normal_questions', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_site_test_normal_questions;
GO
CREATE PROCEDURE dbo.get_site_test_normal_questions
	@testId INT,
	@subjectId INT,
	@amount INT = 3
AS
BEGIN
	DECLARE @trueQuestionId INT
	SELECT @trueQuestionId = dsta.QuestionId
	FROM   D_SITE_TEST_ANSWER AS dsta
	WHERE  dsta.SubjectId = @subjectId
	       AND dsta.IsCorrect = 1
	
	SELECT NEWID(),
	       x.*
	FROM   (
	           SELECT TOP(@amount) dstq.*
	           FROM   D_SITE_TEST_QUESTION AS dstq
	                  JOIN D_SITE_TEST AS dst
	                       ON  dst.Id = dstq.TestId
	                       AND dst.Id = @testId
	                       AND dst.Show = 1
	           WHERE  dstq.Id NOT IN (SELECT TOP 1 dstq.Id
	                                  FROM   D_SITE_TEST_QUESTION AS dstq
	                                  WHERE  dstq.Id = @trueQuestionId)
	           ORDER BY
	                  NEWID()
	           UNION ALL
	           SELECT TOP 1 *
	           FROM   D_SITE_TEST_QUESTION AS dstq
	           WHERE  dstq.Id = @trueQuestionId
	       ) x
	ORDER BY
	       1
END
GO

/*******************************************
 * ���������� ����� Normal
 *******************************************/
--CREATE PROCEDURE dbo.get_site_test_normal_results
--	@oldSteps dbo.OldSiteTestStepNormal READONLY
--AS
--BEGIN
--	SELECT *
--	FROM   D_SITE_TEST_ANSWER         AS dsta
--	       JOIN D_SITE_TEST_QUESTION  AS dstq
--	            ON  dstq.Id = dsta.QuestionId
--	       JOIN D_SITE_TEST_SUBJECT   AS dsts
--	            ON  dsts.Id = dsta.SubjectId
--	       JOIN D_SITE_TEST           AS dst
--	            ON  dst.Id = dstq.TestId
--	WHERE  dsts.Id IN (SELECT os.SubjectId
--	                   FROM   @oldSteps AS os)
--	       AND dsta.IsCorrect = 1
--END
--GO
CREATE PROCEDURE dbo.get_site_test_normal_results
	@subjectId INT
AS
BEGIN
	DECLARE @testId INT
	SELECT TOP 1 @testId = dsts.TestId
	FROM   D_SITE_TEST_SUBJECT AS dsts
	WHERE  dsts.Id = @subjectId
	
	SELECT *
	FROM   D_SITE_TEST_ANSWER         AS dsta
	       JOIN D_SITE_TEST_QUESTION  AS dstq
	            ON  dstq.Id = dsta.QuestionId
	       JOIN D_SITE_TEST_SUBJECT   AS dsts
	            ON  dsts.Id = dsta.SubjectId
	       JOIN D_SITE_TEST           AS dst
	            ON  dst.Id = dstq.TestId
	            AND dst.Id = @testId
	WHERE  dsta.IsCorrect = 1
END
GO

/*******************************************
 * �������� ������ ���
 *******************************************/
IF OBJECT_ID(N'dbo.update_share_button', N'P') IS NOT NULL
    DROP PROCEDURE dbo.update_share_button;
GO
CREATE PROCEDURE dbo.update_share_button
	@id INT,
	@show BIT,
	@showCounter BIT
AS
BEGIN
	UPDATE D_SHARE_BUTTON
	SET    Show = @show,
	       ShowCounter = @showCounter,
	       DateUpdate = GETDATE()
	WHERE  Id = @id
	
	SELECT TOP(1) *
	FROM   D_SHARE_BUTTON  AS dlb
	       JOIN D_NET      AS dn
	            ON  dn.Id = dlb.NetId
	WHERE  dlb.Id = @id
END
GO

/*******************************************
 * ������ ������ ������
 *******************************************/
IF OBJECT_ID(N'dbo.get_share_buttons_list', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_share_buttons_list;
GO
CREATE PROCEDURE dbo.get_share_buttons_list
AS
BEGIN
	SELECT *
	FROM   D_SHARE_BUTTON  AS dlb
	       JOIN D_NET      AS dn
	            ON  dn.Id = dlb.NetId
	WHERE  dlb.Show = 1
END
GO

/*******************************************
 * ����� �����
 *******************************************/
IF OBJECT_ID(N'dbo.get_site_map', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_site_map;
GO
CREATE PROCEDURE dbo.get_site_map
AS
BEGIN
	SELECT dm.TitleUrl,
	       dm.DateCreate,
	       dm.DateUpdate,
	       dm.ModelCoreType,
	       dm.CategoryId
	FROM   DV_MATERIAL AS dm
	WHERE  dm.Show = 1
	       AND dm.DateOfPublication <= GETDATE()
	ORDER BY
	       dm.DateUpdate DESC
END
GO

/*******************************************
 * ���������� ���������� �� ����
 *******************************************/
IF OBJECT_ID(N'dbo.get_material_date_statistic', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_material_date_statistic;
GO
CREATE PROCEDURE dbo.get_material_date_statistic
	@mct INT
AS
BEGIN
	SELECT COUNT(DISTINCT dm.Id)         AS [Count],
	       CONVERT(DATE, dm.DateCreate)  AS DateCreate
	FROM   DV_MATERIAL                   AS dm
	       LEFT JOIN D_ARTICLE           AS da
	            ON  da.ModelCoreType = dm.ModelCoreType
	            AND da.Id = dm.Id
	       LEFT JOIN D_NEWS              AS dn
	            ON  dn.Id = dm.Id
	            AND dn.ModelCoreType = dm.ModelCoreType
	WHERE  dm.ModelCoreType = @mct
	GROUP BY
	       CONVERT(DATE, dm.DateCreate)
	ORDER BY
	       2
END
GO

/*******************************************
* ���������� ��������
*******************************************/
IF OBJECT_ID(N'dbo.get_request_date_statistic', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_request_date_statistic;
GO
CREATE PROCEDURE dbo.get_request_date_statistic
AS
BEGIN
	SET NOCOUNT ON
	
	DECLARE @dateEnd       DATETIME = GETDATE(),
	        @dateStart     DATETIME
	
	SET @dateStart = DATEADD(MONTH, -1, @dateEnd)
	
	SELECT COUNT(DISTINCT dr.Id)         AS [Count],
	       CONVERT(DATE, dr.DateCreate)  AS [DateCreate]
	FROM   D_REQUEST                     AS dr
	WHERE  dr.DateCreate BETWEEN @dateStart AND @dateEnd
	GROUP BY
	       CONVERT(DATE, dr.DateCreate)
	ORDER BY
	       2
END
GO

/*******************************************
* ���������� ��������� ��������
*******************************************/
IF OBJECT_ID(N'dbo.get_banners_statistic', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_banners_statistic;
GO
CREATE PROCEDURE dbo.get_banners_statistic
AS
BEGIN
	SET NOCOUNT ON
	
	DECLARE @date DATETIME = GETDATE()
	
	SELECT SUM(db.ClicksCount)  AS ClicksCount,
	       SUM(db.ShowsCount)   AS ShowsCount,
	       db.[Url]
	FROM   D_BANNER             AS db
	GROUP BY
	       db.[Url]
	ORDER BY
	       db.[Url]
END
GO

/*******************************************
 * ��������� ����������
 *******************************************/
IF OBJECT_ID(N'dbo.get_material_categories_by_mct', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_material_categories_by_mct;
GO
CREATE PROCEDURE dbo.get_material_categories_by_mct
	@mct INT
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT dmc.Id,
	       dmc.Title
	FROM   D_MATERIAL_CATEGORY AS dmc
	WHERE  dmc.ModelCoreType = @mct
END
GO

/*******************************************
 * �������� ������������� ��������� �� TitleUrl
 *******************************************/
IF OBJECT_ID(N'dbo.exists_material_by_title_url', N'P') IS NOT NULL
    DROP PROCEDURE dbo.exists_material_by_title_url;
GO
CREATE PROCEDURE dbo.exists_material_by_title_url
	@titleUrl NVARCHAR(255)
AS
BEGIN
	SET NOCOUNT ON
	
	DECLARE @result BIT = 0
	IF EXISTS(
	       SELECT TOP 1 dm.Id
	       FROM   DV_MATERIAL AS dm
	       WHERE  dm.TitleUrl = @titleUrl
	   )
	    SET @result = 1
	
	SELECT @result
END
GO

/*******************************************
 * �������� ������� ���������
 *******************************************/
IF OBJECT_ID(N'dbo.get_material_rating', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_material_rating;
GO
CREATE PROCEDURE dbo.get_material_rating
	@mid INT,
	@mct INT
AS
BEGIN
	DECLARE @result FLOAT
	SELECT @result = AVG(dr.[Value])
	FROM   D_RATING AS dr
	WHERE  dr.MaterialId = @mid
	       AND dr.ModelCoreType = @mct
	
	SELECT @result
END
GO

/*******************************************
 * �������� ������� ���������
 *******************************************/
IF OBJECT_ID(N'dbo.add_material_rating', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_material_rating;
GO
CREATE PROCEDURE dbo.add_material_rating
	@userId NVARCHAR(128),
	@value INT,
	@sessionId NVARCHAR(128),
	@mid INT,
	@mct INT
AS
BEGIN
	SET NOCOUNT ON
	
	IF NOT EXISTS (
	       SELECT *
	       FROM   D_RATING AS dr
	       WHERE  (dr.UserId = @userId OR dr.SessionId = @sessionId)
	              AND dr.MaterialId = @mid
	              AND dr.ModelCoreType = @mct
	   )
	BEGIN
	    INSERT INTO D_RATING
	      (
	        UserId,
	        [Value],
	        SessionId,
	        MaterialId,
	        ModelCoreType,
	        DateCreate
	      )
	    VALUES
	      (
	        @userId,
	        @value,
	        @sessionId,
	        @mid,
	        @mct,
	        GETDATE()
	      )
	END
	
	EXEC dbo.get_material_rating @mid,
	     @mct
END
GO

--SxAffiliateLink
/*******************************************
 * �������� ���������
 *******************************************/
IF OBJECT_ID(N'dbo.get_affiliate_link', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_affiliate_link;
GO
CREATE PROCEDURE dbo.get_affiliate_link
	@id UNIQUEIDENTIFIER
AS
BEGIN
	SELECT *
	FROM   D_AFFILIATE_LINK AS dal
	WHERE  dal.Id = @id
END
GO

/*******************************************
 * �������� ���������
 *******************************************/
IF OBJECT_ID(N'dbo.add_affiliate_link', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_affiliate_link;
GO
CREATE PROCEDURE dbo.add_affiliate_link
	@id UNIQUEIDENTIFIER,
	@desc NVARCHAR(MAX),
	@cc MONEY
AS
BEGIN
	DECLARE @date DATETIME = GETDATE()
	INSERT INTO D_AFFILIATE_LINK
	  (
	    Id,
	    [Description],
	    DateUpdate,
	    DateCreate,
	    ViewsCount,
	    ClickCost
	  )
	VALUES
	  (
	    NEWID(),
	    @desc,
	    @date,
	    @date,
	    0,
	    @cc
	  )
	EXEC dbo.get_affiliate_link @id
END
GO

/*******************************************
 * ������������� ���������
 *******************************************/
IF OBJECT_ID(N'dbo.update_affiliate_link', N'P') IS NOT NULL
    DROP PROCEDURE dbo.update_affiliate_link;
GO
CREATE PROCEDURE dbo.update_affiliate_link
	@id UNIQUEIDENTIFIER,
	@desc NVARCHAR(MAX),
	@cc MONEY
AS
BEGIN
	UPDATE D_AFFILIATE_LINK
	SET    [Description] = @desc,
	       DateUpdate = GETDATE(),
	       ClickCost = @cc
	WHERE  Id = @id
	
	EXEC dbo.get_affiliate_link @id
END
GO

/*******************************************
 * ������� ���������
 *******************************************/
IF OBJECT_ID(N'dbo.del_affiliate_link', N'P') IS NOT NULL
    DROP PROCEDURE dbo.del_affiliate_link;
GO
CREATE PROCEDURE dbo.del_affiliate_link
	@id UNIQUEIDENTIFIER
AS
BEGIN
	DELETE 
	FROM   D_AFFILIATE_LINK
	WHERE  Id = @id
END
GO

/*******************************************
 * �������� ���� �� ��������
 *******************************************/
IF OBJECT_ID(N'dbo.add_affiliate_link_view', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_affiliate_link_view;
GO
CREATE PROCEDURE dbo.add_affiliate_link_view
	@id UNIQUEIDENTIFIER
AS
BEGIN
	UPDATE D_AFFILIATE_LINK
	SET    ViewsCount = dal.ViewsCount + 1
	FROM   D_AFFILIATE_LINK AS dal
	WHERE  dal.Id = @id
END
GO

/*******************************************
 * ������ ����� ���������
 *******************************************/
IF OBJECT_ID(N'dbo.get_first_material_video', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_first_material_video;
GO
CREATE PROCEDURE dbo.get_first_material_video
	@mid INT,
	@mct INT
AS
BEGIN
	SELECT TOP(1) dv.* 
	FROM   D_VIDEO            AS dv
	       JOIN D_VIDEO_LINK  AS dvl
	            ON  dvl.VideoId = dv.Id
	            AND dvl.MaterialId = @mid
	            AND dvl.ModelCoreType = @mct
	ORDER BY
	       dv.DateCreate DESC
END
GO

/*******************************************
 * ������� ���������
 *******************************************/
IF OBJECT_ID(N'dbo.get_manuals_by_category', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_manuals_by_category;
GO
CREATE PROCEDURE dbo.get_manuals_by_category
	@cat NVARCHAR(MAX)
AS
BEGIN
	SELECT *
	FROM   D_MANUAL                  AS dm
	       JOIN DV_MATERIAL          AS dm2
	            ON  dm2.Id = dm.Id
	            AND dm2.ModelCoreType = dm.ModelCoreType
	       JOIN D_MATERIAL_CATEGORY  AS dmc
	            ON  dmc.Id = dm2.CategoryId
	WHERE  dm2.CategoryId = @cat
	       OR  dmc.ParentCategoryId = @cat
END
GO

/*******************************************
 * ������ ��� ���������
 *******************************************/
IF OBJECT_ID(N'dbo.get_material_cloud', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_material_cloud;
GO
CREATE PROCEDURE dbo.get_material_cloud
	@amount INT,
	@mid INT,
	@mct INT
AS
BEGIN
	SELECT TOP(@amount) x.Id,
	       x.Title,
	       SUM(x.[Count])  AS [Count],
	       (CASE WHEN SUM(x.IsCurrent) >= 1 THEN 1 ELSE 0 END) AS IsCurrent
	FROM   (
	           SELECT dmt.Id,
	                  dmt.Title,
	                  COUNT(1)          AS [Count],
	                  (
	                      CASE 
	                           WHEN (dmt.MaterialId = @mid OR @mid IS NULL)
	                      AND (dmt.ModelCoreType = @mct OR @mct IS NULL) THEN 1 
	                          ELSE 
	                          0 END
	                  )                 AS IsCurrent
	           FROM   D_MATERIAL_TAG    AS dmt
	                  JOIN DV_MATERIAL  AS dm
	                       ON  dm.Id = dmt.MaterialId
	                       AND dm.ModelCoreType = dmt.ModelCoreType
	                       AND dm.Show = 1
	                       AND dm.DateOfPublication <= GETDATE()
	           WHERE  dmt.ModelCoreType = @mct
	           GROUP BY
	                  dmt.Id,
	                  dmt.Title,
	                  dmt.MaterialId,
	                  dmt.ModelCoreType
	       )                  x
	GROUP BY
	       x.Id,
	       x.Title
	ORDER BY
	       x.Title
END
GO

/*******************************************
 * �������� ��� ������ ��������� 
 * ��� ����� ���� ���������� � �������� Id � ModelCoreType
 *******************************************/
IF OBJECT_ID(N'dbo.get_material_tag', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_material_tag;
GO
CREATE PROCEDURE dbo.get_material_tag
	@id NVARCHAR(128),
	@mct INT
AS
BEGIN
	SELECT *
	FROM   D_MATERIAL_TAG AS dmt
	WHERE  dmt.Id = @id
	       AND dmt.ModelCoreType = @mct
END
GO

/*******************************************
 * �������� ��� ������ ��������� 
 *******************************************/
IF OBJECT_ID(N'dbo.add_material_tag', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_material_tag;
GO
CREATE PROCEDURE dbo.add_material_tag
	@id NVARCHAR(128),
	@mid INT,
	@mct INT,
	@title NVARCHAR(100)
AS
BEGIN
	IF NOT EXISTS(
	       SELECT dmt.Id
	       FROM   D_MATERIAL_TAG AS dmt
	       WHERE  dmt.Id = @id
	              AND dmt.ModelCoreType = @mct
	   )
	BEGIN
	    DECLARE @date DATETIME = GETDATE()
	    INSERT INTO D_MATERIAL_TAG
	      (
	        Id,
	        MaterialId,
	        ModelCoreType,
	        DateUpdate,
	        DateCreate,
	        Title
	      )
	    VALUES
	      (
	        @id,
	        @mid,
	        @mct,
	        @date,
	        @date,
	        @title
	      )
	    
	    EXEC dbo.get_material_tag @id,
	         @mct
	END
END
GO

/*******************************************
 * ������� ��� ������ ���������
 *******************************************/
IF OBJECT_ID(N'dbo.del_material_tag', N'P') IS NOT NULL
    DROP PROCEDURE dbo.del_material_tag;
GO
CREATE PROCEDURE dbo.del_material_tag
	@id NVARCHAR(MAX),
	@mct INT
AS
BEGIN
	DELETE 
	FROM   D_MATERIAL_TAG
	WHERE  Id = @id
	       AND ModelCoreType = @mct
END
GO

/*******************************************
 * ������� ��������
 *******************************************/
IF OBJECT_ID(N'dbo.del_material', N'P') IS NOT NULL
    DROP PROCEDURE dbo.del_material;
GO
CREATE PROCEDURE dbo.del_material
	@mid INT,
	@mct INT
AS
	BEGIN TRANSACTION
	--������� �������� ���������
	--������� seo ��� ���������
	EXEC dbo.del_material_seo_tags @mid,
	     @mct
	--������� ��������
	DELETE 
	FROM   DV_MATERIAL
	WHERE  Id = @mid
	       AND ModelCoreType = @mct
	
	COMMIT TRANSACTION
GO

/*******************************************
 * ������� �������� �����
 *******************************************/
IF OBJECT_ID(N'dbo.del_seo_keywords', N'P') IS NOT NULL
    DROP PROCEDURE dbo.del_seo_keywords;
GO
CREATE PROCEDURE dbo.del_seo_keywords
	@id INT
AS
	DELETE 
	FROM   D_SEO_KEYWORD
	WHERE  Id = @id
GO

/*******************************************
 * ������� �����
 *******************************************/
IF OBJECT_ID(N'dbo.del_video', N'P') IS NOT NULL
    DROP PROCEDURE dbo.del_video;
GO
CREATE PROCEDURE dbo.del_video
	@videoId UNIQUEIDENTIFIER
AS
	DELETE 
	FROM   D_VIDEO
	WHERE  Id = @videoId
GO

/*******************************************
 * �������� ���� ���������
 *******************************************/
IF OBJECT_ID(N'dbo.add_material_like', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_material_like;
GO
CREATE PROCEDURE dbo.add_material_like
	@ld BIT,
	@mid INT,
	@mct INT
AS
	BEGIN TRANSACTION
	
	SET NOCOUNT ON
	
	UPDATE DV_MATERIAL
	SET    LikeUpCount = CASE 
	                          WHEN @ld = 1 THEN dm.LikeUpCount + 1
	                          ELSE dm.LikeUpCount
	                     END,
	       LikeDownCount = CASE 
	                            WHEN @ld = 1 THEN dm.LikeDownCount
	                            ELSE dm.LikeDownCount + 1
	                       END
	FROM   DV_MATERIAL AS dm
	WHERE  dm.Id = @mid
	       AND dm.ModelCoreType = @mct
	
	DECLARE @result INT
	SELECT @result = CASE 
	                      WHEN @ld = 1 THEN dm.LikeUpCount
	                      ELSE dm.LikeDownCount
	                 END
	FROM   DV_MATERIAL AS dm
	WHERE  dm.Id = @mid
	       AND dm.ModelCoreType = @mct
	
	SELECT @result
	
	COMMIT TRANSACTION
GO

/*******************************************
 * �������� ������� ���������
 *******************************************/
IF OBJECT_ID(N'dbo.get_like_materials', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_like_materials;
GO
CREATE PROCEDURE dbo.get_like_materials
	@amount INT,
	@mid INT,
	@mct INT
AS
	SELECT DISTINCT TOP(@amount)
	       dm.DateCreate,
	       dm.TitleUrl,
	       dm.Title,
	       dm.ModelCoreType,
	       SUBSTRING(dm.Foreword, 0, 200) AS Foreword,
	       dm.UserId,
	       anu.Id,
	       anu.NikName
	FROM   D_MATERIAL_TAG    AS dmt
	       JOIN DV_MATERIAL  AS dm
	            ON  dm.Id = dmt.MaterialId
	            AND dm.ModelCoreType = dmt.ModelCoreType
	            AND dm.Id NOT IN (@mid)
	       JOIN AspNetUsers  AS anu
	            ON  anu.Id = dm.UserId
	WHERE  dmt.Id IN (SELECT dmt2.Id
	                  FROM   D_MATERIAL_TAG AS dmt2
	                  WHERE  dmt2.MaterialId = @mid
	                         AND dmt2.ModelCoreType = @mct)
	ORDER BY
	       dm.DateCreate DESC
GO

/*******************************************
 * �������� ���� �����
 *******************************************/
IF OBJECT_ID(N'dbo.get_site_net', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_site_net;
GO
CREATE PROCEDURE dbo.get_site_net
	@netId INT
AS
BEGIN
	SELECT *
	FROM   D_SITE_NET              AS dsn
	       RIGHT OUTER JOIN D_NET  AS dn
	            ON  dn.Id = dsn.NetId
	WHERE  dn.Id = @netId
END
GO

/*******************************************
 * �������� ��������� ���� �����
 *******************************************/
IF OBJECT_ID(N'dbo.get_site_nets', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_site_nets;
GO
CREATE PROCEDURE dbo.get_site_nets
AS
BEGIN
	SELECT *
	FROM   D_SITE_NET              AS dsn
	       RIGHT OUTER JOIN D_NET  AS dn
	            ON  dn.Id = dsn.NetId
	WHERE  dsn.Show = 1
END
GO

/*******************************************
 * �������� ��� �������� ���� �����
 *******************************************/
IF OBJECT_ID(N'dbo.update_site_net', N'P') IS NOT NULL
    DROP PROCEDURE dbo.update_site_net;
GO
CREATE PROCEDURE dbo.update_site_net
	@netId INT,
	@url NVARCHAR(255),
	@show BIT
AS
	IF NOT EXISTS (
	       SELECT TOP 1 dsn.NetId
	       FROM   D_SITE_NET AS dsn
	       WHERE  dsn.NetId = @netId
	   )
	BEGIN
	    INSERT INTO D_SITE_NET
	      (
	        NetId,
	        [Url],
	        Show
	      )
	    VALUES
	      (
	        @netId,
	        @url,
	        @show
	      )
	END
	ELSE
	BEGIN
	    UPDATE D_SITE_NET
	    SET    [Url]     = @url,
	           Show      = @show
	    WHERE  NetId     = @netId
	END
	
	EXEC dbo.get_site_net @netId
GO

/*******************************************
 * �������� ������ �����������
 *******************************************/
IF OBJECT_ID(N'dbo.add_analizator_session', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_analizator_session;
GO
CREATE PROCEDURE dbo.add_analizator_session
	@userId NVARCHAR(128)
AS
BEGIN
	DECLARE @date DATETIME = GETDATE();
	
	INSERT INTO D_ANALIZATOR_SESSION
	  (
	    UserId,
	    DateCreate
	  )
	VALUES
	  (
	    @userId,
	    @date
	  )
	
	DECLARE @id INT = @@identity;
	SELECT TOP 1 *
	FROM   D_ANALIZATOR_SESSION AS das
	WHERE  das.Id = @id;
END
GO

/*******************************************
 * �������� Url ������
 *******************************************/
IF OBJECT_ID(N'dbo.add_analizator_session_url', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_analizator_session_url;
GO
CREATE PROCEDURE dbo.add_analizator_session_url
	@url NVARCHAR(255),
	@sessionId INT
AS
BEGIN
	DECLARE @date DATETIME = GETDATE();
	IF NOT EXISTS (
	       SELECT TOP 1 dau.Id
	       FROM   D_ANALIZATOR_URL AS dau
	       WHERE  dau.[Url] = @url
	              AND dau.AnalizatorSessionId = @sessionId
	   )
	BEGIN
	    INSERT INTO D_ANALIZATOR_URL
	      (
	        [Url],
	        AnalizatorSessionId,
	        StatusCode,
	        DateCreate
	      )
	    VALUES
	      (
	        @url,
	        @sessionId,
	        NULL,
	        @date
	      )
	END
END
GO