/************************************************************
 * Code formatted by SoftTree SQL Assistant © v6.5.278
 * Time: 21.06.2016 14:45:08
 ************************************************************/

/*******************************************
 * add site test
 *******************************************/
IF OBJECT_ID(N'dbo.add_site_test', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_site_test;
GO
CREATE PROCEDURE dbo.add_site_test
	@title NVARCHAR(200),
	@desc NVARCHAR(1000),
	@type INT,
	@titleUrl VARCHAR(255)
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
	        DateUpdate,
	        DateCreate,
	        TestType,
	        TitleUrl
	      )
	    VALUES
	      (
	        @title,
	        @desc,
	        GETDATE(),
	        GETDATE(),
	        @type,
	        @titleUrl
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
 * delete site test
 *******************************************/
IF OBJECT_ID(N'dbo.del_site_test', N'P') IS NOT NULL
    DROP PROCEDURE dbo.del_site_test;
GO
CREATE PROCEDURE dbo.del_site_test
	@testId INT
AS
	DELETE 
	FROM   D_SITE_TEST
	WHERE  Id = @testId
GO

/*******************************************
 * add site test block
 *******************************************/
IF OBJECT_ID(N'dbo.add_site_test_block', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_site_test_block;
GO
CREATE PROCEDURE dbo.add_site_test_block
	@testId INT,
	@title NVARCHAR(200),
	@desc NVARCHAR(1000)
AS
BEGIN
	IF NOT EXISTS (
	       SELECT TOP(1) *
	       FROM   D_SITE_TEST_BLOCK AS dstb
	       WHERE  dstb.TestId = @testId
	              AND dstb.Title = @title
	   )
	BEGIN
	    INSERT INTO D_SITE_TEST_BLOCK
	      (
	        TestId,
	        Title,
	        [Description],
	        DateUpdate,
	        DateCreate
	      )
	    VALUES
	      (
	        @testId,
	        @title,
	        @desc,
	        GETDATE(),
	        GETDATE()
	      )
	    
	    DECLARE @id INT
	    SET @id = @@identity
	    
	    SELECT TOP(1) *
	    FROM   D_SITE_TEST_BLOCK AS dstb
	    WHERE  dstb.Id = @id
	END
END
GO
 
 /*******************************************
 * delete site test block
 *******************************************/
IF OBJECT_ID(N'dbo.del_site_test_block', N'P') IS NOT NULL
    DROP PROCEDURE dbo.del_site_test_block;
GO
CREATE PROCEDURE dbo.del_site_test_block
	@blockId INT
AS
	DELETE 
	FROM   D_SITE_TEST_BLOCK
	WHERE  Id = @blockId
GO

 /*******************************************
 * add site test question
 *******************************************/
IF OBJECT_ID(N'dbo.add_site_test_question', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_site_test_question;
GO
CREATE PROCEDURE dbo.add_site_test_question
	@blockId INT,
	@text NVARCHAR(400),
	@isCorrect BIT
AS
BEGIN
	IF NOT EXISTS (
	       SELECT TOP(1) *
	       FROM   D_SITE_TEST_QUESTION AS dstq
	       WHERE  dstq.BlockId = @blockId
	              AND dstq.[Text] = @text
	   )
	BEGIN
	    INSERT INTO D_SITE_TEST_QUESTION
	      (
	        BlockId,
	        [Text],
	        IsCorrect,
	        DateUpdate,
	        DateCreate
	      )
	    VALUES
	      (
	        @blockId,
	        @text,
	        @isCorrect,
	        GETDATE(),
	        GETDATE()
	      )
	    
	    DECLARE @id INT
	    SET @id = @@identity
	    
	    SELECT TOP(1) *
	    FROM   D_SITE_TEST_QUESTION AS dstq
	    WHERE  dstq.Id = @id
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
	DELETE 
	FROM   D_SITE_TEST_QUESTION
	WHERE  Id = @questionId
GO