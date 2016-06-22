/************************************************************
 * Code formatted by SoftTree SQL Assistant © v6.5.278
 * Time: 22.06.2016 12:11:17
 ************************************************************/

/*******************************************
 * create architect user
 *******************************************/
IF NOT EXISTS (
       SELECT TOP(1) *
       FROM   AspNetUsers AS anu
       WHERE  anu.Email = 'simlex.dev.2014@gmail.com'
   )
BEGIN
    INSERT INTO AspNetUsers
      (
        Id,
        DateCreate,
        DateUpdate,
        NikName,
        AvatarId,
        Email,
        EmailConfirmed,
        PasswordHash,
        SecurityStamp,
        PhoneNumber,
        PhoneNumberConfirmed,
        TwoFactorEnabled,
        LockoutEndDateUtc,
        LockoutEnabled,
        AccessFailedCount,
        UserName
      )
    VALUES
      (
        'f1dec2e7-cc29-47da-9195-0d90041bf65b',
        GETDATE(),
        GETDATE(),
        'simlex',
        NULL,
        'simlex.dev.2014@gmail.com',
        1,
        'ADAI4m7/BAIvv/oQFNTRhdSTjegJvd9Llir6z/zFfSvJhjbxhNDsTKLzCsrkt8W9Pw==',
        '63a46c30-91ef-4401-8cf3-8cfcc483ab7f',
        '+79376376582',
        0,
        0,
        NULL,
        0,
        0,
        'simlex.dev.2014@gmail.com'
      )
END
GO

/*******************************************
 * create user roles
 *******************************************/
DECLARE @roles TABLE(
            Id NVARCHAR(128),
            [Name] NVARCHAR(256),
            [Description] NVARCHAR(MAX)
        )

INSERT INTO @roles
VALUES
  (
    '159bf25a-2ed1-43f5-99aa-cbec03283f58',
    'author-news',
    'Автор новостей'
  )
  
INSERT INTO @roles
VALUES
  (
    '547d3cfb-4304-4597-97e1-67fd5de889dd',
    'user',
    'Обычный пользователь сайта'
  )
  
INSERT INTO @roles
VALUES
  (
    '58c37506-99d6-47d5-b287-8c170d4b3963',
    'smm',
    'Social Media Manager'
  )
  
INSERT INTO @roles
VALUES
  (
    '694b7d6b-b1c1-4736-b435-fc6fb1e41fa9',
    'admin',
    'Администратор сайта'
  )
  
INSERT INTO @roles
VALUES
  (
    '6a746901-69c6-40e4-a6bf-4a6961e456d5',
    'seo',
    'Специалист по SEO'
  )
  
INSERT INTO @roles
VALUES
  (
    '89ffad5b-d477-4aca-99bc-297ceec0265b',
    'author-article',
    'Автор статей'
  )
  
INSERT INTO @roles
VALUES
  (
    'a1b516df-99fc-4e35-8d39-2d2123da134e',
    'photo-redactor',
    'Редактор изображений'
  )
  
INSERT INTO @roles
VALUES
  (
    'AF5D1E6D-8608-4F8F-9D84-1E6E4FE7A7DC',
    'architect',
    'Суперадмин сайта (разработчик)'
  )
  
INSERT INTO @roles
VALUES
  (
    'c96994d0-4bb8-4bd3-882c-36d4f67a28e0',
    'video-redactor',
    'Редактор видео'
  )
  
DECLARE @roleId       NVARCHAR(128),
        @roleName     NVARCHAR(256),
        @roleDesc     NVARCHAR(MAX)

DECLARE c CURSOR  
FOR
    SELECT r.Id,
           r.Name,
           r.[Description]
    FROM   @roles AS r

  OPEN c
  FETCH NEXT FROM c INTO @roleId, @roleName, @roleDesc
WHILE @@FETCH_STATUS = 0
BEGIN
    IF NOT EXISTS (
           SELECT *
           FROM   AspNetRoles AS anr
           WHERE  anr.Name = @roleName
       )
    BEGIN
        INSERT INTO AspNetRoles
          (
            Id,
            NAME,
            [Description],
            Discriminator
          )
        VALUES
          (
            @roleId,
            @roleName,
            @roleDesc,
            'SxAppRole'
          )
        
        INSERT INTO AspNetUserRoles
          (
            UserId,
            RoleId
          )
        VALUES
          (
            'f1dec2e7-cc29-47da-9195-0d90041bf65b',
            @roleId
          )
    END
    
    FETCH NEXT FROM c INTO @roleId, @roleName, @roleDesc
END
  CLOSE c
  DEALLOCATE c
  GO
  